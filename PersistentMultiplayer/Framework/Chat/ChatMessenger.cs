using Microsoft.Xna.Framework;
using StardewValley;

namespace PersistentMultiplayer.Framework.Chat
{
    internal static class ChatMessenger
    {
        public static void Error(string message)
        {
            Game1.chatBox.addErrorMessage(message);
        }

        public static void Info(string message)
        {
            Game1.chatBox.addInfoMessage(message);
        }

        public static void Message(string message, Color? colour = null, float alpha = 1f)
        {
            var messageColour = new Color(colour ?? Color.White, alpha);
            Game1.chatBox.addMessage(message, messageColour);
        }
        
        public static void Send(string message)
        {
            Game1.chatBox.activate();
            Game1.chatBox.chatBox.reset();
            Game1.chatBox.setText(message);
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }
    }
}