using System;
using UnityEngine;

namespace DevMinecraftMod.Base
{
    public enum Blocks
    {
        Grass,
        Dirt,
        OakLog,
        SpruceLog,
        BirchLog,
        JungleLog,
        AcaciaLog,
        DarkOakLog,
        OakPlanks,
        SprucePlanks,
        BirchPlanks,
        JunglePlanks,
        AcaciaPlanks,
        DarkOakPlanks,
        Cobblestone,
        Brick,
        Stone,
        Wool,
        OakLeaves,
        SpruceLeaves,
        BirchLeaves,
        JungleLeaves,
        AcaciaLeaves,
        DarkOakLeaves,
        Glass,
        CraftingTable,
        Furnace,
        CoalOre,
        IronOre,
        GoldOre,
        LapisOre,
        RedstoneOre,
        EmeraldOre,
        DiamondOre,
        CoalBlock,
        IronBlock,
        GoldBlock,
        LapisBlock,
        RedstoneBlock,
        EmeraldBlock,
        DiamondBlock,
        Netherrack,
        PackedIce,
        Obsidian,
        Bookshelf,
        StainedGlass,
        RegularIce,
        SoulSand,
        Glowstone,
        SlimeBlock,
        Pumpkin,
        Jackolantern,
        HayBale,
        Melon,
        Bedrock,
        Sponge
    }

    public class MinecraftBlock : MonoBehaviour
    {
        public GameObject minecraftObject; // The physical object that represents the block.
        public Color blockColour = Color.white; // The colour used on the block.
        public Blocks block = Blocks.Grass; // The block type used to identify the block.
        public int blockIndex = 0; // The block index used to identify the block.
    }

    [Serializable]
    public class SoundData
    {
        public int soundRange;
        public int destroyRange;
        public string soundName;
        public string destroySoundName;

        public static SoundData GenerateSoundWithData(int r, string n)
        {
            SoundData theData = new SoundData
            {
                soundRange = r,
                soundName = n,
                destroyRange = r,
                destroySoundName = n
            };

            return theData;
        }

        public static SoundData GenerateSoundWithData(int r, string n, int dR, string dN)
        {
            SoundData theData = new SoundData
            {
                soundRange = r,
                soundName = n,
                destroyRange = dR,
                destroySoundName = dN
            };

            return theData;
        }
    }
}
