using PersistentMultiplayer.Framework;
using StardewModdingAPI;

namespace PersistentMultiplayer.Integrations.GenericModConfigMenu
{
    internal class GenericModConfigMenuIntegration
    {
        private ModConfig _modConfig;
        private readonly IManifest _modManifest;
        private readonly IGenericModConfigMenuApi? _configMenu;

        public GenericModConfigMenuIntegration(IManifest modManifest, IModRegistry modRegistry, ModConfig modConfig)
        {
            this._modConfig = modConfig;
            this._modManifest = modManifest;
            this._configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        public void SetupGenericModConfigMenu(Action save)
        {
            if (this._configMenu is null) {
                return;
            }
            
            this._configMenu.Register(
                mod: this._modManifest,
                reset: () => this._modConfig = new ModConfig(),
                save: save
            );
            
            this._configMenu.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Irreversible Choices",
                tooltip: () => "These options cannot be reversed once they take effect, for example, " +
                               "if your game unlocks the cave you are stuck with your choice of fruit bats or " +
                               "mushrooms for the rest of the game."
            );
            
            this.AddEnumDropdownBox(
                name: "Cave Type",
                tooltip: "What type of cave you would like after earning 25,000g.",
                options: new Dictionary<string, CaveType>  {
                    { "Mushrooms", CaveType.Mushroom },
                    { "Fruit Bats", CaveType.FruitBat }
                },
                getValue: () => this._modConfig.ServerSettings.CaveType,
                setValue: value => this._modConfig.ServerSettings.CaveType = value
            );
            
            this.AddEnumDropdownBox(
                name: "Host Home Upgrade",
                tooltip: "What the host's house upgrade level should be at the start of the game.",
                options: new Dictionary<string, HouseUpgradeLevel>  {
                    { "None", HouseUpgradeLevel.None },
                    { "Level 1: Adds Kitchen", HouseUpgradeLevel.Kitchen },
                    { "Level 2: Adds Extra Rooms", HouseUpgradeLevel.KitchenAndExtraRooms },
                    { "Level 3: Adds Cellar", HouseUpgradeLevel.KitchenExtraRoomsAndCellar }
                },
                getValue: () => this._modConfig.ServerSettings.HouseUpgradeLevel,
                setValue: value => this._modConfig.ServerSettings.HouseUpgradeLevel = value
            );
            
            this._configMenu.AddTextOption(
                mod: this._modManifest,
                name: () => "Pet Name",
                tooltip: () => "The name of the farm pet, maximum 12 characters.",
                getValue: () => this._modConfig.ServerSettings.PetName,
                setValue: (value) => this._modConfig.ServerSettings.PetName = value
            );
            
            this._configMenu.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Server Settings",
                tooltip: () => "These settings can be changed at any time and impact how the server behaves for all players."
            );
            
            this._configMenu.AddBoolOption(
                mod: this._modManifest,
                name: () => "Farmhands Can Pause",
                tooltip: () => "Whether farmhands (connected players) can pause and unpause the game whilst people are playing (the game will still automatically pause when no players are connected).",
                getValue: () => this._modConfig.ServerSettings.FarmhandsCanPause,
                setValue: value => this._modConfig.ServerSettings.FarmhandsCanPause = value
            );
            
            this._configMenu.AddTextOption(
                mod: this._modManifest,
                name: () => "Host Character Sleep Time",
                tooltip: () => "What time the host character will automatically go to sleep when the game is in server mode. (Not being controlled by a player)",
                getValue: () => this._modConfig.ServerSettings.HostCharacterSleepTime,
                setValue: value => this._modConfig.ServerSettings.HostCharacterSleepTime = value,
                allowedValues: this.GetHostCharacterSleepTimes().ToArray()
            );
        }

        private void AddEnumDropdownBox<TEnum>(string name, string tooltip, Dictionary<string, TEnum> options, Func<TEnum> getValue, Action<TEnum> setValue)
        {
            this._configMenu?.AddTextOption(
                mod: this._modManifest,
                name: () => name,
                tooltip: () => tooltip,
                getValue: () => options.FirstOrDefault(x => EqualityComparer<TEnum>.Default.Equals(x.Value, getValue())).Key,
                setValue: value => {
                    if (!options.TryGetValue(value, out var parsedValue)) {
                        throw new ArgumentException($"The value '{value}' is not defined in the enum.", nameof(value));
                    }

                    setValue(parsedValue);
                },
                allowedValues: options.Keys.ToArray()
            );
        }
        
        private List<string> GetHostCharacterSleepTimes()
        {
            var times = new List<string>();

            for (var hour = 18; hour < 26; hour++) {
                for (var minute = 0; minute < 60; minute += 10) {
                    var formatHour = hour switch {
                        24 => 0,
                        25 => 1,
                        _ => hour
                    };
                    times.Add($"{formatHour:D2}{minute:D2}");
                }
            }

            return times;
        }
    }
}
