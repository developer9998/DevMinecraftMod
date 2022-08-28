﻿using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using HarmonyLib;
using Photon.Pun;
using GorillaLocomotion;

// programmed by dev.
// updated material data by frogrilla, thank you :)

namespace DevMinecraftMod.Base
{
    public class MinecraftFunction : MonoBehaviour
    {
        public static MinecraftFunction Instance;

        private Harmony harmony;

        private AssetBundle blockBundle;

        private GameObject objectStorage;
        private GameObject objectStorageBlock;
        private GameObject block;
        private GameObject blockAlt;
        private GameObject itemIndicator;
        private GameObject itemPickaxe;
        private GameObject itemShow;

        private Text itemText;

        private LineRenderer ln;

        private bool triggerPullled = true;
        private bool gripPulled = true;
        private bool upPulled = true;
        private bool downPulled = true;

        private bool raycastExists = false;

        public List<GameObject> minecraftBlocks = new List<GameObject>();
        private List<GameObject> minecraftBlockList = new List<GameObject>();
        private List<string> minecraftBlockListString = new List<string>();

        public int mode = 0;
        public int modeVersion = 0;

        public int currentBlock = 0;
        private readonly int maxBlocks = 17;

        public enum Blocks
        {
            Grass,
            Dirt,
            OakLog,
            OakPlanks,
            Cobblestone,
            Brick,
            Stone,
            Wool,
            OakLeaves,
            Glass,
            CraftingTable,
            Furnace,
            IronOre,
            GoldOre,
            Netherrack,
            PackedIce,
            Obsidian,
            Bookshelf
        }

        Color GetColour(float max)
        {
            float r = Mathf.Clamp(PlayerPrefs.GetFloat("redValue"), 0, max);
            float g = Mathf.Clamp(PlayerPrefs.GetFloat("greenValue"), 0, max);
            float b = Mathf.Clamp(PlayerPrefs.GetFloat("blueValue"), 0, max);
            return new Color(r, g, b, 1);
        }

        void AddBlock(AssetBundle bund, string blockName)
        {
            GameObject objec = bund.LoadAsset<GameObject>(blockName);
            minecraftBlockList.Add(objec);

            string blockNameFinal = blockName.Replace("CraftingTable", "Crafting Table").Replace("IronOre", "Iron Ore").Replace("GoldOre", "Gold Ore").Replace("PackedIce", "Packed Ice").Replace("Plank", "Oak Planks").Replace("Log", "Oak Log").Replace("Obisdian", "Obsidian").Replace("Leaves", "Oak Leaves").Replace("Brick", "Bricks");
            minecraftBlockListString.Add(blockNameFinal);
        }
        
        void Start()
        {
            Instance = this;
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DevMinecraftMod.Resources.devminecraft");
            blockBundle = AssetBundle.LoadFromStream(manifestResourceStream);

            if (harmony == null)
            {
                harmony = new Harmony("com.dev9998.gorillatag.devminecraftmod");
                harmony.PatchAll();
            }

            // Page 1
            AddBlock(blockBundle, "Grass"); // 0
            AddBlock(blockBundle, "Dirt"); // 1 
            AddBlock(blockBundle, "Log"); // 2
            AddBlock(blockBundle, "Plank"); // 3 
            AddBlock(blockBundle, "Cobblestone"); // 4
            AddBlock(blockBundle, "Brick"); // 5
            AddBlock(blockBundle, "Stone"); // 6
            AddBlock(blockBundle, "Wool"); // 7 
            AddBlock(blockBundle, "Leaves"); // 8

            // Page 2
            AddBlock(blockBundle, "Glass"); // 9
            AddBlock(blockBundle, "CraftingTable"); // 10
            AddBlock(blockBundle, "Furnace"); // 11
            AddBlock(blockBundle, "IronOre"); // 12
            AddBlock(blockBundle, "GoldOre"); // 13
            AddBlock(blockBundle, "Netherrack"); // 14
            AddBlock(blockBundle, "PackedIce"); // 15
            AddBlock(blockBundle, "Obisdian"); // 16
            AddBlock(blockBundle, "Bookshelf"); // 17

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

            Transform palm = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/").transform;
            itemIndicator = Instantiate(blockBundle.LoadAsset<GameObject>("ItemSelector"));

            itemIndicator.transform.SetParent(palm, false);
            itemIndicator.transform.localPosition = Vector3.zero;
            itemIndicator.transform.localRotation = Quaternion.identity;
            itemIndicator.transform.localScale = Vector3.one;

            itemIndicator.SetActive(false);

            Transform palm2 = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/").transform;

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

            itemPickaxe.SetActive(false);

            SetSlot(0);

            GameObject lnObject = Instantiate(blockBundle.LoadAsset<GameObject>("lineRendererExample"));
            lnObject.transform.position = Vector3.zero;
            lnObject.transform.rotation = Quaternion.identity;
            lnObject.transform.localScale = Vector3.one;

            ln = lnObject.GetComponent<LineRenderer>();
            ln.enabled = false;

        }

