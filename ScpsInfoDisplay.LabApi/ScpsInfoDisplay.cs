using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace ScpsInfoDisplay.LabApi
{
    public class ScpsInfoDisplay : Plugin<Config>
    {
        public override string Name => "ScpsInfoDisplay";

        public override string Author => "bladuk";

        public override string Description => "SCP: SL plugin that displays information about SCPs in player's team.";

        public override Version Version => new(3, 0, 0);

        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

        public static ScpsInfoDisplay Singleton;

        private EventHandlers _eventHandlers;
        
        public override void Enable()
        {
            Singleton = this;
            _eventHandlers = new();
            CustomHandlersManager.RegisterEventsHandler(_eventHandlers);
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(_eventHandlers);
            _eventHandlers = null;
        }
    }
}