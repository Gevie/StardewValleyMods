using PersistentMultiplayer.Framework.Configuration;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace PersistentMultiplayer.Framework
{
    internal class SleepScheduler
    {
        private IModHelper _helper;
        private ServerSettings _serverSettings;

        public SleepScheduler(IModHelper helper, ServerSettings serverSettings)
        {
            this._helper = helper;
            this._serverSettings = serverSettings;
        }

        public bool IsBedTime()
        {
            var currentTime = new TimeSpan(
                hours: Game1.timeOfDay / 100, 
                minutes: Game1.timeOfDay % 100, 
                seconds: 0
            );
            
            var bedTime = this.GetBedTime();

            if (bedTime.Hours < 2) {
                return (currentTime.Hours < 2 && currentTime >= bedTime);
            }

            return (currentTime >= bedTime);
        }

        private TimeSpan GetBedTime()
        {
            var sleepTime = this._serverSettings.HostCharacterSleepTime;

            return new TimeSpan(
                hours: int.Parse(sleepTime.Substring(0, 2)), 
                minutes: int.Parse(sleepTime.Substring(2, 2)), 
                seconds: 0
            );
        }
    }
}