using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacterWarp
    {
        private readonly IModHelper _modHelper;
        
        public HostCharacterWarp(IModHelper modHelper)
        {
            this._modHelper = modHelper;
        }
        
        public void To(LocationRequest locationRequest, int tileX, int tileY, int facingDirectionAfterWarp)
        {
            if (Game1.player.isRidingHorse()) {
                Game1.player.mount.dismount();
            }

            if (Game1.player.IsSitting()) {
                Game1.player.StopSitting(animate: false);
            }

            if (Game1.player.UsingTool) {
                Game1.player.completelyStopAnimatingOrDoingAction();
            }
            
            Game1.player.previousLocationName = (Game1.player.currentLocation != null) ? Game1.player.currentLocation.Name : "";
            Game1.locationRequest = locationRequest;
            Game1.xLocationAfterWarp = tileX;
            Game1.yLocationAfterWarp = tileY;
            this._modHelper.Reflection.GetField<bool>(typeof(Game1), "_isWarping").SetValue(true);
            Game1.facingDirectionAfterWarp = facingDirectionAfterWarp;
            
            Game1.fadeScreenToBlack();
            Game1.setRichPresence("location", locationRequest.Name);
        }
    }
}