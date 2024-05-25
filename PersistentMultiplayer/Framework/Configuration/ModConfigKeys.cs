using System.Runtime.Serialization;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace PersistentMultiplayer.Framework.Configuration
{
    internal class ModConfigKeys
    {
        public KeybindList ToggleServer { get; set; } = new(SButton.F9);
        public KeybindList TogglePause { get; set; } = new(SButton.F10);

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            this.ToggleServer ??= new KeybindList();
            this.TogglePause ??= new KeybindList();
        }
    }
}