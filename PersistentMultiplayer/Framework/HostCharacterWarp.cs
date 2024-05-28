using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacterWarp
    {
        private readonly IModHelper _modHelper;
        private readonly IMonitor _monitor;
        
        public HostCharacterWarp(IModHelper modHelper, IMonitor monitor)
        {
            this._modHelper = modHelper;
            this._monitor = monitor;
        }
        
        public void ToBed()
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
                
                Game1.warpHome();
                return;
            }

            // TODO
            // Use this method to teleport to a hidden off-screen bed when in dedicated server mode, if I decide
            // not to have an out of bounds hidden bed then this approach becomes redundant, and we should use
            // the `Game1.warpHome()` method instead of a custom OnWarp delegate for `Game1.warpFarmer(...)`.
            
            var bedSpot = homeOfFarmer.GetPlayerBedSpot();
            var farmHouseLocationRequest = Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName);
            farmHouseLocationRequest.OnWarp += delegate {
                Game1.player.position.Set(Utility.PointToVector2(bedSpot) * 64f);
            };
            
            Game1.warpFarmer(
                farmHouseLocationRequest, 
                bedSpot.X / 64, 
                bedSpot.Y / 64, 
                Game1.player.FacingDirection
            );
        }
    }
}