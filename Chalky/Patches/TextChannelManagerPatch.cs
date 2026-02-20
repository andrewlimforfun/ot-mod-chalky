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
    [HarmonyPatch(typeof(TextChannelManager))]

    public class TextChannelManagerPatch
    {
        static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Chalky.TextChannelManagerPatch");

        [HarmonyPatch("OnEnterPressed")]
        [HarmonyPrefix]
        public static void OnEnterPressedPrefix()
        {
            string text = MonoSingleton<UIManager>.I.MessageInput.text;

            // basic validation to only process potential commands - this will allow normal chat messages to go through without interference
            if (string.IsNullOrEmpty(text) || text.StartsWith('/') == false || Plugin.EnableFeature == null)
            {
                return;
            }

            // process commands only if feature is enabled or its a feature toggle command, 
            // so that users can still toggle the feature on if they have it off
            // unrecognized commands will be ignored and treated as normal chat messages
            if (Plugin.EnableFeature.Value == true || text.Contains(ChalkyToggleCommand.CMD))
            {
                bool isProcessed = Plugin.CommandProcessor?.ProcessInput(text) ?? false;

                // only clean the command from chat if the ShowCommand option is disabled
                // otherwise the lock management and unselection can happen in the real OnEnterPressed
                if (Plugin.ShowCommand?.Value == false && isProcessed)
                {
                    ChatUtils.CleanCommand();
                }
            }

        }
    }
}
