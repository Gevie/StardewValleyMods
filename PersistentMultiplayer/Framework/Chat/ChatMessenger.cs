using StardewValley;

namespace PersistentMultiplayer.Framework.Chat
{
    internal static class ChatMessenger
    {
        public static void Send(string message)
        {
            Game1.chatBox.activate();
            Game1.chatBox.setText(message);
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }
    }
}