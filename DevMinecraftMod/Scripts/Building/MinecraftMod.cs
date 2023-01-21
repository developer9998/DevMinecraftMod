using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using HarmonyLib;
using Photon.Pun;
using GorillaLocomotion;
using System.Linq;
using System;
using Random = UnityEngine.Random;

// programmed by dev.
// material data from me, graic, and frogrilla on the official gorilla tag modding discord

// 10/17/2022
// a ton of this code is pretty old, although because its worth over 2.9k lines i'm not in the mood to reprogram large chunks of it rn

// 11/3/2022
// the mod is literally my second most viewed mod on github

// 1/20/2023
// wowie okay it's been a bit... but hey people are still using the mod!
// i had no idea what a dictionary was when revamping the mod

namespace DevMinecraftMod.Base
{
    public class MinecraftMod : MonoBehaviour
    {
        public static MinecraftMod Instance;

        public List<MinecraftBlock> blockLinks = new List<MinecraftBlock>();

        public GameObject objectStorage;
        private GameObject objectStorageBlock;
        private GameObject block;
        private GameObject blockAlt;
        public GameObject itemIndicator;
        private GameObject itemPickaxe;
        private GameObject itemShow;

        private Text itemText;
        public Text noticeText;
        private LineRenderer ln;
        private Harmony harmony;
        public AssetBundle blockBundle;
        public AssetBundle blockBundleAlt;

        public AudioClip clip;

        public List<Color> colours = new List<Color>() { Color.white, Color.white, Color.white };

        private bool triggerPullled = true;
        private bool gripPulled = true;
        private bool middlePulled = false;
        private bool raycastExists = false;
        private float cooldownTime = 0;

        private int mode = 0;
        private readonly int modeVersion = 0;
        public int currentBlock = 0;
        public int currentColourMode = 0;

        public List<GameObject> minecraftBlocks = new List<GameObject>();
        private readonly List<GameObject> minecraftBlockList = new List<GameObject>();
        private readonly List<string> minecraftBlockListString = new List<string>();

        public Dictionary<Blocks, SoundData> blkToSD = new Dictionary<Blocks, SoundData>()
        {
            {Blocks.Grass, SoundData.GenerateSoundWithData(3,"Grass") }, // Soil
            {Blocks.Dirt, SoundData.GenerateSoundWithData(3,"Dirt") },
            {Blocks.OakLog, SoundData.GenerateSoundWithData(3,"Log") }, // Logs
            {Blocks.AcaciaLog, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.BirchLog, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.DarkOakLog, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.JungleLog, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.SpruceLog, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.AcaciaPlanks, SoundData.GenerateSoundWithData(3,"Plank") }, // Planks
            {Blocks.BirchPlanks, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.DarkOakPlanks, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.JunglePlanks, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.OakPlanks, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.SprucePlanks, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.Cobblestone, SoundData.GenerateSoundWithData(3,"Stone") }, // Stone
            {Blocks.Stone, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.Brick, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.Wool, SoundData.GenerateSoundWithData(3,"Fabric") },
            {Blocks.AcaciaLeaves, SoundData.GenerateSoundWithData(3,"Grass") }, // Leaves
            {Blocks.BirchLeaves, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.DarkOakLeaves, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.JungleLeaves, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.OakLeaves, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.SpruceLeaves, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.Glass, SoundData.GenerateSoundWithData(3,"GlassPlace", 4, "GlassBreak") }, // Glass
            {Blocks.CraftingTable, SoundData.GenerateSoundWithData(3,"Plank") }, // Details
            {Blocks.Furnace, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.CoalOre, SoundData.GenerateSoundWithData(3,"Stone") }, // Ores
            {Blocks.DiamondOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.EmeraldOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.GoldOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.IronOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.LapisOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.RedstoneOre, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.Netherrack, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.PackedIce, SoundData.GenerateSoundWithData(3,"IcePlace", 4, "GlassBreak") },
            {Blocks.Obsidian, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.Bookshelf, SoundData.GenerateSoundWithData(3,"Plank") },
            {Blocks.CoalBlock, SoundData.GenerateSoundWithData(3,"Stone") }, // Blocks
            {Blocks.LapisBlock, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.IronBlock, SoundData.GenerateSoundWithData(4,"Metal") },
            {Blocks.GoldBlock, SoundData.GenerateSoundWithData(4,"Metal") },
            {Blocks.RedstoneBlock, SoundData.GenerateSoundWithData(4,"Metal") },
            {Blocks.EmeraldBlock, SoundData.GenerateSoundWithData(4,"Metal") },
            {Blocks.DiamondBlock, SoundData.GenerateSoundWithData(4,"Metal") },
            {Blocks.StainedGlass, SoundData.GenerateSoundWithData(3,"GlassPlace", 4, "GlassBreak") },
            {Blocks.Glowstone, SoundData.GenerateSoundWithData(3,"IcePlace", 4, "GlassBreak") },
            {Blocks.SoulSand, SoundData.GenerateSoundWithData(3,"Sand") },
            {Blocks.RegularIce, SoundData.GenerateSoundWithData(3,"IcePlace", 4, "GlassBreak") },
            {Blocks.SlimeBlock, SoundData.GenerateSoundWithData(3,"SlimePlace") },
            {Blocks.Pumpkin, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.Jackolantern, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.Melon, SoundData.GenerateSoundWithData(3,"Log") },
            {Blocks.Bedrock, SoundData.GenerateSoundWithData(3,"Stone") },
            {Blocks.HayBale, SoundData.GenerateSoundWithData(3,"Grass") },
            {Blocks.Sponge, SoundData.GenerateSoundWithData(3,"Grass") },
        };

