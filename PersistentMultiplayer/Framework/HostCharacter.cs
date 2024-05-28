using PersistentMultiplayer.Framework.Chat;
using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacter
    {
        public static bool IsSleeping { get; set; }
        
        private readonly IModHelper _helper;
        private readonly HostCharacterWarp _hostCharacterWarp;
        private readonly IMonitor _monitor;
        private bool IsGoingToSleep { get; set; }
        public static bool IsInBed => Game1.player.isInBed.Value;
        private const int IsInBedTimeout = 10000;
        private const int IsInBedCheckInterval = 100;

        public HostCharacter(IModHelper helper, IMonitor monitor)
        {
            this._helper = helper;
            this._hostCharacterWarp = new HostCharacterWarp(helper, monitor);
            this._monitor = monitor;
        }

        public async void GoToSleep()
        {
            if (this.IsGoingToSleep || HostCharacter.IsSleeping) {
                return;
            }

            try {
                this.IsGoingToSleep = true;
                await this.Sleep();
            } finally {
                this.IsGoingToSleep = false;
            }
        }

        private async Task Sleep()
        {
            this._hostCharacterWarp.ToBed();
            
            var timeElapsed = 0;
            while (!HostCharacter.IsInBed && timeElapsed < HostCharacter.IsInBedTimeout) {
                await Task.Delay(HostCharacter.IsInBedCheckInterval);
                timeElapsed += HostCharacter.IsInBedCheckInterval;
            }

            if (!HostCharacter.IsInBed) {
                this._monitor.Log("Could not send character to bed.", LogLevel.Error);

                return;
            }
            
            var farmhouse = Utility.getHomeOfFarmer(Game1.player);
            if (farmhouse is null) {
                this._monitor.Log("Cannot find the host's house.",  LogLevel.Error);
                return;
            }
            
            this._helper.Reflection.GetMethod(farmhouse, "startSleep").Invoke();
            
            HostCharacter.IsSleeping = true;
            ChatMessenger.Send("I have gone to bed.");
        }
    }
}