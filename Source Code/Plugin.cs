using System;
using System.ComponentModel;
using BepInEx;
using DevMinecraftMod.CI;
using DevMinecraftMod.Base;
using DevMinecraftMod.Music;
using Bepinject;
using Utilla;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace DevMinecraftMod
{

    [ModdedGamemode]
    [Description("HauntedModMenu")]
    [BepInDependency("tonimacaroni.computerinterface", "1.5.2")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.5")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;

        public MinecraftFunction mf; // mother fucker 😲
        private bool mfExists = false;

        public bool modFunction = true;

        public MinecraftMusic mm;
        private bool mmExists;

        public MinecraftRecoverFunction mrf;
        private bool mrfExists;

        private bool inRoom;

        public bool sIndicatorEnabled = true;
        public bool lIndicatorEnabled = false;
        public float musicVolume = 0.05f;
        public float blockVolume = 0.25f;

        public bool stainedGlass;

        public bool isPlayerModelExist = false;
        public string location;
        public string dataLocation;
        public int placed;
        public int removed;

        public SaveData data = new SaveData();

        private void OnEnable()
        {
            if (mf == null)
                mfExists = false;
        }

        private void OnDisable()
        {
            if (mf != null)
                mfExists = true;
        }

        void Awake()
        {
            Instance = this;

            Events.GameInitialized += OnGameInitialized;

            Zenjector.Install<MainInstaller>().OnProject();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            if (mf == null && !mfExists)
            {
                mf = gameObject.AddComponent<MinecraftFunction>();
                mfExists = true;

                Destroy(GameObject.Find("NetworkTriggers/QuitBox").GetComponent<GorillaQuitBox>());
                GameObject.Find("NetworkTriggers/QuitBox").AddComponent<MinecraftQuitBox>();

                if (mm == null && !mmExists)
                {
                    mm = gameObject.AddComponent<MinecraftMusic>();
                    mmExists = true;
                }

                if (mrf == null && !mrfExists)
                {
                    mrf = gameObject.AddComponent<MinecraftRecoverFunction>();
                    mrfExists = true;
                }

                location = Directory.GetCurrentDirectory();
                location += $"\\BepInEx\\plugins\\{PluginInfo.Name}\\Data";

                if (!Directory.Exists(location))
                    Directory.CreateDirectory(location);

                dataLocation = location + $"\\OptionData.json";

                GetSettings();
            }
        }

        public void GetSettings()
        {
            if (File.Exists(dataLocation))
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(dataLocation));
            else
                File.WriteAllText(dataLocation, JsonUtility.ToJson(data));

            sIndicatorEnabled = data.square;
            lIndicatorEnabled = data.line;
            musicVolume = data.musicVolume;
            blockVolume = data.blockVolume;
            placed = data.totalBlocksPlaced;
            removed = data.totalBlocksRemoved;
        }

        public void SetSettings()
        {
            data.square = sIndicatorEnabled;
            data.line = lIndicatorEnabled;
            data.musicVolume = musicVolume;
            data.blockVolume = blockVolume;
            data.totalBlocksPlaced = placed;
            data.totalBlocksRemoved = removed;

            File.WriteAllText(dataLocation, JsonUtility.ToJson(data));

            if (MinecraftView.Instance != null)
                MinecraftView.Instance.UpdateScreen();
        }

        public bool GetRoomState()
        {
            return inRoom;
        }

        [ModdedGamemodeJoin] private void RoomJoinedModded() => inRoom = true;
        [ModdedGamemodeLeave] private void RoomLeftModded() => inRoom = false;
    }
}
