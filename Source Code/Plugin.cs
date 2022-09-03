using System;
using System.ComponentModel;
using BepInEx;
using DevMinecraftMod.CI;
using DevMinecraftMod.Base;
using DevMinecraftMod.Music;
using Bepinject;
using Utilla;
using UnityEngine;

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
        public bool mfExists = false;

        public MinecraftMusic mm;
        public bool mmExists;

        private bool inRoom;

        public bool sIndicatorEnabled = true;
        public bool lIndicatorEnabled = false;
        public float musicVolume = 0.05f;

        public bool stainedGlass;

        private void OnEnable()
        {
            if (mf == null)
            {
                mfExists = false;
            }
        }

        private void OnDisable()
        {
            if (mf != null)
            {
                mfExists = true;
            }
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

                GetSettings();

                if (mm == null && !mmExists)
                {
                    mm = gameObject.AddComponent<MinecraftMusic>();
                    mmExists = true;
                }
            }
        }

        private void GetSettings()
        {
            int sIndicatorEnabledInt = PlayerPrefs.GetInt("isSIndicatorEnabledMinecraft0", 1);
            int lIndicatorEnabledInt = PlayerPrefs.GetInt("isLIndicatorEnabledMinecraft0", 0);

            sIndicatorEnabled = sIndicatorEnabledInt == 1;
            lIndicatorEnabled = lIndicatorEnabledInt == 1;
            musicVolume = PlayerPrefs.GetFloat("isMusicVolumeMinecraft0", 0.05f);
        }

        public void SetSettings()
        {
            PlayerPrefs.SetInt("isSIndicatorEnabledMinecraft0", sIndicatorEnabled ? 1 : 0);
            PlayerPrefs.SetInt("isLIndicatorEnabledMinecraft0", lIndicatorEnabled ? 1 : 0);
            PlayerPrefs.SetFloat("isMusicVolumeMinecraft0", musicVolume);

            PlayerPrefs.Save();
        }

        public bool GetRoomState()
        {
            return inRoom;
        }


        [ModdedGamemodeJoin] private void RoomJoinedModded() => inRoom = true;
        [ModdedGamemodeLeave] private void RoomLeftModded() => inRoom = false;
    }
}
