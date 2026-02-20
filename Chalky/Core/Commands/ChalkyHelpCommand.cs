using System;
using System.Collections.Generic;

namespace Chalky.Core.Commands
{
    public class ChalkyHelpCommand : IChatCommand
    {
        public const string CMD = "chalkyhelp";
        public string Name => CMD;
        public string ShortName => "fh";
        public string Description => "Lists all available commands.";

        SortedSet<IChatCommand>? commands;

        public void Initialize(IEnumerable<IChatCommand> values)
        {
            this.commands = new SortedSet<IChatCommand>(values);
        }

        public void Execute(string[] args)
        {
            if (commands == null)
            {
                ChatUtils.AddGlobalNotification("No commands available!");
                return;
            }

            // list all commands and descriptions
            ChatUtils.AddGlobalNotification($"Chalky Version ({Plugin.ModVersion}).");
            ChatUtils.AddGlobalNotification("Available commands:");
            foreach (var cmd in commands)
            {
                ChatUtils.AddGlobalNotification($"/{cmd.Name} (/{cmd.ShortName}) :: {cmd.Description}");
            }

        }

    }
}
