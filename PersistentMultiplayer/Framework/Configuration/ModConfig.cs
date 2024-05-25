using System.Runtime.Serialization;

namespace PersistentMultiplayer.Framework.Configuration
{
    internal class ModConfig
    {
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