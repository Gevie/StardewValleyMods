using StardewValley.Menus;

namespace PersistentMultiplayer.Framework.Chat
{
    internal class ChatMessageHandler
    {
        private static readonly Queue<ChatMessage> Pending = new Queue<ChatMessage>();

        public static void Enqueue(List<ChatMessage> messages)
        {
            foreach (var message in messages) {
                ChatMessageHandler.Pending.Enqueue(message);
            }
        }

        public static void ProcessNext()
        {
            if (ChatMessageHandler.Pending.Count < 1) {
                return;
            }

            var message = ChatMessageHandler.Pending.Dequeue();
            ChatMessenger.Info($"Processing \"{message.message}\"...");
        }

        public static void ProcessAll()
        {
            while (ChatMessageHandler.Pending.Count > 0) {
                ChatMessageHandler.ProcessNext();
            }
        }
    }
}