using System.Diagnostics;
using Microsoft.Xna.Framework;
using PersistentMultiplayer.Framework;
using PersistentMultiplayer.Integrations.GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.Pathfinding;

namespace PersistentMultiplayer
{
    internal class ModEntry : Mod
    {
        private ModConfig ModConfig = null!;
        private ModConfigKeys ModConfigKeys => this.ModConfig.Controls;
        private ServerSettings ServerSettings => this.ModConfig.ServerSettings;
        
        // private Vector2 BedLocation = new Vector2(5, 4);
        
        public override void Entry(IModHelper helper)
        {
            this.ModConfig = this.LoadModConfig();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
        }
        
        private ModConfig LoadModConfig()
        {
            return this.Helper.ReadConfig<ModConfig>();
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs gameLaunchedEventArguments)
        {
            this.SetupGenericModConfigMenu();
        }

        private void SetupGenericModConfigMenu()
        {
            var menuIntegration = new GenericModConfigMenuIntegration(this.ModManifest, this.Helper.ModRegistry, this.ModConfig);
            menuIntegration.SetupGenericModConfigMenu(
                () => this.Helper.WriteConfig(this.ModConfig)
            );
        }
        
        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs saveLoadedEventArguments)
        {
            // this.PlaceBedForDedicatedHost();
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs updateTickedEventArguments)
        {
            // Do nothing right now.
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs oneSecondUpdateTickedEventArguments)
        {
            // Do nothing right now.
        }

        // private void PlaceBedForDedicatedHost()
        // {
        //     FarmHouse? farmHouse = Game1.getLocationFromName("FarmHouse") as FarmHouse;
        //     if (farmHouse == null)
        //     {
        //         this.Monitor.Log("Unable to find FarmHouse location.", LogLevel.Error);
        //         return;
        //     }
        //     
        //     if (!farmHouse.isTileLocationOpen(this.BedLocation))
        //     {
        //         this.Monitor.Log("The chosen location for the host bed is not available.", LogLevel.Error);
        //         return;
        //     }
        //
        //     BedFurniture hostBed = new BedFurniture("2052", this.BedLocation, 0);
        //     farmHouse.furniture.Add(hostBed);
        // }
        //
        // private void GoToSleep()
        // {
        //     // Game1.warpFarmer("Farmhouse", (int) this.BedLocation.X, (int) this.BedLocation.Y, false);
        //     // BedFurniture.ShiftPositionForBed(Game1.player);
        //     Game1.player.isInBed.Value = true;
        //     Helper.Reflection.GetMethod(Game1.currentLocation, "startSleep").Invoke();
        // }
    }
}