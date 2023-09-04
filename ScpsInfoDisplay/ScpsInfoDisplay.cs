using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;

namespace ScpsInfoDisplay
{
    internal class ScpsInfoDisplay : Plugin<Config>
    {
        public override string Prefix => "scpsinfodisplay";
        public override string Name => "ScpsInfoDisplay";
        public override string Author => "Vicious Vikki";
        public override Version Version { get; } = new Version(2, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(8, 1, 0);
        public static ScpsInfoDisplay Singleton = new ScpsInfoDisplay();
        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers();

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            _eventHandlers = null;
            Singleton = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            Server.RoundStarted += _eventHandlers.OnRoundStarted;
        }

        private void UnregisterEvents()
        {
            Server.RoundStarted -= _eventHandlers.OnRoundStarted;
        }
    }
}