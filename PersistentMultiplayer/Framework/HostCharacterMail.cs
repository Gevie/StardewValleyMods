using Netcode;
using PersistentMultiplayer.Framework.Chat;
using StardewValley;

namespace PersistentMultiplayer.Framework
{
    internal static class HostCharacterMail
    {
        public static NetStringHashSet Received => Game1.player.mailReceived;
        
        public static void Check()
        {
            var mail = Game1.player.mailReceived;
        }
    }
}