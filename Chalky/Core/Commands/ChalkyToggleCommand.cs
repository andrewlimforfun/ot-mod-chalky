using Chalky;

namespace Chalky.Core.Commands
{
    public class ChalkyToggleCommand : IChatCommand
    {
        public const string CMD = "chalkytoggle";
        public string Name => CMD;
        public string ShortName => "ct";
        public string Description => "Toggle Chalky feature on/off. ";

        public void Execute(string[] args)
        {
            if (Plugin.EnableFeature == null)
            {
                return;
            }

            Plugin.EnableFeature.Value = !Plugin.EnableFeature.Value;
            ChatUtils.AddGlobalNotification($"Chalky feature is now {(Plugin.EnableFeature.Value ? "enabled" : "disabled")}.");
        }
    }
}
