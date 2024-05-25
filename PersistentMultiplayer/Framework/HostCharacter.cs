using System.Reflection;
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

            var farmhouse = Utility.getHomeOfFarmer(Game1.player);
            if(farmhouse == null) {
                this._monitor.Log("Cannot find player's farmhouse.",  LogLevel.Alert);
                return;
            }

            this._monitor.Log($"Game1.player: {Game1.player}", LogLevel.Warn);
            this._monitor.Log($"Game1.player.team: {Game1.player.team}", LogLevel.Warn);
            this._monitor.Log($"Game1.activeClickableMenu: {Game1.activeClickableMenu}", LogLevel.Warn);
            this._monitor.Log($"Game1.player.timeWentToBed.Value {Game1.player.timeWentToBed.Value}", LogLevel.Warn);
            this._monitor.Log($"Game1.player.team.announcedSleepingFarmers.Contains(Game1.player): {Game1.player.team.announcedSleepingFarmers.Contains(Game1.player)}", LogLevel.Warn);
            
            var startSleepMethod = this._helper.Reflection.GetMethod(farmhouse, "startSleep");
            
            bool success;
            try {
                success = startSleepMethod.Invoke<bool>();
            } catch (Exception ex) {
                this._monitor.Log($"Error invoking 'startSleep': {ex}", LogLevel.Alert);
                return;
            }

            this._monitor.Log(success ? "Start sleep was successful" : "Start sleep failed", LogLevel.Alert);
        }

        private void WarpToBed()
        {
            if (InBed) {
                return;
            }

            var homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            if (homeOfFarmer.GetPlayerBed() == null) {
                return;
            }
            
            this._monitor.Log($"Player Bed {homeOfFarmer.GetPlayerBed()}", LogLevel.Debug);
            
            var bed = Utility.PointToVector2(homeOfFarmer.GetPlayerBedSpot()) * 64f;
            this._monitor.Log($"Bed vector: {bed.X} {bed.Y}", LogLevel.Debug);
            Game1.warpFarmer(
                locationRequest: Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName), 
                tileX: (int) bed.X / 64, 
                tileY: (int) bed.Y / 64, 
                facingDirectionAfterWarp: 2
            );
        }
    }
}