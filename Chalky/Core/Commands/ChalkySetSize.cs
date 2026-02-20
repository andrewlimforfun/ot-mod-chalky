using System;
using Chalky;

namespace Chalky.Core.Commands
{
    public class ChalkySetSize : IChatCommand
    {
        public const string CMD = "chalkysetsize";
        public string Name => CMD;
        public string ShortName => "css";
        public string Description => "Set the size of the chalk. Not persistent, will reset to default on game restart. Game Default: 2. Usage: /chalkysetsize [size]";

        public void Execute(string[] args)
        {
            if (Plugin.EnableFeature == null)
            {
                return;
            }

            if (args.Length == 0)
            {
                Plugin.chalkySize = 2; // reset to default
                ChatUtils.AddGlobalNotification("Reset chalk size.");
                return;
            }

            if (int.TryParse(args[0], out int newSize))
            {
                Plugin.chalkySize = newSize;
                ChatUtils.AddGlobalNotification($"Chalk size is now set to {Plugin.chalkySize}.");
            }
            else
            {
                ChatUtils.AddGlobalNotification("Invalid size value. Please provide a valid integer.");
            }
        }
    }
}
