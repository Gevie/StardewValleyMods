using PersistentMultiplayer.Framework.Chat;
using StardewModdingAPI;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal class HostCharacter
    {
        public static bool IsSleeping { get; set; }
        
        private readonly IModHelper _helper;
        private HostCharacterWarp _hostCharacterWarp;
        private readonly IMonitor _monitor;
        private bool IsGoingToSleep { get; set; }
        private static bool IsInBed => Game1.player.isInBed.Value;
        private const int IsInBedTimeout = 10000;
        private const int IsInBedCheckInterval = 100;

        public HostCharacter(IModHelper helper, IMonitor monitor)
        {
            this._helper = helper;
            this._hostCharacterWarp = new HostCharacterWarp(helper);
            this._monitor = monitor;
        }

        public async void GoToSleep()
        {
            if (this.IsGoingToSleep || HostCharacter.IsSleeping) {
                return;
            }

            try {
                this.IsGoingToSleep = true;
                this._monitor.Log($"Attempt to sleep...", LogLevel.Alert);
                await this.Sleep();
            } finally {
                this.IsGoingToSleep = false;
            }
        }

        private async Task Sleep()
        {
            var homeName = Utility.getHomeOfFarmer(Game1.player);
            var currentLocation = Game1.player.currentLocation;
            if (Game1.player.currentLocation.NameOrUniqueName != Utility.getHomeOfFarmer(Game1.player).NameOrUniqueName) {
                this.WarpToBed();
            }
            
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
            
            var bedSpot = homeOfFarmer.GetPlayerBed().GetBedSpot();
            var getBedSpot = homeOfFarmer.GetPlayerBedSpot();
            var entryLocation = homeOfFarmer.getEntryLocation();
            this._monitor.Log($"Bod Spot: {bedSpot.X}:{bedSpot.Y}", LogLevel.Alert);
            this._monitor.Log($"Get Bed Spot: {getBedSpot.X}:{getBedSpot.Y}", LogLevel.Alert);
            this._monitor.Log($"Entry Location: {entryLocation.X}:{entryLocation.Y}", LogLevel.Alert);
            
            var bed = Utility.PointToVector2(homeOfFarmer.GetPlayerBedSpot()) * 64f;
            
            this._monitor.Log($"Location Request: {Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName)}", LogLevel.Alert);
            this._monitor.Log($"tileX: {(int) bed.X / 64}", LogLevel.Alert);
            this._monitor.Log($"tileY: {(int) bed.Y / 64}", LogLevel.Alert);
            
            Game1.warpHome();
            
            // LocationRequest obj = getLocationRequest(player.homeLocation.Value);
            // obj.OnWarp += delegate
            // {
            //     player.position.Set(Utility.PointToVector2((currentLocation as FarmHouse).GetPlayerBedSpot()) * 64f);
            // };
            // warpFarmer(obj, 5, 9, player.FacingDirection);
            
            // this._hostCharacterWarp.To(
            //     locationRequest: Game1.getLocationRequest(homeOfFarmer.NameOrUniqueName), 
            //     tileX: (int) bed.X / 64, 
            //     tileY: (int) bed.Y / 64, 
            //     facingDirectionAfterWarp: 2
            // );
        }
    }
}