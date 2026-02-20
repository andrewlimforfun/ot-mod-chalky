using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using Chalky.Core.Commands;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.IO;
using BepInEx.Logging;

namespace Chalky.Patches
{
    [HarmonyPatch(typeof(QuadPainterGPU))]

    public class QuadPainterGPUPatch
    {
        static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Chalky.QuadPainterGPUPatch");

        // cached once at startup — avoids repeated reflection lookups per paint call
        private static readonly System.Reflection.FieldInfo _pixelsField =
            typeof(QuadPainterGPU).GetField("_pixelsToUpdate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // private void PaintOnTexture(Vector2 uv, int colIndex, bool isErase, bool isBigErase)
        [HarmonyPatch("PaintOnTexture")]
        [HarmonyPrefix]
        public static bool PaintOnTexturePrefix(QuadPainterGPU __instance, Vector2 uv, int colIndex, bool isErase, bool isBigErase)
        {
            // if chalky feature is disabled, do not interfere with the painting logic
            if (Plugin.EnableFeature == null || Plugin.EnableFeature.Value == false)
            {
                return true;
            }

            int size = Plugin.chalkySize;

            // on erase mode, run the original method unchanged
            if (isErase)
            {
                return true;
            }

            // replicate PaintOnTexture logic with an expanded size x size block
            Vector2Int gridSize = __instance.gridSize;
            
            // Supress warning Harmony003 : Harmony non-ref patch parameters modified
            // because we aren't actually modifying the input UVs, just using them to calculate the affected grid cells. 
            // The original UV parameters are still used as intended for the painting logic, we're just expanding the area of effect based on the chalkySize config.
            #pragma warning disable Harmony003 
            // x coordinate
            float uvx = uv.x;
            // y coordinate
            float uvy = uv.y;
            #pragma warning restore Harmony003 

            // calculate the grid cell coordinates based on the uv coordinates input
            // see original method UVToGrid() which converts Vector2 to Vector2Int 
            int gx = Mathf.Clamp(Mathf.FloorToInt(uvx * (float)gridSize.x), 0, gridSize.x - 1);
            int gy = Mathf.Clamp(Mathf.FloorToInt(uvy * (float)gridSize.y), 0, gridSize.y - 1);

            // pre-clamp loop bounds once — eliminates per-iteration branch and intermediate Vector2Int allocs
            int xEnd = Mathf.Min(gx + size, gridSize.x);
            int yEnd = Mathf.Min(gy + size, gridSize.y);

            var pixelsToUpdate = (Dictionary<Vector2Int, Color>)_pixelsField.GetValue(__instance);
            Color paintColor =  Plugin.chalkyColor ?? MonoSingleton<DrawingManager>.I.GetColor(colIndex);
            int colorVal = colIndex + 1;

            for (int x = gx; x < xEnd; x++)
            {
                // hoist column lookup out of the inner loop
                IntList col = __instance.PaintColors[x];
                for (int y = gy; y < yEnd; y++)
                {
                    if (col.Ints[y] != colorVal)
                    {
                        col.Ints[y] = colorVal;
                        pixelsToUpdate[new Vector2Int(x, y)] = paintColor;
                    }
                }
            }

            // skip original since we've painted with the expanded size
            return false;
        }



    }
}
