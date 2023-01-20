﻿using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace DevMinecraftMod.Music
{
    public class MinecraftMusic : MonoBehaviour
    {
        public static MinecraftMusic Instance;

        private GameObject buttonObject;

        private Material buttonUnpressed;
        private Material buttonPressed;

        private GameObject buttonTextObject;
        private Text buttonText;

        public bool isActivated = false;

        public List<AudioClip> albumTracks = new List<AudioClip>();

        private bool isPlaying = false;
        private int song = 0;
        private int plays = 0;
        private float sThing;

        private bool alreadyPlayed;
        private int lastPlayed = 0;

        private bool pauseEvent = false;

        private AudioSource soundThing;

        public void SwitchButtonMode()
        {
            isActivated = !isActivated;

            int isActivatedInt = isActivated ? 1 : 0;
            PlayerPrefs.SetInt("MinecraftModMusicMute", isActivatedInt);

            buttonObject.GetComponent<Renderer>().material = isActivated ? buttonPressed : buttonUnpressed;
            buttonText.text = $"{(isActivated ? "UNMUTE" : "MUTE")}";
        }

        public void SetButtonMode(bool setTo)
        {
            isActivated = setTo;

            int isActivatedInt = isActivated ? 1 : 0;
            PlayerPrefs.SetInt("MinecraftModMusicMute", isActivatedInt);

            buttonObject.GetComponent<Renderer>().material = isActivated ? buttonPressed : buttonUnpressed;
            buttonText.text = $"{(isActivated ? "UNMUTE" : "MUTE")}";
        }

        private GameObject CreateButton(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject tempButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BoxCollider tempButtonCollider = tempButton.GetComponent<BoxCollider>();
            Renderer tempButtonRenderer = tempButton.GetComponent<Renderer>();

            tempButtonCollider.isTrigger = true;
            tempButtonRenderer.material = isActivated ? buttonPressed : buttonUnpressed;

            tempButton.layer = 18;

            tempButton.transform.SetParent(null, false);

            tempButton.transform.position = position;
            tempButton.transform.rotation = rotation;
            tempButton.transform.localScale = scale;

            return tempButton;
        }

        void Start()
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DevMinecraftMod.Resources.devminecraftmusic");
            AssetBundle remoteAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("city"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("clark"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("dry"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("hag"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("key"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("mice"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("minecraft"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("subwoofer"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("sweden"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("venus"));
            albumTracks.Add(remoteAssetBundle.LoadAsset<AudioClip>("wet"));

            GorillaPressableButton baseButton = null;
            foreach (var mButton in Resources.FindObjectsOfTypeAll<ModeSelectButton>())
            {
                if (mButton.gameObject.name is "Casual")
                {
                    baseButton = mButton;
                    break;
                }
            }

            buttonUnpressed = baseButton.unpressedMaterial ?? null;
            buttonPressed = baseButton.pressedMaterial ?? null;

            int tempInt = PlayerPrefs.GetInt("MinecraftModMusicMute", 0);

            isActivated = tempInt == 1;

            Instance = this;

            Vector3 buttonPosition = new Vector3(-69.49554f, 3.393653f, -70.40147f);
            Quaternion buttonRotation = Quaternion.Euler(35.362f, -5.194f, 0);
            Vector3 buttonScale = new Vector3(0.08537038f, 0.08537036f, 0.1013f);

            buttonObject = CreateButton(buttonPosition, buttonRotation, buttonScale);

            buttonTextObject = new GameObject();
            buttonTextObject.transform.SetParent(buttonObject.transform, false);

            buttonTextObject.transform.localPosition = new Vector3(-0.048987f, -0.001668297f, -0.5086516f);
            buttonTextObject.transform.localRotation = Quaternion.Euler(-180, 180, -180);
            buttonTextObject.transform.localScale = new Vector3(0.02167028f, 0.02167028f, 0.9871666f);

            Font utopiumFontTemp = null;
            foreach (var fC in Resources.FindObjectsOfTypeAll<Text>())
            {
                if (fC.font.name is "Utopium")
                {
                    utopiumFontTemp = fC.font;
                    break;
                }
            }

            buttonTextObject.AddComponent<Canvas>();
            buttonText = buttonTextObject.AddComponent<Text>();
            buttonText.font = utopiumFontTemp;
            buttonText.supportRichText = true;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = new Color(0.1960784f, 0.1960784f, 0.1960784f);
            buttonText.text = $"{(isActivated ? "UNMUTE" : "MUTE")}";

            buttonObject.AddComponent<MinecraftMuteButton>();

            remoteAssetBundle.Unload(false);

            GameObject tObject = new GameObject();
            tObject.transform.position = Vector3.zero;
            tObject.transform.rotation = Quaternion.identity;
            tObject.transform.localScale = Vector3.one;

            soundThing = tObject.AddComponent<AudioSource>();
            soundThing.playOnAwake = false;
            soundThing.dopplerLevel = 0;
            soundThing.rolloffMode = AudioRolloffMode.Linear;
            soundThing.maxDistance = 5000;
            soundThing.minDistance = 4999;
            soundThing.volume = 0.08f;
        }

        void Update()
        {
            if (buttonObject == null || buttonText == null || albumTracks.Count == 0)
                return;

            if (!PhotonNetwork.InRoom)
            {
                buttonObject.SetActive(false);

                if (soundThing.isPlaying)
                {
                    soundThing.Pause();
                    pauseEvent = true;
                }

                if (!pauseEvent)
                    sThing = Time.time + 45;

                return;
            }

            if (!Plugin.Instance.InRoom)
            {
                buttonObject.SetActive(false);

                if (soundThing.isPlaying)
                {
                    soundThing.Pause();
                    pauseEvent = true;
                }

                if (!pauseEvent)
                    sThing = Time.time + 45;

                return;
            }

            if (isActivated)
                soundThing.volume = 0;
            else
                soundThing.volume = Plugin.Instance.musicVolume;

            buttonObject.SetActive(true);

            if (!isPlaying)
            {
                if (Time.time >= sThing)
                {
                    isPlaying = true;
                    song = Random.Range(0, albumTracks.Count - 1);

                    if (alreadyPlayed && lastPlayed == song)
                    {
                        while(lastPlayed == song)
                        {
                            song = Random.Range(0, albumTracks.Count - 1);
                        }
                    }

                    if (alreadyPlayed && lastPlayed != song)
                    {
                        //Debug.LogError("playing a new song");
                    }
                    else
                    {
                        //Debug.LogError("playing the first song");
                    }

                    soundThing.clip = albumTracks[song];
                    sThing = Time.time + albumTracks[song].length + 60;

                    pauseEvent = false;

                    alreadyPlayed = true;
                    lastPlayed = song;
                }
            }
            else
            {
                if (!soundThing.isPlaying)
                {
                    if (plays == 1 && !pauseEvent)
                    {
                        isPlaying = false;
                        pauseEvent = false;
                        soundThing.Stop();
                        sThing = Time.time + 45;
                        plays = 0;
                        //Debug.LogError("switching to new song");
                    }
                    else
                    {
                        if (!pauseEvent)
                        {
                            //Debug.LogError("playing current song");
                            plays++;
                        }
                        else
                        {
                            //Debug.LogError("playing current song (pause event)");
                        }
                        soundThing.Play();
                        pauseEvent = false;
                    }
                }
            }
        }

        public class MinecraftMuteButton : GorillaPressableButton
        {
            public override void ButtonActivation()
            {
                base.ButtonActivation();
                Instance.SwitchButtonMode();
            }
        }
    }
}
