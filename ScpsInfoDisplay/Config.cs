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
            { RoleTypeId.Scp049, "<color=#FF0000>SCP-049 [%healthpercent%% Zombies: %zombies%] </color>" },
            { RoleTypeId.Scp079, "<color=#FF00000>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>" },
            { RoleTypeId.Scp096, "<color=#FF0000>SCP-096 [%healthpercent%%] </color>" },
            { RoleTypeId.Scp106, "<color=#FF0000>SCP-106 [%healthpercent%%] Vigor: %106vigor%% </color>" },
            { RoleTypeId.Scp173, "<color=#FF0000>SCP-173 [%healthpercent%%] </color>" },
            { RoleTypeId.Scp939, "<color=#FF0000>SCP-939 [%healthpercent%%] </color>" },
        };
        [Description("Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();
        [Description("Display string alignment. Possible values: Left, Center, Right.")]
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Right;
        [Description("Display text position offset.")]
        public sbyte TextPositionOffset { get; set; } = 30;
        [Description("The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.")]
        public string PlayersMarker { get; set; } = "<color=#D7E607>You:</color>";
        [Description("Display debug messages in server console?")]
        public bool Debug { get; set; } = false;
    }
}