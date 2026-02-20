using System;
using System.Drawing;
using Chalky;
using UnityEngine;

namespace Chalky.Core.Commands
{
    public class DisabledChalkySetColor //: IChatCommand
    {
        public const string CMD = "chalkysetcolor";
        public string Name => CMD;
        public string ShortName => "csc";
        public string Description => "Set the color of the chalk. Not persistent, will reset to default on game restart. Usage: /chalkysetcolor [hex color]";

        public void Execute(string[] args)
        {
            if (Plugin.EnableFeature == null)
            {
                return;
            }

            if (args.Length == 0)
            {
                ChatUtils.AddGlobalNotification("Reset chalk color.");
                Plugin.chalkyColor = null;
                return;
            }

            if (ColorUtility.TryParseHtmlString(args[0], out UnityEngine.Color parsedColor) == false)
            {
                ChatUtils.AddGlobalNotification("Invalid color value. Please provide a valid hex color code (e.g. #FF0000 for red).");
                return;
            }

            Plugin.chalkyColor = parsedColor;
            ChatUtils.AddGlobalNotification($"Chalk color is now set to {args[0]}.");

        }
    }
}
