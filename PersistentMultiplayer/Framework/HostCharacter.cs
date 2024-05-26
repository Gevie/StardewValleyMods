using PersistentMultiplayer.Framework.Chat;
using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacter
    {
        public static bool IsSleeping { get; set; }
        
        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;
        private bool IsGoingToSleep { get; set; }
        private static bool IsInBed => Game1.player.isInBed.Value;
        private const int IsInBedTimeout = 10000;
        private const int IsInBedCheckInterval = 100;

        public HostCharacter(IModHelper helper, IMonitor monitor)
        {
            this._helper = helper;
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
            // Introduction of async task for proper handling now means the player is being warped to the entrance
            // of their farmhouse if they are not already in the farmhouse. The while loop will then timeout
            // because HostCharacter.IsInBed is false, and providing the player has not left their home - the player
            // will then successfully teleport to the bed and sleep because ModEntry triggered a new GoToSleep() call.
            // The previous implementation just fired the events off wildly each applicable tick so this wasn't noticed.
            // TODO: Check if the player is in their home or not and handle both warps with await / async implementation
            // TODO: OR more preferably, implement the warp so that it always goes directly to the bed tile regardless.
            
            this.WarpToBed();
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

        private void WarpToBed()
        {
            if (HostCharacter.IsInBed) {
                this._monitor.Log("Host character is already in bed.", LogLevel.Alert);
                return;
            }

            var homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            if (homeOfFarmer.GetPlayerBed() is null) {
                this._monitor.Log(
                    message: "Cannot find the host's bed for auto sleep, please make sure it is placed in their home.",  
                    level: LogLevel.Warn
                );
                
                return;
            }
            
            var bed = Utility.PointToVector2(homeOfFarmer.GetPlayerBedSpot()) * 64f;
            Game1.warpFarmer(
                locationRequest: Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName), 
                tileX: (int) bed.X / 64, 
                tileY: (int) bed.Y / 64, 
                facingDirectionAfterWarp: 2
            );
        }
    }
}