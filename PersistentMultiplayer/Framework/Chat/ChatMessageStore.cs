using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace PersistentMultiplayer.Framework.Chat
{
    internal class ChatMessageStore
    {
        public static ChatMessageStore Instance {
            get {
                if (_instance is null) {
                    throw new InvalidOperationException("ChatMessageStore has not been initialized.");
                }

                return _instance;
            }
            private set => _instance = value;
        }
        
        public static List<ChatMessage> List { get; private set; } = new List<ChatMessage>();
        
        private readonly IModHelper _helper;
        private static ChatMessageStore? _instance;
        
        public ChatMessageStore(IModHelper helper)
        {
            this._helper = helper;
            Instance = this;
        }

        public void Refresh()
        {
            var messages = this.GetMessages();
            if (messages.Count < 1) {
                return;
            }

            if (messages.SequenceEqual(ChatMessageStore.List)) {
                return;
            }

            var currentMessages = ChatMessageStore.List.Select(message => message.message);
            var newMessages = messages.Where(message => !currentMessages.Contains(message.message)).ToList();

            ChatMessageStore.List = messages;
            ChatMessageHandler.Enqueue(newMessages);
        }

        private List<ChatMessage> GetMessages()
        {
            return this._helper
                .Reflection
                .GetField<List<ChatMessage>>(Game1.chatBox, "messages")
                .GetValue();
        }
    }
}