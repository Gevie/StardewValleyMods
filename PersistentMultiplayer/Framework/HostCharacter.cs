using System.Reflection;
using Microsoft.Xna.Framework;
using Netcode;
using PersistentMultiplayer.Framework.Chat;
using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacter
    {
        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;
        private static bool IsInBed => Game1.player.isInBed.Value;
        private const int IsInBedTimeout = 6000;
        private const int IsInBedCheckInterval = 100;
        private bool IsGoingToSleep { get; set; }

        public HostCharacter(IModHelper helper, IMonitor monitor)
        {
            this._helper = helper;
            this._monitor = monitor;
        }

        public async void GoToSleep()
        {
            if (this.IsGoingToSleep) {
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
            ChatMessenger.Send("I have gone to bed.");
        }

        private void WarpToBed()
        {
            if (HostCharacter.IsInBed) {
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
            this._monitor.Log("Warping character to bed, in bed should switch to true...", LogLevel.Trace);
            Game1.warpFarmer(
                locationRequest: Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName), 
                tileX: (int) (bed.X) / 64, 
                tileY: (int) bed.Y / 64, 
                facingDirectionAfterWarp: 2
            );
        }
    }
}