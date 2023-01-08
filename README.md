# ScpsInfoDisplay [![Downloads](https://img.shields.io/github/downloads/bladuk/ScpsInfoDisplay/total.svg)]
When SCP spawns, displays information about players who are in the SCP team. Possible integration with custom roles.

## How to install plugin?
Put ScpsInfoDisplay.dll under the release tab into %appdata%\EXILED\Plugins (Windows) or .config/EXILED/Plugins (Linux) folder.

## String variables
| Variable  | Meaning |
| ------------- | ------------- |
| `%health%` | Player's health points | 
| `%healthpercent%`  | Player's health in percent |
| `%generators%`  | Engaged generators count  |
| `%engaging%`  | Count of generators that are currently engaging (format: (+count)) |
| `%zombies%`  | Count of SCP-049-2 |
| `%distance%` | Distance between the player who seeng the list and SCP in meters |
| `%079level%` | Level of SCP-079 |
| `%079experience%` | Experience points of SCP-079 |
| `%079energy%` | Aux power of SCP-079 |
| `%106vigor%` | Vigor amount of SCP-106 |

Feel free to use this variables in display strings.

## Default configs
```yaml
scpsinfodisplay:
# Is the plugin enabled?
  is_enabled: true
  # Display strings. Format: Role, display string.
  display_strings:
    Scp106: ;<color=#D51D1D>SCP-106 [%healthpercent%%] Vigor: %106vigor%% %distance%</color>'
    Scp049: '<color=#D51D1D>SCP-049 [%healthpercent%% Zombies: %zombies%] %distance%</color>'
    Scp079: '<color=#D51D1D>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>'
    Scp096: <color=#D51D1D>SCP-096 [%healthpercent%%] %distance%</color>
    Scp173: <color=#D51D1D>SCP-173 [%healthpercent%%] %distance%</color>
    Scp939: <color=#D51D1D>SCP-939 [%healthpercent%%] %distance%</color>
  # Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.
  custom_roles_integrations: {}
  # Display string alignment. Possible values: left, center, right.
  text_alignment: right
  # Display text position offset.
  text_position_offset: 30
  # The player seeing the list will be highlighted with the special marker to the left
  mark_player_in_list: false
  # Player's marker
  players_marker: <color=#D51D1D>You --></color>
  # Display debug messages in server console?
  debug: false
```
