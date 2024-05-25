using PersistentMultiplayer.Framework;
using PersistentMultiplayer.Framework.Configuration;
using PersistentMultiplayer.Integrations.GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace PersistentMultiplayer
{
    internal class ModEntry : Mod
    {
        private HostCharacter HostCharacter;
        private ModConfig _modConfig = null!;
        private ModConfigKeys ModConfigKeys => this._modConfig.Controls;
        private bool ServerMode { get; set; }
        private ServerSettings ServerSettings => this._modConfig.ServerSettings;
        private SleepScheduler SleepScheduler;
        
        public override void Entry(IModHelper helper)
        {
            this._modConfig = this.LoadModConfig();
            this.HostCharacter = new HostCharacter(helper, this.Monitor);
            this.SleepScheduler = new SleepScheduler(helper, this._modConfig.ServerSettings);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
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
            // Debugging: We don't have to wait 12 in game hours to test sleeping code.
            Game1.timeOfDay = 1810;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs updateTickedEventArguments)
        {
            // Do nothing right now.
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs oneSecondUpdateTickedEventArguments)
        {
            this.Monitor.Log($"ServerMode : {this.ServerMode}", LogLevel.Alert);
            if (!this.ServerMode) {
                return;
            }
            
            this.Monitor.Log($"Checking for bed time...", LogLevel.Alert);
            if (this.SleepScheduler.IsBedTime()) {
                this.Monitor.Log($"It is bed time, attempting to sleep.", LogLevel.Alert);
                this.HostCharacter.GoToSleep();
            }
            else
            {
                this.Monitor.Log($"It isn't bed time...", LogLevel.Alert);
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
        }

        private static void TogglePause()
        {
            Game1.netWorldState.Value.IsPaused = !Game1.netWorldState.Value.IsPaused;
        }
    }
}