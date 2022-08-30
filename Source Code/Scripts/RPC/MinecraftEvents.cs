using System;
using DevMinecraftMod.Base;
using UnityEngine;

namespace DevMinecraftMod.RPC
{
    public class MinecraftEvents
    {
        public static event EventHandler<BlockArgs> OnBlockPlaced;
        public static event EventHandler<BlockArgs> OnBlockDestroyed;

        public virtual void TriggerBlockPlace(BlockArgs ba) => OnBlockPlaced?.SafeInvoke(this, ba);
        public virtual void TriggerBlockDestroy(BlockArgs ba) => OnBlockDestroyed?.SafeInvoke(this, ba);

        public class BlockArgs : EventArgs
        {
            public Blocks block { get; set; }
            public Vector3 blockPosition { get; set; }
        }
    }
}