        void SetSlot(int slot)
        {
            Transform selections = itemIndicator.transform.Find("ItemSelections");

            for (int i = 0; i < selections.childCount; i++)
            {
                selections.GetChild(i).GetComponent<Renderer>().enabled = false;
                if (i == slot)
                    selections.GetChild(i).GetComponent<Renderer>().enabled = true;
            }

            UpdateAssets();
        }

        void UpdateAssets()
        {
            itemText.text = minecraftBlockListString[currentBlock];

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

            if (currentBlock == 7)
            {
                Renderer[] renderers = tempBlock.transform.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                    renderer.material.color = GetColour(0.9f);
            }
        }

        void Update()
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

            if (!Plugin.Instance.GetRoomState())
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

            if (Physics.Raycast(__instance.rightHandTransform.position, -__instance.rightHandTransform.up, out RaycastHit hit, 45, Player.Instance.locomotionEnabledLayers))
            {
                Vector3 newPos = hit.point;

                raycastExists = true;

                if (mode == 0)
                    ln.SetPosition(1, block.transform.position);
                else
                    ln.SetPosition(1, blockAlt.transform.position);

                ln.enabled = Plugin.Instance.lIndicatorEnabled;

                if (hit.transform.gameObject != null && hit.transform.GetComponent<MinecraftBlockLink>() != null)
                {
                    blockAlt.transform.position = hit.transform.GetComponent<MinecraftBlockLink>().minecraftObject.transform.position;
                    blockAlt.transform.rotation = hit.transform.GetComponent<MinecraftBlockLink>().minecraftObject.transform.rotation;
                }
                else
                {
                    blockAlt.transform.eulerAngles = Vector3.zero;
                    blockAlt.transform.position = new Vector3(((Mathf.RoundToInt(newPos.x) != 0) ? (Mathf.RoundToInt(newPos.x / 1f) * 1) : 2), ((Mathf.RoundToInt(newPos.y) != 0) ? (Mathf.RoundToInt(newPos.y / 1f) * 1) : 0), ((Mathf.RoundToInt(newPos.z) != 0) ? (Mathf.RoundToInt(newPos.z / 1f) * 1) : 2));
                }
                
                block.transform.position = new Vector3(((Mathf.RoundToInt(newPos.x) != 0) ? (Mathf.RoundToInt(newPos.x / 1f) * 1) : 2), ((Mathf.RoundToInt(newPos.y) != 0) ? (Mathf.RoundToInt(newPos.y / 1f) * 1) : 0), ((Mathf.RoundToInt(newPos.z) != 0) ? (Mathf.RoundToInt(newPos.z / 1f) * 1) : 2));

                if (currentBlock == 10 || currentBlock == 11)
                    block.transform.eulerAngles = new Vector3(0, ((Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y) != 0) ? (Mathf.RoundToInt(Player.Instance.bodyCollider.transform.eulerAngles.y / 90f) * 90) : 0), 0f);
                else if (currentBlock == 2)
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

            if (p2Da.y > 0.75f && !upPulled)
            {
                upPulled = true;
                currentBlock++;

                if (currentBlock >= maxBlocks + 1)
                    currentBlock = 0;

                SetSlot(currentBlock);
            }
            else if (p2Da.y < 0.45 && upPulled)
                upPulled = false;

            if (p2Da.y < -0.75f && !downPulled)
            {
                downPulled = true;
                currentBlock--;
                if (currentBlock <= -1)
                    currentBlock = maxBlocks;

                SetSlot(currentBlock);
            }
            else if (p2Da.y > -0.45 && downPulled)
                downPulled = false;

            if (p2DaC && mode == 1)
            {
                if (minecraftBlocks.Count != 0)
                {
                    for (int i = 0; i < minecraftBlocks.Count; i++)
                    {
                        GameObject tempOb = minecraftBlocks[i];
                        minecraftBlocks.Remove(minecraftBlocks[i]);
                        Destroy(tempOb);
                    }
                }
            }

