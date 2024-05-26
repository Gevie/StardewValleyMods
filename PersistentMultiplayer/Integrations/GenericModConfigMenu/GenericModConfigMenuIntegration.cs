using PersistentMultiplayer.Framework.Configuration;
using PersistentMultiplayer.Framework.Configuration.Constants;
using PersistentMultiplayer.Framework.Helper;
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
            
            this.AddLocalServerModeInformation();
            this.AddDedicatedServerModeInformation();
            this.AddIrreversibleChoicesSection();
            this.AddServerSettingsSection();
            this.AddKeybindsSection();
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

        private void AddLocalServerModeInformation()
        {
            this._configMenu?.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Local Server Mode"
            );
            
            this._configMenu?.AddParagraph(
                mod: this._modManifest,
                text: () => "This is best used when you are hosting the server from your own computer and intend to " +
                            "play as the host character when you're not away. The server will still automate your " +
                            "character when you are away or pause the game when no players are actively playing."
            );
        }

        private void AddDedicatedServerModeInformation()
        {
            this._configMenu?.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Dedicated Server Mode"
            );
            
            this._configMenu?.AddParagraph(
                mod: this._modManifest,
                text: () => "This is best used when you are hosting the server remotely or within a container, so it " +
                            "is not easy for you to control the host character. The host character be fully automated " +
                            "and the mod will attempt to keep the host character out of the game as much as possible."
            );
        }

        private void AddIrreversibleChoicesSection()
        {
            this._configMenu?.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Irreversible Choices",
                tooltip: () => "These options cannot be reversed once they take effect, for example, " +
                               "if your game unlocks the cave you are stuck with your choice of fruit bats or " +
                               "mushrooms for the rest of the game."
            );
            
            this.AddEnumDropdownBox(
                name: "Server Mode",
                tooltip: "The server mode to use, both will act as a server when nobody is playing or the host is away.",
                options: new Dictionary<string, ServerMode>  {
                    { "Local", ServerMode.Local },
                    { "Dedicated", ServerMode.Dedicated }
                },
                getValue: () => this._modConfig.ServerSettings.ServerMode,
                setValue: value => this._modConfig.ServerSettings.ServerMode = value
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
            
            this.AddEnumDropdownBox(
                name: "Progression Choice",
                tooltip: "Whether you would like the server to use the Community Center (bundles) or Joja Mart (pay for upgrades).",
                options: new Dictionary<string, ProgressionChoice>
                {
                    { "Community Center", ProgressionChoice.CommunityCenter },
                    { "Joja Mart", ProgressionChoice.JojaMart }
                },
                getValue: () => this._modConfig.ServerSettings.ProgressionChoice,
                setValue: value => this._modConfig.ServerSettings.ProgressionChoice = value
            );
        }

        private void AddServerSettingsSection()
        {
            this._configMenu?.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Server Settings",
                tooltip: () => "These settings can be changed at any time and impact how the server behaves for all players."
            );
            
            this._configMenu?.AddBoolOption(
                mod: this._modManifest,
                name: () => "Farmhands Can Pause",
                tooltip: () => "Whether farmhands can pause and unpause the game whilst people are playing (the game will still automatically pause when no players are connected).",
                getValue: () => this._modConfig.ServerSettings.FarmhandsCanPause,
                setValue: value => this._modConfig.ServerSettings.FarmhandsCanPause = value
            );
            
            this._configMenu?.AddTextOption(
                mod: this._modManifest,
                name: () => "Host Character Sleep Time",
                tooltip: () => "What time the host character will automatically go to sleep when the game is in server mode. (Not being controlled by a player)",
                getValue: () => this._modConfig.ServerSettings.HostCharacterSleepTime,
                setValue: value => this._modConfig.ServerSettings.HostCharacterSleepTime = value,
                allowedValues: GenericModConfigMenuIntegration.GeneratePossibleHostSleepTimes().ToArray()
            );
            
            this._configMenu?.AddBoolOption(
                mod: this._modManifest,
                name: () => "Lock Player Chests",
                tooltip: () => "If player chests placed in their cabin or home should be locked and only accessible by the owner or not.",
                getValue: () => this._modConfig.ServerSettings.LockPlayerChests,
                setValue: value => this._modConfig.ServerSettings.LockPlayerChests = value
            );
            
            this._configMenu?.AddTextOption(
                mod: this._modManifest,
                name: () => "Pet Name",
                tooltip: () => "The name of the farm pet, maximum 12 characters.",
                getValue: () => this._modConfig.ServerSettings.PetName,
                setValue: (value) => this._modConfig.ServerSettings.PetName = value
            );

            this._configMenu?.AddNumberOption(
                mod: this._modManifest,
                name: () => "Profit Margin",
                tooltip: () => "The sale profit margin difficulty modifier for all players, 50% would mean items sell for half the regular value.",
                getValue: () => this._modConfig.ServerSettings.ProfitMarginPercent,
                setValue: value => this._modConfig.ServerSettings.ProfitMarginPercent = value,
                min: 0,
                max: 100,
                interval: 1,
                formatValue: (value) => $"{value}%"
            );
        }

        private void AddKeybindsSection()
        {
            this._configMenu?.AddSectionTitle(
                mod: this._modManifest,
                text: () => "Keybinds"
            );
            
            this._configMenu?.AddKeybindList(
                mod: this._modManifest,
                name: () => "Toggle Server Mode",
                tooltip: () => "The key bind for toggling the server mode on or off",
                getValue: () => this._modConfig.Controls.ToggleServer,
                setValue: value => this._modConfig.Controls.ToggleServer = value
            );
            
            this._configMenu?.AddKeybindList(
                mod: this._modManifest,
                name: () => "Toggle Pause",
                tooltip: () => "This keybind allows you to pause and unpause as the host character when not in server mode.",
                getValue: () => this._modConfig.Controls.TogglePause,
                setValue: value => this._modConfig.Controls.TogglePause = value
            );
        }
        
        private static List<string> GeneratePossibleHostSleepTimes()
        {
            var times = new List<string>();

            for (var hour = 18; hour < 26; hour++) {
                for (var minute = 0; minute < 60; minute += 10) {
                    var normalizedHour = TimeHelper.NormalizeHour(hour);
                   
                    times.Add($"{normalizedHour:D2}{minute:D2}");
                }
            }

            return times;
        }
    }
}