        public Dictionary<int, BlockPhysData> blkToBD = new Dictionary<int, BlockPhysData>()
        {
            {0, BlockPhysData.GenerateBlockDataWithData(7, 14, 14, 14, 14, 14, false, false, Blocks.Grass)}, // Soil
            {1, BlockPhysData.GenerateBlockDataWithData(14, 14, 14, 14, 14, 14, false, false, Blocks.Dirt)},
            {2, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.OakLog)}, // Logs
            {3, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.SpruceLog)},
            {4, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.BirchLog)},
            {5, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.JungleLog)},
            {6, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.AcaciaLog)},
            {7, BlockPhysData.GenerateBlockDataWithData(9, 9, 8, 8, 8, 8, false, false, Blocks.DarkOakLog)},
            {8, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.OakPlanks)}, // Planks
            {9, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.SprucePlanks)},
            {10, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.BirchPlanks)},
            {11, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.JunglePlanks)},
            {12, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.AcaciaPlanks)},
            {13, BlockPhysData.GenerateBlockDataWithData(9, 9, 9, 9, 9, 9, false, false, Blocks.DarkOakPlanks)},
            {14, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.OakLeaves)}, // Leaves
            {15, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.SpruceLeaves)},
            {16, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.BirchLeaves)},
            {17, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.JungleLeaves)},
            {18, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.AcaciaLeaves)}, // Leaves
            {19, BlockPhysData.GenerateBlockDataWithData(99, 99, 99, 99, 99, 99, false, false, Blocks.DarkOakLeaves)},
            {20, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Cobblestone)}, // Stone
            {21, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Stone)},
            {22, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.CoalOre)}, // Ores
            {23, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.IronOre)},
            {24, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.GoldOre)},
            {25, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.RedstoneOre)},
            {26, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.LapisOre)},
            {27, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.EmeraldOre)},
            {28, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.DiamondOre)},
            {29, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.CoalBlock)}, // Block
            {30, BlockPhysData.GenerateBlockDataWithData(146, 146, 146, 146, 146, 146, false, false, Blocks.IronBlock)},
            {31, BlockPhysData.GenerateBlockDataWithData(146, 146, 146, 146, 146, 146, false, false, Blocks.GoldBlock)},
            {32, BlockPhysData.GenerateBlockDataWithData(146, 146, 146, 146, 146, 146, false, false, Blocks.RedstoneBlock)},
            {33, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.LapisBlock)},
            {34, BlockPhysData.GenerateBlockDataWithData(146, 146, 146, 146, 146, 146, false, false, Blocks.EmeraldBlock)},
            {35, BlockPhysData.GenerateBlockDataWithData(146, 146, 146, 146, 146, 146, false, false, Blocks.DiamondBlock)},
            {36, BlockPhysData.GenerateBlockDataWithData(106, 106, 106, 106, 106, 106, false, false, Blocks.Glass)},
            {37, BlockPhysData.GenerateBlockDataWithData(106, 106, 106, 106, 106, 106, false, true, Blocks.StainedGlass)},
            {38, BlockPhysData.GenerateBlockDataWithData(107, 107, 107, 107, 107, 107, false, true, Blocks.Wool)}, // Blocks
            {39, BlockPhysData.GenerateBlockDataWithData(145, 145, 145, 145, 145, 145, false, false, Blocks.Bookshelf)},
            {40, BlockPhysData.GenerateBlockDataWithData(145, 145, 145, 145, 145, 145, false, false, Blocks.CraftingTable)},
            {41, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Furnace)},
            {42, BlockPhysData.GenerateBlockDataWithData(59, 59, 59, 59, 59, 59, false, false, Blocks.RegularIce)},
            {43, BlockPhysData.GenerateBlockDataWithData(59, 59, 59, 59, 59, 59, false, false, Blocks.PackedIce)},
            {44, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Netherrack)},
            {45, BlockPhysData.GenerateBlockDataWithData(88, 88, 88, 88, 88, 88, false, false, Blocks.SoulSand)},
            {46, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Obsidian)},
            {47, BlockPhysData.GenerateBlockDataWithData(120, 120, 120, 120, 120, 120, false, false, Blocks.Glowstone)},
            {48, BlockPhysData.GenerateBlockDataWithData(96, 96, 96, 96, 96, 96, true, false, Blocks.SlimeBlock)},
            {49, BlockPhysData.GenerateBlockDataWithData(81, 81, 81, 81, 81, 81, false, false, Blocks.Pumpkin)},
            {50, BlockPhysData.GenerateBlockDataWithData(81, 81, 81, 81, 81, 81, false, false, Blocks.Jackolantern)},
            {51, BlockPhysData.GenerateBlockDataWithData(96, 96, 96, 96, 96, 96, false, false, Blocks.Sponge)},
            {52, BlockPhysData.GenerateBlockDataWithData(145, 145, 145, 145, 145, 145, false, false, Blocks.Melon)},
            {53, BlockPhysData.GenerateBlockDataWithData(88, 88, 88, 88, 88, 88, false, false, Blocks.HayBale)},
            {54, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Bedrock)},
            {55, BlockPhysData.GenerateBlockDataWithData(0, 0, 0, 0, 0, 0, false, false, Blocks.Brick)},
        };

        #region Place/Destroy Functions

        void PlaySpawnBlockAudio(Blocks block, Vector3 blockPosition)
        {
            GameObject soundObjectTemp = Instantiate(blockBundle.LoadAsset<GameObject>("SoundExample"));
            soundObjectTemp.transform.position = blockPosition;
            AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

            AudioClip clip = blockBundle.LoadAsset<AudioClip>($"{blkToSD[block].soundName}{Random.Range(1, blkToSD[block].soundRange)}") ?? blockBundleAlt.LoadAsset<AudioClip>($"{blkToSD[block].soundName}{Random.Range(1, blkToSD[block].soundRange)}");
            audioSourceTemp.PlayOneShot(clip, Plugin.Instance.blockVolume);
            audioSourceTemp.transform.SetParent(objectStorage.transform, false);
            audioSourceTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 3;
        }

        void DestroyBlock(Blocks block, Vector3 blockPosition, Color blockColour)
        {
            GameObject soundObjectTemp = Instantiate(blockBundle.LoadAsset<GameObject>("SoundExample"));
            soundObjectTemp.transform.position = blockPosition;
            AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

            AudioClip clip = blockBundle.LoadAsset<AudioClip>($"{blkToSD[block].destroySoundName}{Random.Range(1, blkToSD[block].destroyRange)}") ?? blockBundleAlt.LoadAsset<AudioClip>($"{blkToSD[block].destroySoundName}{Random.Range(1, blkToSD[block].destroyRange)}");
            audioSourceTemp.PlayOneShot(clip, Plugin.Instance.blockVolume);
            audioSourceTemp.transform.SetParent(objectStorage.transform, false);
            audioSourceTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 3;

            GameObject particleObjectTemp = Instantiate(blockBundle.LoadAsset<GameObject>("BlockParticle"));
            particleObjectTemp.transform.position = blockPosition - new Vector3(0, 0.15f, 0);

            foreach (ParticleSystem partS in particleObjectTemp.transform.GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystemRenderer psr = partS.GetComponent<ParticleSystemRenderer>();

                SetParticleMaterial(psr, blockBundle, "dirt", Blocks.Grass, block);
                SetParticleMaterial(psr, blockBundle, "dirt", Blocks.Dirt, block);
                SetParticleMaterial(psr, blockBundle, "log", Blocks.OakLog, block);

                SetParticleMaterial(psr, blockBundleAlt, "SpruceLog", Blocks.SpruceLog, block);
                SetParticleMaterial(psr, blockBundleAlt, "BirchLog", Blocks.BirchLog, block);
                SetParticleMaterial(psr, blockBundleAlt, "JungleLog", Blocks.JungleLog, block);
                SetParticleMaterial(psr, blockBundleAlt, "AcaciaLog", Blocks.AcaciaLog, block);
                SetParticleMaterial(psr, blockBundleAlt, "BigOakLog", Blocks.DarkOakLog, block);

                SetParticleMaterial(psr, blockBundle, "plank", Blocks.OakPlanks, block);

                SetParticleMaterial(psr, blockBundleAlt, "SprucePlanks", Blocks.SprucePlanks, block);
                SetParticleMaterial(psr, blockBundleAlt, "BirchPlanks", Blocks.BirchPlanks, block);
                SetParticleMaterial(psr, blockBundleAlt, "JunglePlanks", Blocks.JunglePlanks, block);
                SetParticleMaterial(psr, blockBundleAlt, "AcaciaPlanks", Blocks.AcaciaPlanks, block);
                SetParticleMaterial(psr, blockBundleAlt, "DarkOakPlanks", Blocks.DarkOakPlanks, block);

                SetParticleMaterial(psr, blockBundle, "leaves", Blocks.OakLeaves, block);
                SetParticleMaterial(psr, blockBundleAlt, "SpruceLeaves", Blocks.SpruceLeaves, block);
                SetParticleMaterial(psr, blockBundleAlt, "BirchLeaves", Blocks.BirchLeaves, block);
                SetParticleMaterial(psr, blockBundleAlt, "JungleLeaves", Blocks.JungleLeaves, block);
                SetParticleMaterial(psr, blockBundleAlt, "AcaciaLeaves", Blocks.AcaciaLeaves, block);
                SetParticleMaterial(psr, blockBundleAlt, "DarkOakLeaves", Blocks.DarkOakLeaves, block);

                SetParticleMaterial(psr, blockBundle, "stone", Blocks.Stone, block);
                SetParticleMaterial(psr, blockBundle, "cobblestone", Blocks.Cobblestone, block);
                SetParticleMaterial(psr, blockBundle, "brick", Blocks.Brick, block);

                SetParticleMaterial(psr, blockBundleAlt, "CoalOre", Blocks.CoalOre, block);
                SetParticleMaterial(psr, blockBundleAlt, "RedstoneOre", Blocks.RedstoneOre, block);
                SetParticleMaterial(psr, blockBundleAlt, "EmeraldOre", Blocks.EmeraldOre, block);
                SetParticleMaterial(psr, blockBundleAlt, "LapisOre", Blocks.LapisOre, block);
                SetParticleMaterial(psr, blockBundleAlt, "DiamondOre", Blocks.DiamondOre, block);

                SetParticleMaterial(psr, blockBundle, "ironore", Blocks.IronOre, block);
                SetParticleMaterial(psr, blockBundle, "goldore", Blocks.GoldOre, block);

                SetParticleMaterial(psr, blockBundleAlt, "CoalBlock", Blocks.CoalBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "RedstoneBlock", Blocks.RedstoneBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "EmeraldBlock", Blocks.EmeraldBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "LapisBlock", Blocks.LapisBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "DiamondBlock", Blocks.DiamondBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "IronBlock", Blocks.IronBlock, block);
                SetParticleMaterial(psr, blockBundleAlt, "GoldBlock", Blocks.GoldBlock, block);

                SetParticleMaterial(psr, blockBundle, "glass", Blocks.Glass, block);
                SetParticleMaterial(psr, blockBundle, "stainGlass", Blocks.StainedGlass, block);
                SetParticleMaterial(psr, blockBundle, "wool", Blocks.Wool, block);
                SetParticleMaterial(psr, blockBundle, "bookshelf", Blocks.Bookshelf, block);

                SetParticleMaterial(psr, blockBundle, "craft1", Blocks.CraftingTable, block);
                SetParticleMaterial(psr, blockBundle, "furn2", Blocks.Furnace, block);

                SetParticleMaterial(psr, blockBundle, "netherrack", Blocks.Netherrack, block);
                SetParticleMaterial(psr, blockBundle, "packed", Blocks.PackedIce, block);
                SetParticleMaterial(psr, blockBundle, "obsidian", Blocks.Obsidian, block);

                SetParticleMaterial(psr, blockBundleAlt, "SoulSand", Blocks.SoulSand, block);
                SetParticleMaterial(psr, blockBundleAlt, "RegularIce", Blocks.RegularIce, block);
                SetParticleMaterial(psr, blockBundleAlt, "Glowstone", Blocks.Glowstone, block);

                SetParticleMaterial(psr, blockBundleAlt, "SlimeBlock", Blocks.SlimeBlock, block);

                SetParticleMaterial(psr, blockBundleAlt, "PumpkinFront", Blocks.Pumpkin, block);
                SetParticleMaterial(psr, blockBundleAlt, "PumpkinOn", Blocks.Jackolantern, block);
                SetParticleMaterial(psr, blockBundleAlt, "MelonSide", Blocks.Melon, block);
                SetParticleMaterial(psr, blockBundleAlt, "HaySide", Blocks.HayBale, block);
                SetParticleMaterial(psr, blockBundleAlt, "Sponge", Blocks.Sponge, block);
                SetParticleMaterial(psr, blockBundleAlt, "bedrock", Blocks.Bedrock, block);

                SetParticleMaterial(psr, blockBundle, "brick", Blocks.Brick, block);

                psr.material.mainTextureScale = new Vector2(0.2f, 0.2f);
                if (block == Blocks.Wool)
                {
                    psr.material.color = blockColour;
                }
                if (block == Blocks.StainedGlass)
                {
                    psr.material.color = blockColour;
                }

                psr.material.mainTextureOffset = new Vector2(0.1f * Random.Range(0, 11), 0.1f * Random.Range(0, 11));
                partS.Play();
            }

            particleObjectTemp.transform.SetParent(objectStorage.transform, false);
            particleObjectTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 2;
        }

        public void DestroyBlockOptimized(GameObject destroyObject)
        {
            if (destroyObject.GetComponent<MinecraftBlock>() == null)
                return;

            MinecraftBlock destroyObjectBlockLink = destroyObject.GetComponent<MinecraftBlock>();
            Transform tempBlock = destroyObjectBlockLink.minecraftObject.transform;

            foreach (MinecraftBlock link in tempBlock.GetComponentsInChildren<MinecraftBlock>(true))
            {
                if (blockLinks.Contains(link))
                    blockLinks.Remove(link);
            }

            minecraftBlocks.Remove(destroyObjectBlockLink.minecraftObject);
            Destroy(destroyObjectBlockLink.minecraftObject);

            if (minecraftBlocks.Contains(destroyObjectBlockLink.minecraftObject))
                minecraftBlocks.Remove(destroyObjectBlockLink.minecraftObject);
        }

        public void DestroyAllBlocks()
        {
            if (minecraftBlocks.Count == 0)
                return;

            for (int i = 0; i < minecraftBlocks.Count; i++)
            {
                MinecraftBlock QueuedMinecraftBlockLink = minecraftBlocks[i].transform.Find("Collider/Front").GetComponent<MinecraftBlock>();

                DestroyBlockOptimized(QueuedMinecraftBlockLink.gameObject);
            }
        }

        void SetParticleMaterial(ParticleSystemRenderer renderer, AssetBundle bundle, string assetName, Blocks whichBlock, Blocks thisblock)
        {
            if (thisblock != whichBlock)
                return;

            renderer.material = bundle.LoadAsset<Material>(assetName);
        }

        void SetTrampoline(Transform block)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                if (block.GetChild(i).gameObject.name == "Top")
                {
                    block.GetChild(i).gameObject.AddComponent<SlimeBlock>();
                }
            }
        }

        void SetSurfaceIndex(Transform block, int frontIndex, int leftIndex, int rightIndex, int backIndex, int topIndex, int bottomIndex, bool customColour, bool slippery)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                GorillaSurfaceOverride gso = block.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();

                gso.overrideIndex = 0;

                if (block.GetChild(i).gameObject.name == "Top")
                {
                    gso.overrideIndex = topIndex;
                }
                if (block.GetChild(i).gameObject.name == "Bottom")
                {
                    gso.overrideIndex = bottomIndex;
                }
                if (block.GetChild(i).gameObject.name == "Front")
                {
                    gso.overrideIndex = frontIndex;
                }
                if (block.GetChild(i).gameObject.name == "Back")
                {
                    gso.overrideIndex = backIndex;
                }
                if (block.GetChild(i).gameObject.name == "Left")
                {
                    gso.overrideIndex = leftIndex;
                }
                if (block.GetChild(i).gameObject.name == "Right")
                {
                    gso.overrideIndex = rightIndex;
                }

                if (customColour)
                {
                    if (block.GetChild(i).GetComponent<Renderer>() != null)
                    {
                        Renderer renderer = block.GetChild(i).GetComponent<Renderer>();
                        renderer.material.color = colours[currentColourMode];
                    }
                }

                if (slippery)
                {
                    PhysicMaterial slipMat = null;
                    var iceObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "mountainsideice");
                    if (iceObjects.ToList().Count != 0) slipMat = iceObjects.ToList()[0].GetComponent<Collider>().material;
                    block.GetChild(i).gameObject.GetComponent<BoxCollider>().material = slipMat ?? null;
                }
            }
        }

        void SetSurfaceIndexOther(Transform block, int frontIndex, int leftIndex, int rightIndex, int backIndex, int topIndex, int bottomIndex, bool customColour, bool slippery, Color col)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                GorillaSurfaceOverride gso = block.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();

                gso.overrideIndex = 0;

                if (block.GetChild(i).gameObject.name == "Top")
                {
                    gso.overrideIndex = topIndex;
                }
                if (block.GetChild(i).gameObject.name == "Bottom")
                {
                    gso.overrideIndex = bottomIndex;
                }
                if (block.GetChild(i).gameObject.name == "Front")
                {
                    gso.overrideIndex = frontIndex;
                }
                if (block.GetChild(i).gameObject.name == "Back")
                {
                    gso.overrideIndex = backIndex;
                }
                if (block.GetChild(i).gameObject.name == "Left")
                {
                    gso.overrideIndex = leftIndex;
                }
                if (block.GetChild(i).gameObject.name == "Right")
                {
                    gso.overrideIndex = rightIndex;
                }

                if (customColour)
                {
                    if (block.GetChild(i).GetComponent<Renderer>() != null)
                    {
                        Renderer renderer = block.GetChild(i).GetComponent<Renderer>();
                        renderer.material.color = col;
                    }
                }

                if (slippery)
                {
                    block.GetChild(i).gameObject.GetComponent<BoxCollider>().material = Resources.Load<PhysicMaterial>("objects/forest/materials/Slippery");
                }
            }
        }

        public void PlaceBlock()
        {
            float headDistance = Vector3.Distance(block.transform.position, Player.Instance.headCollider.transform.position);

            if (headDistance < 0.75f)
                return;

            float rightHandDistance = Vector3.Distance(block.transform.position, Player.Instance.rightHandTransform.position);

            if (rightHandDistance < 0.685f)
                return;

            float leftHandDistance = Vector3.Distance(block.transform.position, Player.Instance.leftHandTransform.position);

            if (leftHandDistance < 0.685f)
                return;

            float bodyDistance = Vector3.Distance(block.transform.position, Player.Instance.bodyCollider.transform.position);

            if (bodyDistance < 0.5f)
                return;

            if (!raycastExists)
                return;

            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.35f);

            Plugin.Instance.placed++;
            Plugin.Instance.SetSettings();

            switch (modeVersion)
            {
                case 0:

                    GameObject tempBlock = Instantiate(minecraftBlockList[currentBlock]);

                    minecraftBlocks.Add(tempBlock);

                    if (!minecraftBlocks.Contains(tempBlock))
                        minecraftBlocks.Add(tempBlock);

                    tempBlock.transform.SetParent(objectStorageBlock.transform, false);
                    tempBlock.name = minecraftBlockList[currentBlock].name;

                    tempBlock.transform.localScale = Vector3.one;
                    tempBlock.transform.localPosition = block.transform.localPosition;
                    tempBlock.transform.localRotation = block.transform.localRotation;

                    Transform blockColliders = tempBlock.transform.GetChild(0);
                    blockColliders.GetComponent<BoxCollider>().enabled = false;

                    var dataUsed = blkToBD[currentBlock];
                    Blocks usedBlockEnum = dataUsed.CurBlock;
                    SetSurfaceIndex(blockColliders, dataUsed.SurData.front, dataUsed.SurData.left, dataUsed.SurData.right, dataUsed.SurData.back, dataUsed.SurData.up, dataUsed.SurData.down, dataUsed.HasCustomColour, Player.Instance.materialData[currentBlock].slidePercent >= 0.1f);
                    if (dataUsed.IsBouncy) SetTrampoline(blockColliders);

                    foreach (BoxCollider bx in tempBlock.transform.GetComponentsInChildren<BoxCollider>(false))
                    {
                        bx.enabled = true;
                        bx.gameObject.layer = 0;

                        MinecraftBlock bl = bx.gameObject.AddComponent<MinecraftBlock>();

                        bl.minecraftObject = tempBlock;

                        int ccm = currentColourMode;
                        Color col = colours[ccm];

                        bl.blockIndex = currentBlock;
                        if (dataUsed.HasCustomColour) bl.blockColour = col;

                        bl.block = usedBlockEnum;

                        blockLinks.Add(bl);
                    }

                    PlaySpawnBlockAudio(usedBlockEnum, tempBlock.transform.position);

                    break;
            }
        }

        public void CreateBlock(int blockIndex, Vector3 position, Vector3 eulerAngles, Color blockColour)
        {
            GameObject tempBlock = Instantiate(minecraftBlockList[blockIndex]);

            minecraftBlocks.Add(tempBlock);

            if (!minecraftBlocks.Contains(tempBlock))
            {
                minecraftBlocks.Add(tempBlock);
            }

            tempBlock.transform.SetParent(objectStorageBlock.transform, false);
            tempBlock.name = minecraftBlockList[blockIndex].name;

            tempBlock.transform.localScale = Vector3.one;
            tempBlock.transform.position = position;
            tempBlock.transform.eulerAngles = eulerAngles;

            Transform blockColliders = tempBlock.transform.GetChild(0);
            blockColliders.GetComponent<BoxCollider>().enabled = false;

            var dataUsed = blkToBD[blockIndex];
            Blocks usedBlockEnum = dataUsed.CurBlock;
            SetSurfaceIndexOther(blockColliders, dataUsed.SurData.front, dataUsed.SurData.left, dataUsed.SurData.right, dataUsed.SurData.back, dataUsed.SurData.up, dataUsed.SurData.down, dataUsed.HasCustomColour, Player.Instance.materialData[currentBlock].slidePercent >= 0.1f, blockColour);
            if (dataUsed.IsBouncy) SetTrampoline(blockColliders);

            BoxCollider[] boxColliders = tempBlock.transform.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bx in boxColliders)
            {
                bx.enabled = true;
                bx.gameObject.layer = 0;

                MinecraftBlock bl = bx.gameObject.AddComponent<MinecraftBlock>();

                bl.minecraftObject = tempBlock;

                bl.blockColour = Color.white;
                bl.blockIndex = blockIndex;

                if (dataUsed.HasCustomColour) bl.blockColour = blockColour;

                bl.block = usedBlockEnum;

                blockLinks.Add(bl);
            }
        }

        public void DestroyBlock(GameObject destroyObject)
        {
            if (destroyObject.GetComponent<MinecraftBlock>() == null)
                return;

            MinecraftBlock destroyObjectBlockLink = destroyObject.GetComponent<MinecraftBlock>();

            // erfhegrrfo0dwjigvrewr

            Transform tempBlock = destroyObjectBlockLink.minecraftObject.transform;
            MinecraftBlock[] tempLinks = tempBlock.GetComponentsInChildren<MinecraftBlock>();

            foreach (MinecraftBlock link in tempBlock.GetComponentsInChildren<MinecraftBlock>(true))
            {
                if (blockLinks.Contains(link))
                    blockLinks.Remove(link);
            }

            foreach (MinecraftBlock link in tempLinks)
            {
                if (blockLinks.Contains(link))
                {
                    blockLinks.Remove(link);
                }
            }

            DestroyBlock(destroyObjectBlockLink.block, destroyObjectBlockLink.minecraftObject.transform.position, destroyObjectBlockLink.blockColour);

            minecraftBlocks.Remove(destroyObjectBlockLink.minecraftObject);
            Destroy(destroyObjectBlockLink.minecraftObject);

            if (minecraftBlocks.Contains(destroyObjectBlockLink.minecraftObject))
            {
                minecraftBlocks.Remove(destroyObjectBlockLink.minecraftObject);
            }
        }

        #endregion

        #region Start/Update Functions

        void Start()
        {
            Instance = this;

            blockBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DevMinecraftMod.Resources.devminecraft"));
            blockBundleAlt = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DevMinecraftMod.Resources.devminecraftblock"));
            AssetBundle extraBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DevMinecraftMod.Resources.extrablockresource"));

            if (harmony == null)
            {
                harmony = new Harmony("com.dev9998.gorillatag.devminecraftmod");
                harmony.PatchAll();
            }

            objectStorage = new GameObject();
            objectStorage.transform.position = Vector3.zero;
            objectStorage.transform.rotation = Quaternion.identity;
            objectStorage.transform.localScale = Vector3.one;
            objectStorage.name = "DevMinecraftModStorage";

            AddBlocks(blockBundle, "Grass");
            AddBlocks(extraBundle, "Dirt");
            AddBlocks(extraBundle, "OakLog");
            AddBlocks(blockBundleAlt, "LogSpruce");
            AddBlocks(blockBundleAlt, "LogBirch");
            AddBlocks(blockBundleAlt, "LogJungle");
            AddBlocks(blockBundleAlt, "LogAcacia");
            AddBlocks(blockBundleAlt, "LogDark");
            AddBlocks(blockBundle, "Plank");
            AddBlocks(blockBundleAlt, "PlankSpruce");
            AddBlocks(blockBundleAlt, "PlankBirch");
            AddBlocks(blockBundleAlt, "PlankJungle");
            AddBlocks(blockBundleAlt, "PlankAcacia");
            AddBlocks(blockBundleAlt, "PlankDark");
            AddBlocks(blockBundle, "Leaves");
            AddBlocks(blockBundleAlt, "LeavesSpruce");
            AddBlocks(blockBundleAlt, "LeavesBirch");
            AddBlocks(blockBundleAlt, "LeavesJungle");
            AddBlocks(blockBundleAlt, "LeavesAcacia");
            AddBlocks(blockBundleAlt, "LeavesDark");
            AddBlocks(blockBundle, "Cobblestone");
            AddBlocks(blockBundle, "Stone");
            AddBlocks(blockBundleAlt, "CoalOre");
            AddBlocks(blockBundle, "IronOre");
            AddBlocks(blockBundle, "GoldOre");
            AddBlocks(blockBundleAlt, "RedstoneOre");
            AddBlocks(blockBundleAlt, "LapisOre");
            AddBlocks(blockBundleAlt, "EmeraldOre");
            AddBlocks(blockBundleAlt, "DiamondOre");
            AddBlocks(blockBundleAlt, "CoalBlock");
            AddBlocks(blockBundleAlt, "IronBlock");
            AddBlocks(blockBundleAlt, "GoldBlock");
            AddBlocks(blockBundleAlt, "RedstoneBlock");
            AddBlocks(blockBundleAlt, "LapisBlock");
            AddBlocks(blockBundleAlt, "EmeraldBlock");
            AddBlocks(blockBundleAlt, "DiamondBlock");
            AddBlocks(blockBundle, "Glass");
            AddBlocks(blockBundle, "StainGlass");
            AddBlocks(blockBundle, "Wool");
            AddBlocks(blockBundle, "Bookshelf");
            AddBlocks(blockBundle, "CraftingTable");
            AddBlocks(blockBundle, "Furnace");
            AddBlocks(blockBundleAlt, "RegularIce");
            AddBlocks(blockBundle, "PackedIce");
            AddBlocks(blockBundle, "Netherrack");
            AddBlocks(blockBundleAlt, "SoulSand");
            AddBlocks(blockBundle, "Obisdian");
            AddBlocks(blockBundleAlt, "Glowstone");
            AddBlocks(blockBundleAlt, "SlimeBlock");
            AddBlocks(blockBundleAlt, "Pumpkin");
            AddBlocks(blockBundleAlt, "PumpkinOn");
            AddBlocks(blockBundleAlt, "Sponge");
            AddBlocks(blockBundleAlt, "Melon");
            AddBlocks(blockBundleAlt, "Hay");
            AddBlocks(blockBundleAlt, "Bedrock");
            AddBlocks(blockBundle, "Brick");

            // ADDING BLOCKS
            // fire the "AddBlocks" function with the block name and assetbundle, add the blocks particles, surface index, and sounds

            objectStorageBlock = new GameObject();
            objectStorageBlock.transform.position = Vector3.zero;
            objectStorageBlock.transform.rotation = Quaternion.identity;
            objectStorageBlock.transform.localScale = Vector3.one;
            objectStorageBlock.name = "Blocks";
            objectStorageBlock.transform.SetParent(objectStorage.transform, false);

            block = Instantiate(blockBundle.LoadAsset<GameObject>("BlockIndicator"));
            block.transform.localScale = Vector3.one * 1.015f;
            block.transform.SetParent(objectStorage.transform, false);

            blockAlt = Instantiate(blockBundle.LoadAsset<GameObject>("BlockIndicatorRed"));
            blockAlt.transform.localScale = Vector3.one * 1.015f;
            blockAlt.transform.SetParent(objectStorage.transform, false);

            clip = blockBundle.LoadAsset<AudioClip>("clicknew");

            BoxCollider[] boxColliders = block.transform.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bx in boxColliders)
            {
                bx.enabled = false;
                bx.gameObject.layer = 8;
            }

            boxColliders = blockAlt.transform.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bx in boxColliders)
            {
                bx.enabled = false;
                bx.gameObject.layer = 8;
            }

            Transform palm = GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent.Find("palm.01.L");
            itemIndicator = Instantiate(blockBundle.LoadAsset<GameObject>("ItemSelector"));

            itemIndicator.transform.SetParent(palm, false);
            itemIndicator.transform.localPosition = Vector3.zero;
            itemIndicator.transform.localRotation = Quaternion.identity;
            itemIndicator.transform.localScale = Vector3.one * 0.95f;

            itemIndicator.SetActive(false);

            Transform palm2 = GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent.Find("palm.01.R");

            itemShow = new GameObject();
            itemShow.transform.SetParent(palm2, false);
            itemShow.transform.localPosition = new Vector3(-0.048f, 0.038f, 0.003f);
            itemShow.transform.localRotation = Quaternion.Euler(78.077f, 146.811f, -27.275f);
            itemShow.transform.localScale = Vector3.one * 0.07628721f;

            itemShow.SetActive(false);

            itemPickaxe = Instantiate(blockBundle.LoadAsset<GameObject>("DiamondPickaxe"));
            itemPickaxe.transform.SetParent(palm2, false);
            itemPickaxe.transform.localPosition = new Vector3(-0.017f, 0.051f, -0.066f);
            itemPickaxe.transform.localRotation = Quaternion.Euler(55.561f, -11.768f, -279.547f);
            itemPickaxe.transform.localScale = Vector3.one * 0.007977522f;

            itemText = itemIndicator.transform.Find("CurrentBlockText").GetComponent<Text>();
            noticeText = itemIndicator.transform.Find("NoticeText").GetComponent<Text>();

            itemPickaxe.SetActive(false);

            SetSlot(0);

            SetGetSlot(0, false);
            SetEquipSlot(0, false);

            ClearColourSlots();

            SetLoadSave();

            GameObject lnObject = Instantiate(blockBundle.LoadAsset<GameObject>("lineRendererExample"));
            lnObject.transform.position = Vector3.zero;
            lnObject.transform.rotation = Quaternion.identity;
            lnObject.transform.localScale = Vector3.one;

            ln = lnObject.GetComponent<LineRenderer>();
            ln.enabled = false;
        }

        void FixedUpdate()
        {
            if (Time.time >= cooldownTime)
            {
                if (triggerPullled)
                    triggerPullled = false;
            }
        }

        void LateUpdate()
        {
            if (Instance == null || block == null || blockAlt == null || itemIndicator == null || minecraftBlockList.Count == 0 || itemPickaxe == null || itemShow == null || harmony == null)
                return;

            if (!PhotonNetwork.InRoom)
            {
                block.SetActive(false);
                blockAlt.SetActive(false);

                if (minecraftBlocks.Count != 0)
                    foreach (GameObject block in minecraftBlocks)
                        block.SetActive(false);

                itemIndicator.SetActive(false);
                itemShow.SetActive(false);
                itemPickaxe.SetActive(false);
                ln.enabled = false;

                return;
            }

            if (!Plugin.Instance.InRoom)
            {
                block.SetActive(false);
                blockAlt.SetActive(false);
                if (minecraftBlocks.Count != 0)
                {
                    foreach (GameObject block in minecraftBlocks)
                        block.SetActive(false);
                }
                itemIndicator.SetActive(false);
                itemShow.SetActive(false);
                itemPickaxe.SetActive(false);
                ln.enabled = false;

                return;
            }

            if (!Plugin.Instance.enabled)
            {
                block.SetActive(false);
                blockAlt.SetActive(false);
                if (minecraftBlocks.Count != 0)
                {
                    foreach (GameObject block in minecraftBlocks)
                        block.SetActive(true);
                }
                itemIndicator.SetActive(false);
                itemShow.SetActive(false);
                itemPickaxe.SetActive(false);
                ln.enabled = false;

                //hides the block and item indicators but not the actual blocks
                return;
            }

            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool p2DaC2);


            if (p2DaC2 && !middlePulled)
            {
                middlePulled = true;
                Plugin.Instance.modFunction = !Plugin.Instance.modFunction;
            }
            else if (!p2DaC2 && middlePulled)
            {
                middlePulled = false;
            }


            if (!Plugin.Instance.modFunction)
            {
                block.SetActive(false);
                blockAlt.SetActive(false);
                if (minecraftBlocks.Count != 0)
                {
                    foreach (GameObject block in minecraftBlocks)
                        block.SetActive(true);
                }
                itemIndicator.SetActive(false);
                itemShow.SetActive(false);
                itemPickaxe.SetActive(false);
                ln.enabled = false;

                //hides the block and item indicators but not the actual blocks
                return;
            }

            if (Plugin.Instance.sIndicatorEnabled)
            {
                if (raycastExists)
                {
                    block.SetActive(mode == 0);
                    blockAlt.SetActive(mode == 1);
                }
            }
            else
            {
                block.SetActive(false);
                blockAlt.SetActive(false);
            }

            itemShow.SetActive(mode == 0);

            itemPickaxe.SetActive(mode == 1);

            if (Plugin.Instance.sIndicatorEnabled || Plugin.Instance.lIndicatorEnabled)
                itemIndicator.SetActive(true);
            else
                itemIndicator.SetActive(false);

            if (mode == 0)
                ln.material.color = new Color(0, 0.8245816f, 1, 0.2f);
            else
                ln.material.color = new Color(1, 0, 0, 0.2f);

            ln.SetPosition(0, Player.Instance.rightHandTransform.position);

            if (minecraftBlocks.Count != 0)
            {
                foreach (GameObject blk in minecraftBlocks)
                    blk.SetActive(true);
            }

            Player __instance = Player.Instance;

            if (Physics.Raycast(__instance.rightHandTransform.position, -__instance.rightHandTransform.up, out RaycastHit hit, 25, Player.Instance.locomotionEnabledLayers))
            {
                Vector3 newPos = hit.point;

                raycastExists = true;

                if (mode == 0)
                    ln.SetPosition(1, block.transform.position);
                else
                    ln.SetPosition(1, blockAlt.transform.position);

                ln.enabled = Plugin.Instance.lIndicatorEnabled;

                if (hit.transform.gameObject != null && hit.transform.GetComponent<MinecraftBlock>() != null)
                {
                    blockAlt.transform.position = hit.transform.GetComponent<MinecraftBlock>().minecraftObject.transform.position;
                    blockAlt.transform.rotation = hit.transform.GetComponent<MinecraftBlock>().minecraftObject.transform.rotation;
                }
                else
                {
                    blockAlt.transform.eulerAngles = Vector3.zero;
                    blockAlt.transform.position = new Vector3(((Mathf.RoundToInt(newPos.x) != 0) ? (Mathf.RoundToInt(newPos.x / 1f) * 1) : 2), ((Mathf.RoundToInt(newPos.y) != 0) ? (Mathf.RoundToInt(newPos.y / 1f) * 1) : 0), ((Mathf.RoundToInt(newPos.z) != 0) ? (Mathf.RoundToInt(newPos.z / 1f) * 1) : 2));
                }

                block.transform.position = new Vector3(((Mathf.RoundToInt(newPos.x) != 0) ? (Mathf.RoundToInt(newPos.x / 1f) * 1) : 2), ((Mathf.RoundToInt(newPos.y) != 0) ? (Mathf.RoundToInt(newPos.y / 1f) * 1) : 0), ((Mathf.RoundToInt(newPos.z) != 0) ? (Mathf.RoundToInt(newPos.z / 1f) * 1) : 2));

                if (currentBlock == 40 || currentBlock == 41 || currentBlock == 49 || currentBlock == 50)
                    block.transform.eulerAngles = new Vector3(0, ((Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y) != 0) ? (Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y / 90f) * 90) : 0), 0f);
                else if (currentBlock == 2 || currentBlock == 3 || currentBlock == 4 || currentBlock == 5 || currentBlock == 6 || currentBlock == 7)
                    block.transform.eulerAngles = new Vector3(((Mathf.RoundToInt(Player.Instance.rightHandTransform.transform.eulerAngles.x) != 0) ? (Mathf.RoundToInt(Player.Instance.rightHandTransform.transform.eulerAngles.x / 90f) * 90) : 0), ((Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y) != 0) ? (Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y / 90f) * 90) : 0), 0f);
                else
                    block.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                raycastExists = false;

                ln.enabled = false;
                ln.SetPosition(1, Player.Instance.rightHandTransform.position);
                block.SetActive(false);
                blockAlt.SetActive(false);
            }

            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 p2Da);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool p2DaC);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerDown);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out bool gripDown);

            if (p2DaC && mode == 1)
            {
                while (Instance.minecraftBlocks.Count != 0)
                    Instance.DestroyAllBlocks();
            }

            if (triggerDown && !triggerPullled)
            {
                triggerPullled = true;
                cooldownTime = Time.time + 0.3f;

                if (mode == 0)
                    PlaceBlock();

                else if (mode == 1 && hit.transform.gameObject != null)
                {
                    DestroyBlock(hit.transform.gameObject);

                    GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.35f);

                    Plugin.Instance.removed++;
                    Plugin.Instance.SetSettings();
                }
            }
            else if (!triggerDown && triggerPullled)
            {
                cooldownTime = Time.time;
                triggerPullled = false;
            }

            if (gripDown && !gripPulled)
            {
                gripPulled = true;

                if (mode == 0)
                    mode = 1;
                else
                    mode = 0;

                cooldownTime = Time.time;
            }
            else if (!gripDown && gripPulled)
                gripPulled = false;
        }

        #endregion

        #region Harmony Functions

        [HarmonyPatch(typeof(PlayerPrefs))]
        [HarmonyPatch("SetFloat", MethodType.Normal)]
        public class OnColourChanged
        {
            private static void Postfix(string key)
            {
                bool colourSwap = false;
                if (key == "redValue")
                    colourSwap = true;

                if (key == "greenValue")
                    colourSwap = true;

                if (key == "blueValue")
                    colourSwap = true;

                if (colourSwap)
                {
                    Plugin.Instance.mf.UpdateAssets(); // changes handheld wool colour whenever you change your colour
                    Plugin.Instance.mf.UpdateColour();
                }
            }
        }

        #endregion

        #region Additional Functions

        Color GetColour(float max)
        {
            float r = Mathf.Clamp(PlayerPrefs.GetFloat("redValue"), 0, max);
            float g = Mathf.Clamp(PlayerPrefs.GetFloat("greenValue"), 0, max);
            float b = Mathf.Clamp(PlayerPrefs.GetFloat("blueValue"), 0, max);
            return new Color(r, g, b, 1);
        }

        IEnumerator SetNoticeText(string text, float timeSpent, bool error)
        {
            string tempText = text;

            if (error)
                noticeText.color = Color.red;
            else
                noticeText.color = Color.white;

            noticeText.text = tempText;

            yield return new WaitForSeconds(timeSpent);

            if (noticeText.text == tempText)
                noticeText.text = "";

            yield break;
        }

        void AddBlocks(AssetBundle bundle, string blockName)
        {
            try
            {
                GameObject objec = Instantiate(bundle.LoadAsset<GameObject>(blockName));
                minecraftBlockList.Add(objec);
                objec.transform.SetParent(objectStorage.transform);
                objec.transform.localPosition = Vector3.zero;

                string blockNameFinal = blockName

                    .Replace("CraftingTable", "Crafting Table")

                    .Replace("IronOre", "Iron Ore")
                    .Replace("GoldOre", "Gold Ore")

                    .Replace("PackedIce", "Packed Ice")

                    .Replace("Plank", "Oak Planks")

                    .Replace("Log", "Oak Log")

                    .Replace("Obisdian", "Obsidian")

                    .Replace("Leaves", "Oak Leaves")

                    .Replace("Brick", "Bricks")

                    .Replace("Oak LogSpruce", "Spruce Log")
                    .Replace("Oak LogBirch", "Birch Log")
                    .Replace("Oak LogJungle", "Jungle Log")
                    .Replace("Oak LogAcacia", "Acacia Log")
                    .Replace("Oak LogDark", "Dark Oak Log")

                    .Replace("Oak PlanksSpruce", "Spruce Planks")
                    .Replace("Oak PlanksBirch", "Birch Planks")
                    .Replace("Oak PlanksJungle", "Jungle Planks")
                    .Replace("Oak PlanksAcacia", "Acacia Planks")
                    .Replace("Oak PlanksDark", "Dark Oak Planks")
                    .Replace("Oak LeavesSpruce", "Spruce Leaves")
                    .Replace("Oak LeavesBirch", "Birch Leaves")
                    .Replace("Oak LeavesJungle", "Jungle Leaves")
                    .Replace("Oak LeavesAcacia", "Acacia Leaves")
                    .Replace("Oak LeavesDark", "Dark Oak Leaves")

                    .Replace("CoalOre", "Coal Ore")
                    .Replace("IronOre", "Iron Ore")
                    .Replace("GoldOre", "Gold Ore")
                    .Replace("RedstoneOre", "Redstone Ore")
                    .Replace("LapisOre", "Lapis Lazuli Ore")
                    .Replace("EmeraldOre", "Emerald Ore")
                    .Replace("DiamondOre", "Diamond Ore")

                    .Replace("CoalBlock", "Block of Coal")
                    .Replace("IronBlock", "Block of Iron")
                    .Replace("GoldBlock", "Block of Gold")
                    .Replace("RedstoneBlock", "Block of Redstone")
                    .Replace("LapisBlock", "Lapis Lazuli Block")
                    .Replace("EmeraldBlock", "Block of Emerald")
                    .Replace("DiamondBlock", "Block of Diamomd")

                    .Replace("StainGlass", "Stained Glass")
                    .Replace("RegularIce", "Ice")
                    .Replace("SoulSand", "Soul Sand")

                    .Replace("SlimeBlock", "Slime Block")
                    .Replace("PumpkinOn", "Jack o'Lantern");

                minecraftBlockListString.Add(blockNameFinal);
            }
            catch
            {
                Console.WriteLine("issue with " + blockName);
            }
        }

        public void SetSlot(int slot)
        {
            Transform selections = itemIndicator.transform.Find("ItemSelections");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;

                if (!selections.GetChild(i).GetComponent<ButtonMain>())
                    selections.GetChild(i).gameObject.AddComponent<ButtonMain>().slot = i;

                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
                if (i == slot)
                    selections.GetChild(i).GetComponent<Renderer>().enabled = true;

            }
            UpdateAssets();
        }

        public void SetLoadSave()
        {
            Transform selections = itemIndicator.transform.Find("LoadSave");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;

                if (!selections.GetChild(i).GetComponent<ButtonRecover>())
                {
                    selections.GetChild(i).gameObject.AddComponent<ButtonRecover>().eq = selections.GetChild(i).name == "load";
                }

            }
        }

        public void SetGetSlot(int slot, bool set)
        {
            Transform selections = itemIndicator.transform.Find("GetColour");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;

                if (!selections.GetChild(i).GetComponent<ButtonColour>())
                    selections.GetChild(i).gameObject.AddComponent<ButtonColour>().slot = i;

                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
                if (i == slot)
                    selections.GetChild(i).GetComponent<Renderer>().enabled = true;
            }

            if (set)
            {
                UpdateAssets();
            }

            UpdateColour();
        }

        public void SetEquipSlot(int slot, bool set)
        {
            Transform selections = itemIndicator.transform.Find("SetColour");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;

                if (!selections.GetChild(i).GetComponent<ButtonColour>())
                {
                    selections.GetChild(i).gameObject.AddComponent<ButtonColour>().slot = i;
                    selections.GetChild(i).GetComponent<ButtonColour>().eq = true;
                }

                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
                if (i == slot)
                    selections.GetChild(i).GetComponent<Renderer>().enabled = true;
            }

            if (set)
            {
                colours[slot] = GetColour(0.85f);
                UpdateAssets();
            }

            UpdateColour();
        }

        public void ClearColourSlots()
        {
            Transform selections = itemIndicator.transform.Find("GetColour");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;
                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
            }

            selections = itemIndicator.transform.Find("SetColour");
            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).gameObject.layer = 18;
                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
        }

        void UpdateColour()
        {
            Transform selections = itemIndicator.transform.Find("SetColour");

            for (int i = 0; i < selections.childCount; i++)
                selections.GetChild(i).Find("Items").GetComponent<Renderer>().material.color = GetColour(0.85f);

            selections = itemIndicator.transform.Find("GetColour");

            for (int i = 0; i < selections.childCount; i++)
                selections.GetChild(i).Find("Items").GetComponent<Renderer>().material.color = colours[i];
        }

        void UpdateAssets()
        {
            itemText.text = "Block: " + minecraftBlockListString[currentBlock];

            Transform ISTransform = itemShow.transform;

            if (ISTransform.childCount != 0)
            {
                if (ISTransform.childCount >= 1)
                    for (int i = 0; i < ISTransform.childCount; i++)
                        Destroy(ISTransform.GetChild(i).gameObject);
                else
                    Destroy(ISTransform.GetChild(0).gameObject);
            }

            GameObject tempBlock = Instantiate(minecraftBlockList[currentBlock]);
            tempBlock.transform.SetParent(itemShow.transform, false);

            tempBlock.transform.localScale = Vector3.one;
            tempBlock.transform.localPosition = Vector3.zero;
            tempBlock.transform.localRotation = Quaternion.identity;

            BoxCollider[] boxColliders = tempBlock.transform.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bx in boxColliders)
                bx.enabled = false;

            if (currentBlock == 37 || currentBlock == 38)
            {
                Renderer[] renderers = tempBlock.transform.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                    renderer.material.color = colours[currentColourMode];
            }
        }

        #endregion

    }
}
