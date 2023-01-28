using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace ScpsInfoDisplay
{
    internal sealed class ScpsInfoDisplay
    {
        public static ScpsInfoDisplay Singleton;

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("ScpsInfoDisplay", "2.0.1", "A plugin that displays information about SCPs in player's team.", "bladuk.")]
        private void LoadPlugin()
        {
            Singleton = this;
            
            EventManager.RegisterEvents<EventHandlers>(this);
        }
    }
}