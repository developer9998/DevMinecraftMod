using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using HarmonyLib;
using Photon.Pun;
using GorillaLocomotion;
using System.Collections;

// programmed by dev.
// material data from me, graic, and frogrilla on the official gorilla tag modding discord

// 10/17/2022
// a ton of this code is pretty old, although because its worth over 2.9k lines i'm not in the mood to reprogram large chunks of it rn

// 11/3/2022
// the mod is literally my second most viewed mod on github

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

        #region Place/Destroy Functions

        void PlaySpawnBlockAudio(Blocks block, Vector3 blockPosition)
        {
            GameObject soundObjectTemp = Instantiate(blockBundle.LoadAsset<GameObject>("SoundExample"));
            soundObjectTemp.transform.position = blockPosition;
            AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

            switch (block)
            {
                case Blocks.Grass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Dirt:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Dirt{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SpruceLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JungleLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SprucePlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JunglePlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Cobblestone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Brick:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Stone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Wool:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Fabric{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SpruceLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JungleLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Glass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassPlace{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CraftingTable:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Furnace:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.IronOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.GoldOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DiamondOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.RedstoneOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.EmeraldOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CoalOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.LapisOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Netherrack:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.PackedIce:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"IcePlace{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Obsidian:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Bookshelf:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CoalBlock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.LapisBlock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.IronBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.GoldBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.RedstoneBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.EmeraldBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.DiamondBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.StainedGlass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassPlace{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Glowstone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"IcePlace{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SoulSand:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Sand{Random.Range(1, 3)}"), 0.2f); break;
                case Blocks.RegularIce:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"IcePlace{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SlimeBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"SlimePlace{Random.Range(1, 3)}"), 0.2f); break;
                case Blocks.Pumpkin:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Jackolantern:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Melon:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Bedrock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.HayBale:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Sponge:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
            }

            audioSourceTemp.transform.SetParent(objectStorage.transform, false);
            audioSourceTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 3;
        }

        void DestroyBlock(Blocks block, Vector3 blockPosition, Color blockColour)
        {
            GameObject soundObjectTemp = Instantiate(blockBundle.LoadAsset<GameObject>("SoundExample"));
            soundObjectTemp.transform.position = blockPosition;
            AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

            switch (block)
            {
                case Blocks.Grass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Dirt:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Dirt{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SpruceLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JungleLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakLog:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SprucePlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JunglePlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakPlanks:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Cobblestone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Brick:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Stone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Wool:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Fabric{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.OakLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SpruceLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.BirchLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.JungleLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.AcaciaLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DarkOakLeaves:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Glass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassBreak{Random.Range(1, 4)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CraftingTable:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Furnace:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.IronOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.GoldOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.DiamondOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.RedstoneOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.EmeraldOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CoalOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.LapisOre:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Netherrack:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.PackedIce:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassBreak{Random.Range(1, 4)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Obsidian:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.Bookshelf:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Plank{Random.Range(1, 3)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.CoalBlock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.LapisBlock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.IronBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.GoldBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.RedstoneBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.EmeraldBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.DiamondBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Metal{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.StainedGlass:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassBreak{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Glowstone:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassBreak{Random.Range(1, 4)}"), Plugin.Instance.blockVolume); break;
                case Blocks.SoulSand:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"Sand{Random.Range(1, 3)}"), 0.2f); break;
                case Blocks.RegularIce:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"GlassBreak{Random.Range(1, 4)}"), Plugin.Instance.blockVolume);
                    break;
                case Blocks.SlimeBlock:
                    audioSourceTemp.PlayOneShot(blockBundleAlt.LoadAsset<AudioClip>($"SlimePlace{Random.Range(1, 3)}"), 0.2f); break;
                case Blocks.Pumpkin:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Jackolantern:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Melon:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Log{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Bedrock:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Stone{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.HayBale:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
                case Blocks.Sponge:
                    audioSourceTemp.PlayOneShot(blockBundle.LoadAsset<AudioClip>($"Grass{Random.Range(1, 3)}"), Plugin.Instance.blockVolume); break;
            }

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
                    block.GetChild(i).gameObject.GetComponent<BoxCollider>().material = Resources.Load<PhysicMaterial>("objects/forest/materials/Slippery");
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

                    Blocks usedBlockEnum = Blocks.Grass;

                    if (currentBlock == 0)
                    {
                        SetSurfaceIndex(blockColliders, 14, 14, 14, 14, 7, 14, false, false);
                    }
                    else if (currentBlock == 1)
                    {
                        SetSurfaceIndex(blockColliders, 14, 14, 14, 14, 14, 14, false, false);
                        usedBlockEnum = Blocks.Dirt;
                    }
                    else if (currentBlock == 2)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.OakLog;
                    }
                    else if (currentBlock == 3)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.SpruceLog;
                    }
                    else if (currentBlock == 4)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.BirchLog;
                    }
                    else if (currentBlock == 5)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.JungleLog;
                    }
                    else if (currentBlock == 6)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.AcaciaLog;
                    }
                    else if (currentBlock == 7)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 9, 9, false, false);
                        usedBlockEnum = Blocks.DarkOakLog;
                    }
                    else if (currentBlock == 8)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.OakPlanks;
                    }
                    else if (currentBlock == 9)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.SprucePlanks;
                    }
                    else if (currentBlock == 10)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.BirchPlanks;
                    }
                    else if (currentBlock == 11)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.JunglePlanks;
                    }
                    else if (currentBlock == 12)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.AcaciaPlanks;
                    }
                    else if (currentBlock == 13)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.DarkOakPlanks;
                    }
                    else if (currentBlock == 14)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.OakLeaves;
                    }
                    else if (currentBlock == 15)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.SpruceLeaves;
                    }
                    else if (currentBlock == 16)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.BirchLeaves;
                    }
                    else if (currentBlock == 17)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.JungleLeaves;
                    }
                    else if (currentBlock == 18)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.AcaciaLeaves;
                    }
                    else if (currentBlock == 19)
                    {
                        SetSurfaceIndex(blockColliders, 31, 31, 31, 31, 31, 31, false, false);
                        usedBlockEnum = Blocks.DarkOakLeaves;
                    }
                    else if (currentBlock == 20)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.Cobblestone;
                    }
                    else if (currentBlock == 21)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.Stone;
                    }
                    else if (currentBlock == 22)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.CoalOre;
                    }
                    else if (currentBlock == 23)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.IronOre;
                    }
                    else if (currentBlock == 24)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.GoldOre;
                    }
                    else if (currentBlock == 25)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.RedstoneOre;
                    }
                    else if (currentBlock == 26)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.LapisOre;
                    }
                    else if (currentBlock == 27)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.EmeraldOre;
                    }
                    else if (currentBlock == 28)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.DiamondOre;
                    }
                    else if (currentBlock == 29)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.CoalBlock;
                    }
                    else if (currentBlock == 30)
                    {
                        SetSurfaceIndex(blockColliders, 80, 80, 80, 80, 80, 80, false, false);
                        usedBlockEnum = Blocks.IronBlock;
                    }
                    else if (currentBlock == 31)
                    {
                        SetSurfaceIndex(blockColliders, 80, 80, 80, 80, 80, 80, false, false);
                        usedBlockEnum = Blocks.GoldBlock;
                    }
                    else if (currentBlock == 32)
                    {
                        SetSurfaceIndex(blockColliders, 80, 80, 80, 80, 80, 80, false, false);
                        usedBlockEnum = Blocks.RedstoneBlock;
                    }
                    else if (currentBlock == 33)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.LapisBlock;
                    }
                    else if (currentBlock == 34)
                    {
                        SetSurfaceIndex(blockColliders, 80, 80, 80, 80, 80, 80, false, false);
                        usedBlockEnum = Blocks.EmeraldBlock;
                    }
                    else if (currentBlock == 35)
                    {
                        SetSurfaceIndex(blockColliders, 80, 80, 80, 80, 80, 80, false, false);
                        usedBlockEnum = Blocks.DiamondBlock;
                    }
                    else if (currentBlock == 36)
                    {
                        SetSurfaceIndex(blockColliders, 55, 55, 55, 55, 55, 55, false, false);
                        usedBlockEnum = Blocks.Glass;
                    }
                    else if (currentBlock == 37)
                    {
                        SetSurfaceIndex(blockColliders, 55, 55, 55, 55, 55, 55, true, false);
                        usedBlockEnum = Blocks.StainedGlass;
                    }
                    else if (currentBlock == 38)
                    {
                        SetSurfaceIndex(blockColliders, 3, 3, 3, 3, 3, 3, true, false);
                        usedBlockEnum = Blocks.Wool;
                    }
                    else if (currentBlock == 39)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.Bookshelf;
                    }
                    else if (currentBlock == 40)
                    {
                        SetSurfaceIndex(blockColliders, 9, 9, 9, 9, 9, 9, false, false);
                        usedBlockEnum = Blocks.CraftingTable;
                    }
                    else if (currentBlock == 41)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.Furnace;
                    }
                    else if (currentBlock == 42)
                    {
                        SetSurfaceIndex(blockColliders, 59, 59, 59, 59, 59, 59, false, true);
                        usedBlockEnum = Blocks.RegularIce;
                    }
                    else if (currentBlock == 43)
                    {
                        SetSurfaceIndex(blockColliders, 59, 59, 59, 59, 59, 59, false, true);
                        usedBlockEnum = Blocks.PackedIce;
                    }
                    else if (currentBlock == 44)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);

                        usedBlockEnum = Blocks.Netherrack;
                    }
                    else if (currentBlock == 45)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);

                        usedBlockEnum = Blocks.SoulSand;
                    }
                    else if (currentBlock == 46)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);

                        usedBlockEnum = Blocks.Obsidian;
                    }
                    else if (currentBlock == 47)
                    {
                        SetSurfaceIndex(blockColliders, 55, 55, 55, 55, 55, 55, false, false);
                        usedBlockEnum = Blocks.Glowstone;
                    }
                    else if (currentBlock == 48)
                    {
                        SetSurfaceIndex(blockColliders, 82, 82, 82, 82, 82, 82, false, false);
                        usedBlockEnum = Blocks.SlimeBlock;
                        SetTrampoline(blockColliders);
                    }
                    else if (currentBlock == 49)
                    {
                        SetSurfaceIndex(blockColliders, 81, 81, 81, 81, 81, 81, false, false);
                        usedBlockEnum = Blocks.Pumpkin;
                    }
                    else if (currentBlock == 50)
                    {
                        SetSurfaceIndex(blockColliders, 81, 81, 81, 81, 81, 81, false, false);
                        usedBlockEnum = Blocks.Jackolantern;
                    }
                    else if (currentBlock == 51)
                    {
                        SetSurfaceIndex(blockColliders, 7, 7, 7, 7, 7, 7, false, false);
                        usedBlockEnum = Blocks.Sponge;
                    }
                    else if (currentBlock == 52)
                    {
                        SetSurfaceIndex(blockColliders, 8, 8, 8, 8, 8, 8, false, false);
                        usedBlockEnum = Blocks.Melon;
                    }
                    else if (currentBlock == 53)
                    {
                        SetSurfaceIndex(blockColliders, 7, 7, 7, 7, 7, 7, false, false);
                        usedBlockEnum = Blocks.HayBale;
                    }
                    else if (currentBlock == 54)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.Bedrock;
                    }
                    else if (currentBlock == 55)
                    {
                        SetSurfaceIndex(blockColliders, 0, 0, 0, 0, 0, 0, false, false);
                        usedBlockEnum = Blocks.Brick;
                    }

                    BoxCollider[] boxColliders = tempBlock.transform.GetComponentsInChildren<BoxCollider>();

                    foreach (BoxCollider bx in boxColliders)
                    {
                        bx.enabled = true;
                        bx.gameObject.layer = 0;

                        MinecraftBlock bl = bx.gameObject.AddComponent<MinecraftBlock>();

                        bl.minecraftObject = tempBlock;

                        int ccm = currentColourMode;
                        Color col = colours[ccm];

                        bl.blockIndex = currentBlock;

                        if (currentBlock == 37 || currentBlock == 38)
                            bl.blockColour = col;

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

            Blocks usedBlockEnum = Blocks.Grass;

            if (blockIndex == 0)
            {
                SetSurfaceIndexOther(blockColliders, 14, 14, 14, 14, 7, 14, false, false, blockColour);
            }
            else if (blockIndex == 1)
            {
                SetSurfaceIndexOther(blockColliders, 14, 14, 14, 14, 14, 14, false, false, blockColour);
                usedBlockEnum = Blocks.Dirt;
            }
            else if (blockIndex == 2)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.OakLog;
            }
            else if (blockIndex == 3)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.SpruceLog;
            }
            else if (blockIndex == 4)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.BirchLog;
            }
            else if (blockIndex == 5)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.JungleLog;
            }
            else if (blockIndex == 6)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.AcaciaLog;
            }
            else if (blockIndex == 7)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.DarkOakLog;
            }
            else if (blockIndex == 8)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.OakPlanks;
            }
            else if (blockIndex == 9)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.SprucePlanks;
            }
            else if (blockIndex == 10)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.BirchPlanks;
            }
            else if (blockIndex == 11)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.JunglePlanks;
            }
            else if (blockIndex == 12)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.AcaciaPlanks;
            }
            else if (blockIndex == 13)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.DarkOakPlanks;
            }
            else if (blockIndex == 14)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.OakLeaves;
            }
            else if (blockIndex == 15)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.SpruceLeaves;
            }
            else if (blockIndex == 16)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.BirchLeaves;
            }
            else if (blockIndex == 17)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.JungleLeaves;
            }
            else if (blockIndex == 18)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.AcaciaLeaves;
            }
            else if (blockIndex == 19)
            {
                SetSurfaceIndexOther(blockColliders, 31, 31, 31, 31, 31, 31, false, false, blockColour);
                usedBlockEnum = Blocks.DarkOakLeaves;
            }
            else if (blockIndex == 20)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.Cobblestone;
            }
            else if (blockIndex == 21)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.Stone;
            }
            else if (blockIndex == 22)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.CoalOre;
            }
            else if (blockIndex == 23)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.IronOre;
            }
            else if (blockIndex == 24)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.GoldOre;
            }
            else if (blockIndex == 25)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.RedstoneOre;
            }
            else if (blockIndex == 26)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.LapisOre;
            }
            else if (blockIndex == 27)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.EmeraldOre;
            }
            else if (blockIndex == 28)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.DiamondOre;
            }
            else if (blockIndex == 29)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.CoalBlock;
            }
            else if (blockIndex == 30)
            {
                SetSurfaceIndexOther(blockColliders, 80, 80, 80, 80, 80, 80, false, false, blockColour);
                usedBlockEnum = Blocks.IronBlock;
            }
            else if (blockIndex == 31)
            {
                SetSurfaceIndexOther(blockColliders, 80, 80, 80, 80, 80, 80, false, false, blockColour);
                usedBlockEnum = Blocks.GoldBlock;
            }
            else if (blockIndex == 32)
            {
                SetSurfaceIndexOther(blockColliders, 80, 80, 80, 80, 80, 80, false, false, blockColour);
                usedBlockEnum = Blocks.RedstoneBlock;
            }
            else if (blockIndex == 33)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.LapisBlock;
            }
            else if (blockIndex == 34)
            {
                SetSurfaceIndexOther(blockColliders, 80, 80, 80, 80, 80, 80, false, false, blockColour);
                usedBlockEnum = Blocks.EmeraldBlock;
            }
            else if (blockIndex == 35)
            {
                SetSurfaceIndexOther(blockColliders, 80, 80, 80, 80, 80, 80, false, false, blockColour);
                usedBlockEnum = Blocks.DiamondBlock;
            }
            else if (blockIndex == 36)
            {
                SetSurfaceIndexOther(blockColliders, 55, 55, 55, 55, 55, 55, false, false, blockColour);
                usedBlockEnum = Blocks.Glass;
            }
            else if (blockIndex == 37)
            {
                SetSurfaceIndexOther(blockColliders, 55, 55, 55, 55, 55, 55, true, false, blockColour);
                usedBlockEnum = Blocks.StainedGlass;
            }
            else if (blockIndex == 38)
            {
                SetSurfaceIndexOther(blockColliders, 3, 3, 3, 3, 3, 3, true, false, blockColour);
                usedBlockEnum = Blocks.Wool;
            }
            else if (blockIndex == 39)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.Bookshelf;
            }
            else if (blockIndex == 40)
            {
                SetSurfaceIndexOther(blockColliders, 9, 9, 9, 9, 9, 9, false, false, blockColour);
                usedBlockEnum = Blocks.CraftingTable;
            }
            else if (blockIndex == 41)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.Furnace;
            }
            else if (blockIndex == 42)
            {
                SetSurfaceIndexOther(blockColliders, 59, 59, 59, 59, 59, 59, false, true, blockColour);
                usedBlockEnum = Blocks.RegularIce;
            }
            else if (blockIndex == 43)
            {
                SetSurfaceIndexOther(blockColliders, 59, 59, 59, 59, 59, 59, false, true, blockColour);
                usedBlockEnum = Blocks.PackedIce;
            }
            else if (blockIndex == 44)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);

                usedBlockEnum = Blocks.Netherrack;
            }
            else if (blockIndex == 45)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);

                usedBlockEnum = Blocks.SoulSand;
            }
            else if (blockIndex == 46)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);

                usedBlockEnum = Blocks.Obsidian;
            }
            else if (blockIndex == 47)
            {
                SetSurfaceIndexOther(blockColliders, 55, 55, 55, 55, 55, 55, false, false, blockColour);
                usedBlockEnum = Blocks.Glowstone;
            }
            else if (blockIndex == 48)
            {
                SetSurfaceIndexOther(blockColliders, 82, 82, 82, 82, 82, 82, false, false, blockColour);
                usedBlockEnum = Blocks.SlimeBlock;
                SetTrampoline(blockColliders);
            }
            else if (blockIndex == 49)
            {
                SetSurfaceIndexOther(blockColliders, 81, 81, 81, 81, 81, 81, false, false, blockColour);
                usedBlockEnum = Blocks.Pumpkin;
            }
            else if (blockIndex == 50)
            {
                SetSurfaceIndexOther(blockColliders, 81, 81, 81, 81, 81, 81, false, false, blockColour);
                usedBlockEnum = Blocks.Jackolantern;
            }
            else if (blockIndex == 51)
            {
                SetSurfaceIndexOther(blockColliders, 7, 7, 7, 7, 7, 7, false, false, blockColour);
                usedBlockEnum = Blocks.Sponge;
            }
            else if (blockIndex == 52)
            {
                SetSurfaceIndexOther(blockColliders, 8, 8, 8, 8, 8, 8, false, false, blockColour);
                usedBlockEnum = Blocks.Melon;
            }
            else if (blockIndex == 53)
            {
                SetSurfaceIndexOther(blockColliders, 7, 7, 7, 7, 7, 7, false, false, blockColour);
                usedBlockEnum = Blocks.HayBale;
            }
            else if (blockIndex == 54)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.Bedrock;
            }
            else if (blockIndex == 55)
            {
                SetSurfaceIndexOther(blockColliders, 0, 0, 0, 0, 0, 0, false, false, blockColour);
                usedBlockEnum = Blocks.Brick;
            }

            BoxCollider[] boxColliders = tempBlock.transform.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bx in boxColliders)
            {
                bx.enabled = true;
                bx.gameObject.layer = 0;

                MinecraftBlock bl = bx.gameObject.AddComponent<MinecraftBlock>();

                bl.minecraftObject = tempBlock;

                bl.blockColour = Color.white;
                bl.blockIndex = blockIndex;

                if (blockIndex == 37 || blockIndex == 38)
                {
                    bl.blockColour = blockColour;
                }

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

            if (harmony == null)
            {
                harmony = new Harmony("com.dev9998.gorillatag.devminecraftmod");
                harmony.PatchAll();
            }

            AddBlocks(blockBundle, "Grass");
            AddBlocks(blockBundle, "Dirt");
            AddBlocks(blockBundle, "Log");
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

            objectStorage = new GameObject();
            objectStorage.transform.position = Vector3.zero;
            objectStorage.transform.rotation = Quaternion.identity;
            objectStorage.transform.localScale = Vector3.one;
            objectStorage.name = "DevMinecraftModStorage";

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
            GameObject objec = bundle.LoadAsset<GameObject>(blockName);
            minecraftBlockList.Add(objec);

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
