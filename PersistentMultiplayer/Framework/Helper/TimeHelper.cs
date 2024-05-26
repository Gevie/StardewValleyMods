namespace PersistentMultiplayer.Framework.Helper
{
    internal static class TimeHelper
    {
        public static int NormalizeHour(int hour)
        {
            return hour switch
            {
                24 => 0,
                25 => 1,
                _ => hour
            };
        }
    }
}