            if (triggerDown && !triggerPullled)
            {
                triggerPullled = true;


                if (mode == 0)
                {

                    float headDistance = Vector3.Distance(block.transform.position, Player.Instance.headCollider.transform.position);

                    if (headDistance < 0.925f)
                        return;

                    float rightHandDistance = Vector3.Distance(block.transform.position, Player.Instance.rightHandTransform.position);

                    if (rightHandDistance < 0.65f)
                        return;

                    float leftHandDistance = Vector3.Distance(block.transform.position, Player.Instance.leftHandTransform.position);

                    if (leftHandDistance < 0.65f)
                        return;

                    switch (modeVersion)
                    {
                        case 0:
                            GameObject tempBlock = Instantiate(minecraftBlockList[currentBlock]);
                            tempBlock.transform.SetParent(objectStorageBlock.transform, false);

                            Transform blockColliders = tempBlock.transform.GetChild(0);

                            Blocks usedBlockEnum = Blocks.Grass;

                            blockColliders.GetComponent<BoxCollider>().enabled = false;

                            switch (currentBlock)
                            {
                                case 0:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 14;
                                        if (blockColliders.GetChild(i).gameObject.name == "Top")
                                        {
                                            gso.overrideIndex = 7;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 14;
                                    }
                                    usedBlockEnum = Blocks.Dirt;
                                    break;
                                case 2:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 8;
                                        if (blockColliders.GetChild(i).gameObject.name == "Top" || blockColliders.GetChild(i).gameObject.name == "Bottom")
                                        {
                                            gso.overrideIndex = 9;
                                        }
                                    }
                                    usedBlockEnum = Blocks.OakLog;
                                    break;
                                case 3:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 9;
                                    }
                                    usedBlockEnum = Blocks.OakPlanks;
                                    break;
                                case 4:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Cobblestone;
                                    break;
                                case 5:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Brick;
                                    break;
                                case 6:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Stone;
                                    break;
                                case 7:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 5;

                                        if (blockColliders.GetChild(i).GetComponent<Renderer>() != null)
                                        {
                                            Renderer renderer = blockColliders.GetChild(i).GetComponent<Renderer>();
                                            renderer.material.color = GetColour(0.9f);
                                        }
                                    }
                                    usedBlockEnum = Blocks.Wool;
                                    break;
                                case 8:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 31;
                                    }
                                    usedBlockEnum = Blocks.OakLeaves;
                                    break;

                                case 9:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 55;
                                    }
                                    usedBlockEnum = Blocks.Glass;
                                    break;

                                case 10:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 9;
                                    }
                                    usedBlockEnum = Blocks.CraftingTable;
                                    break;

                                case 11:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Furnace;
                                    break;

                                case 12:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.IronOre;
                                    break;

                                case 13:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.GoldOre;
                                    break;

                                case 14:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Netherrack;
                                    break;

                                case 15:
                                    blockColliders.GetComponent<BoxCollider>().material = Resources.Load<PhysicMaterial>("objects/forest/materials/Slippery");
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 59;
                                        blockColliders.GetChild(i).gameObject.GetComponent<BoxCollider>().material = Resources.Load<PhysicMaterial>("objects/forest/materials/Slippery");
                                    }
                                    usedBlockEnum = Blocks.PackedIce;
                                    break;

                                case 16:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 0;
                                    }
                                    usedBlockEnum = Blocks.Obsidian;
                                    break;

                                case 17:
                                    for (int i = 0; i < blockColliders.childCount; i++)
                                    {
                                        GorillaSurfaceOverride gso = blockColliders.GetChild(i).gameObject.AddComponent<GorillaSurfaceOverride>();
                                        gso.overrideIndex = 9;
                                    }
                                    usedBlockEnum = Blocks.Bookshelf;
                                    break;
                            }

                            tempBlock.transform.localScale = Vector3.one;
                            tempBlock.transform.localPosition = block.transform.localPosition;
                            tempBlock.transform.localRotation = block.transform.localRotation;

                            BoxCollider[] boxColliders = tempBlock.transform.GetComponentsInChildren<BoxCollider>();

                            foreach (BoxCollider bx in boxColliders)
                            {
                                bx.gameObject.layer = 0;
                                bx.enabled = true;
                                bx.gameObject.AddComponent<MinecraftBlockLink>().minecraftObject = tempBlock;
                                bx.gameObject.GetComponent<MinecraftBlockLink>().block = usedBlockEnum;
                            }

                            minecraftBlocks.Add(tempBlock);

                            break;
                    }
                }
                else if (mode == 1)
                {
                    switch (modeVersion)
                    {
                        case 0:
                            if (hit.transform.gameObject != null )
                            {
                                if (hit.transform.GetComponent<MinecraftBlockLink>() != null)
                                {
                                    minecraftBlocks.Remove(hit.transform.GetComponent<MinecraftBlockLink>().minecraftObject);
                                    Destroy(hit.transform.GetComponent<MinecraftBlockLink>().minecraftObject);
                                }
                            }
                            break;
                    }
                }
            }
            else if (!triggerDown && triggerPullled)
                triggerPullled = false;

            if (gripDown && !gripPulled)
            {
                gripPulled = true;

                switch (mode)
                {
                    case 0:
                        mode = 1;
                        break;
                    case 1:
                        mode = 0;
                        break;
                }
            }
            else if (!gripDown && gripPulled)
                gripPulled = false;
        }

        [HarmonyPatch(typeof(PlayerPrefs))]
        [HarmonyPatch("SetFloat", MethodType.Normal)]
        public class OnColourChanged
        {
            private static void Postfix(string key, string value)
            {
                bool colourSwap = false;
                if (key == "redValue")
                    colourSwap = true;

                if (key == "greenValue")
                    colourSwap = true;

                if (key == "blueValue")
                    colourSwap = true;

                if (colourSwap)
                    Plugin.Instance.mf.UpdateAssets();
            }
        }
    }
}
