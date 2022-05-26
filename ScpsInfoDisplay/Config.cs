using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ScpsInfoDisplay
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Display strings. Format: Role, display string.")]
        public Dictionary<RoleType, string> DisplayStrings { get; set; } = new Dictionary<RoleType, string>()
        {
            { RoleType.Scp106, "<color=#D51D1D>SCP-106 [%healthpercent%%]</color>" },
            { RoleType.Scp049, "<color=#D51D1D>SCP-049 [%healthpercent%%]</color>" },
            { RoleType.Scp079, "<color=#D51D1D>SCP-079 [%generators%%engaging%/3]</color>" },
            { RoleType.Scp096, "<color=#D51D1D>SCP-096 [%healthpercent%%]</color>" },
            { RoleType.Scp173, "<color=#D51D1D>SCP-173 [%healthpercent%%]</color>" },
            { RoleType.Scp93953, "<color=#D51D1D>SCP-939-53 [%healthpercent%%]</color>" },
            { RoleType.Scp93989, "<color=#D51D1D>SCP-939-89 [%healthpercent%%]</color>" }
        };
        [Description("Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();
    }
}