using Chalky;

namespace Chalky.Core.Commands
{
    public class ShowChalkyCommand : IChatCommand
    {
        public const string CMD = "chalkyshowcommand";
        public string Name => CMD;
        public string ShortName => "cshc";
        public string Description => "Toggle Chalky show/hide Chalky command in chat." +
            "If enabled, user command such as '/chalkysetcolor' will be shown in chat. Current: " + 
            (Plugin.ShowCommand?.Value == true ? "shown" : "hidden");

        public void Execute(string[] args)
        {
            if (Plugin.ShowCommand == null)
            {
                return;
            }

            Plugin.ShowCommand.Value = !Plugin.ShowCommand.Value;
            ChatUtils.AddGlobalNotification($"Chalky user command is now {(Plugin.ShowCommand.Value ? "shown" : "hidden")}.");
        }
    }
}
