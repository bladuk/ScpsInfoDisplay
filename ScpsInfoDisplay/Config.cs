using Exiled.API.Interfaces;
using PlayerRoles;
using ScpsInfoDisplay.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScpsInfoDisplay
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Display strings. Format: Role, display string.")]
        public Dictionary<RoleTypeId, string> DisplayStrings { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.Scp049, "<color=#FF0000><size=30>SCP-049</color> [<color=#19FF40>%healthpercent%%</color>]\n<color=#19B2FF>Zombies: %zombies%</color></size>" },
            { RoleTypeId.Scp079, "<color=#FF0000><size=30>SCP-079</color> <color=#19FF40>Generators:</color> [<color=#19FF40>%generators%</color><color=#FF0000>%engaging%</color><color=#19FF40>/3</color>]\n <color=#19B2FF>Level: %079level%</color> <color=#FF19FF>Energy: %079energy%</color></size>" },
            { RoleTypeId.Scp096, "<color=#FF0000><size=30>SCP-096</color> [<color=#19FF40>%healthpercent%%</color>] </size>" },
            { RoleTypeId.Scp106, "<color=#FF0000><size=30>SCP-106</color> [<color=#19FF40>%healthpercent%%</color>] <color=#19B2FF>Vigor: %106vigor%% </size></color>" },
            { RoleTypeId.Scp173, "<color=#FF0000><size=30>SCP-173</color> [<color=#19FF40>%healthpercent%%</color>] </size>" },
            { RoleTypeId.Scp939, "<color=#FF0000><size=30>SCP-939</color> [<color=#19FF40>%healthpercent%%</color>] </size>" },
        };
        [Description("Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();
        [Description("Display string alignment. Possible values: Left, Center, Right.")]
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Right;
        [Description("Display text position offset.")]
        public sbyte TextPositionOffset { get; set; } = 30;
        [Description("The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.")]
        public string PlayersMarker { get; set; } = "<color=#D7E607><size=30>You: </size></color>";
        [Description("Display debug messages in server console?")]
        public bool Debug { get; set; } = false;
    }
}