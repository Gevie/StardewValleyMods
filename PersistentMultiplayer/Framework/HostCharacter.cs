using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacter
    {
        public static bool InBed => Game1.player.isInBed.Value;

        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;

        public HostCharacter(IModHelper helper, IMonitor monitor)
        {
            this._helper = helper;
            this._monitor = monitor;
        }

        public void GoToSleep()
        {
            if (!InBed) {
                this.WarpToBed();
            }
            
            this._helper.Reflection.GetMethod(Game1.currentLoader, "startSleep").Invoke();
        }

        private void WarpToBed()
        {
            this._monitor.Log($"InBed: {InBed}", LogLevel.Debug);
            if (InBed) {
                return;
            }

            var homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            this._monitor.Log($"Home of Farmer: {homeOfFarmer}", LogLevel.Debug);
            
            if (homeOfFarmer.GetPlayerBed() == null) {
                return;
            }
            
            this._monitor.Log($"Player Bed {homeOfFarmer.GetPlayerBed()}", LogLevel.Debug);
            
            var bed = Utility.PointToVector2(homeOfFarmer.GetPlayerBedSpot()) * 64f;
            this._monitor.Log($"Bed vector: {bed.X} {bed.Y}");
            Game1.warpFarmer(
                locationRequest: Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName), 
                tileX: (int) bed.X / 64, 
                tileY: (int) bed.Y / 64, 
                facingDirectionAfterWarp: 2
            );
        }
    }
}