using PersistentMultiplayer.Framework.Constant;

namespace PersistentMultiplayer.Framework.Configuration
{
    internal class ServerSettings
    {
        public CaveType CaveType { get; set; } = CaveType.Mushroom;
        
        public bool FarmhandsCanPause { get; set; }
        
        public string HostCharacterSleepTime {
            get => this._hostCharacterSleepTime;
            set => this.SetHostCharacterSleepTime(value);
        }
        
        public HouseUpgradeLevel HouseUpgradeLevel { get; set; } = HouseUpgradeLevel.None;
        
        public bool LockPlayerChests { get; set; }

        public ServerType ServerType { get; set; } = ServerType.Local;
        
        public string PetName {
            get => this._petName;
            set => this.SetPetName(value);
        }
        
        public int ProfitMarginPercent {
            get => _profitMarginPercent;
            set => this.SetProfitMarginInPercent(value);
        }
        
        public ProgressionChoice ProgressionChoice { get; set; } = ProgressionChoice.CommunityCenter;

        private string _hostCharacterSleepTime = "1830";
        
        private string _petName = "Toby";
        
        private int _profitMarginPercent = 100;

        private void SetHostCharacterSleepTime(string twentyFourHourTime)
        {
            if (twentyFourHourTime.Length != 4 || !int.TryParse(twentyFourHourTime, out _)) {
                throw new ArgumentException(
                    $"Host character sleep time must be a 4 digit time string, i.e. \"0130\" for 01:30, \"{twentyFourHourTime}\" given."
                );
            }
            
            int hour = int.Parse(twentyFourHourTime.Substring(0, 2));
            int minute = int.Parse(twentyFourHourTime.Substring(2, 2));
            
            if (minute % 10 != 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(twentyFourHourTime),
                    $"Provided time is not valid, please use only 10 minute intervals, \"{minute}\" given."
                );
            }

            if (hour is > 01 and < 18) {
                throw new ArgumentOutOfRangeException(
                    nameof(twentyFourHourTime),
                    $"Provided time is not valid, you may only sleep between the hours 18 and 01, \"{hour}\" provided."
                );
            }

            this._hostCharacterSleepTime = twentyFourHourTime;
        }
        
        private void SetPetName(string petName)
        {
            if (petName.Length is < 1 or > 12) {
                throw new ArgumentException(
                    $"Pet name must be between 1 and 12 characters long, \"{petName}\" is {petName.Length}."
                );
            }

            this._petName = petName;
        }
        
        private void SetProfitMarginInPercent(int profitMarginPercent)
        {
            if (profitMarginPercent is < 0 or > 100) {
                throw new ArgumentOutOfRangeException(
                    nameof(profitMarginPercent), 
                    $"Profit margin percent must be an integer between 0 and 100, {profitMarginPercent} given."
                );
            }

            this._profitMarginPercent = profitMarginPercent;
        }
    }
}