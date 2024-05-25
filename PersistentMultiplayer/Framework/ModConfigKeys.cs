﻿using System.Runtime.Serialization;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace PersistentMultiplayer.Framework
{
    internal class ModConfigKeys
    {
        public KeybindList ToggleServerMode { get; set; } = new(SButton.F9);
        public KeybindList TogglePause { get; set; } = new(SButton.F10);

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            this.ToggleServerMode ??= new KeybindList();
            this.TogglePause ??= new KeybindList();
        }
    }
}