using System;
using System.Collections.Generic;
using System.Text;

namespace DevMinecraftMod.Base
{
    [System.Serializable]
    public class SaveData
    {
        public bool square = true;
        public bool line = true;
        public float musicVolume = 0.05f;
        public float blockVolume = 0.25f;
        public int totalBlocksPlaced;
        public int totalBlocksRemoved;
    }
}
