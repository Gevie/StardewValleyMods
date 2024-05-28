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
        private ChatMessageStore _chatStore = null!;
        private HostCharacter _hostCharacter = null!;
        private ModConfig _modConfig = null!;
        private ModConfigKeys ModConfigKeys => this._modConfig.Controls;
        private bool ServerMode { get; set; }
        private ServerSettings ServerSettings => this._modConfig.ServerSettings;
        private SleepScheduler _sleepScheduler = null!;
        
        public override void Entry(IModHelper helper)
        {
            this._chatStore = new ChatMessageStore(helper);
            this._modConfig = this.Helper.ReadConfig<ModConfig>();
            this._hostCharacter = new HostCharacter(helper, this.Monitor);
            this._sleepScheduler = new SleepScheduler(this.ServerSettings);

            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }

        private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs buttonsChangedEventArguments)
        {
            if (!Context.IsWorldReady) {
                return;
            }

            if (this.ModConfigKeys.ToggleServer.JustPressed()) {
                this.ToggleServerMode();
                return;
            }

            if (this.ModConfigKeys.TogglePause.JustPressed()) {
                TogglePause(Game1.player);
            }
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs dayStartedEventArguments)
        {
            HostCharacter.IsSleeping = false;
            // HostCharacterMail.Check();
            //Game1.timeOfDay = 1830;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs gameLaunchedEventArguments)
        {
            this.SetupGenericModConfigMenu();
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs oneSecondUpdateTickedEventArguments)
        {
            if (Context.IsWorldReady) {
                this._chatStore.Refresh();
            }
            
            if (!this.ServerMode) {
                return;
            }
            
            ChatMessageHandler.ProcessNext();
            
            if (this._sleepScheduler.IsBedTime() && !HostCharacter.IsSleeping) {
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

        private static void TogglePause(Farmer initiatedBy)
        {
            Game1.netWorldState.Value.IsPaused = !Game1.netWorldState.Value.IsPaused;
            
            var mode = Game1.netWorldState.Value.IsPaused ? "paused" : "resumed";
            ChatMessenger.Info($"{initiatedBy.Name} has {mode} the game.");
        }

        private void ToggleServerMode()
        {
            this.ServerMode = !this.ServerMode;

            if (!this.ServerMode) {
                ChatMessenger.Info($"{Game1.player.Name} has returned, server mode disabled.");
                return;
            }
            
            ChatMessenger.Info($"{Game1.player.Name} has gone away, server mode enabled.");
        }
    }
}