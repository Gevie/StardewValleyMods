using PersistentMultiplayer.Framework;
using PersistentMultiplayer.Framework.Chat;
using PersistentMultiplayer.Framework.Configuration;
using PersistentMultiplayer.Integrations.GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace PersistentMultiplayer
{
    internal class ModEntry : Mod
    {
        private HostCharacter _hostCharacter;
        private ModConfig _modConfig = null!;
        private ModConfigKeys ModConfigKeys => this._modConfig.Controls;
        private bool ServerMode { get; set; }
        private ServerSettings ServerSettings => this._modConfig.ServerSettings;
        private SleepScheduler _sleepScheduler;
        
        public override void Entry(IModHelper helper)
        {
            this._modConfig = this.LoadModConfig();
            
            this._hostCharacter = new HostCharacter(helper, this.Monitor);
            this._sleepScheduler = new SleepScheduler(helper, this._modConfig.ServerSettings);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }
        
        private ModConfig LoadModConfig()
        {
            return this.Helper.ReadConfig<ModConfig>();
        }

        private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs buttonsChangedEventArguments)
        {
            if (!Context.IsWorldReady) {
                return;
            }

            if (this.ModConfigKeys.ToggleServer.JustPressed()) {
                this.ToggleServer();
                return;
            }

            if (this.ModConfigKeys.TogglePause.JustPressed()) {
                TogglePause();
            }
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs gameLaunchedEventArguments)
        {
            this.SetupGenericModConfigMenu();
        }
        
        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs saveLoadedEventArguments)
        {
            // TODO: To be deleted, fast forward time on game load so go to bed logic can be tested without waiting.
            Game1.timeOfDay = 1820;
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs oneSecondUpdateTickedEventArguments)
        {
            if (!this.ServerMode) {
                return;
            }
            
            if (this._sleepScheduler.IsBedTime()) {
                this._hostCharacter.GoToSleep();
            }
        }

        private void SetupGenericModConfigMenu()
        {
            var menuIntegration = new GenericModConfigMenuIntegration(
                this.ModManifest, 
                this.Helper.ModRegistry, 
                this._modConfig
            );
            
            menuIntegration.SetupGenericModConfigMenu(
                save: () => this.Helper.WriteConfig(this._modConfig)
            );
        }

        private void ToggleServer()
        {
            this.ServerMode = !this.ServerMode;
            var serverModeStatus = this.ServerMode ? "On" : "Off";
            
            ChatMessenger.Send($"Server Mode {serverModeStatus}");
        }

        private static void TogglePause()
        {
            Game1.netWorldState.Value.IsPaused = !Game1.netWorldState.Value.IsPaused;
            var pausedMode = Game1.netWorldState.Value.IsPaused ? "Paused" : "Resumed";
            
            ChatMessenger.Send($"Game {pausedMode}");
        }
    }
}