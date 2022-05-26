# ScpsInfoDisplay
When SCP spawns, displays information about players who are in the SCP team. Possible integration with custom roles.

## How to install plugin?
Put ScpsInfoDisplay.dll under the release tab into %appdata%\EXILED\Plugins (Windows) or .config/EXILED/Plugins (Linux) folder.

## String variables
| Variable  | Value |
| ------------- | ------------- |
| `%health%` | Player's health points | 
| `%arhealth%`  | Player's artifical health points |
| `%helathpercent%`  | Player's health in percent |
| `%generators%`  | Engaged generators count  |
| `%engaging%`  | Count of generators that are currently engaging (format: (+count)) |

Feel free to use this variables in display strings.

## Default configs
```yaml
scpsinfodisplay:
# Is the plugin enabled?
  is_enabled: true
  # Display strings. Format: Role, display string.
  display_strings:
    Scp106: <color=#D51D1D>SCP-106 [%healthpercent%%]</color>
    Scp049: <color=#D51D1D>SCP-049 [%healthpercent%%]</color>
    Scp079: <color=#D51D1D>SCP-079 [%generators%%engaging%/3]</color>
    Scp096: <color=#D51D1D>SCP-096 [%healthpercent%%]</color>
    Scp173: <color=#D51D1D>SCP-173 [%healthpercent%%]</color>
    Scp93953: <color=#D51D1D>SCP-939-53 [%healthpercent%%]</color>
    Scp93989: <color=#D51D1D>SCP-939-89 [%healthpercent%%]</color>
  # Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string
  custom_roles_integrations: {}
```
