using System.Runtime.Serialization;

namespace PersistentMultiplayer.Framework
{
    internal class ModConfig
    {
        /**
         * Accessors
         */
        public bool DedicatedMode { get; set; } = false;

        public ServerSettings ServerSettings { get; set; } = new();
        
        public ModConfigKeys Controls { get; set; } = new();
        
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            this.Controls ??= new ModConfigKeys();
            this.ServerSettings ??= new ServerSettings();
        }
    }
}