using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Reflection;
using DevMinecraftMod.Base;
using UnityEngine.InputSystem;

namespace DevMinecraftMod.Base
{
    public class MinecraftRecoverFunction : MonoBehaviourPunCallbacks
    {
        public static MinecraftRecoverFunction Instance { get; private set; }

        public string location;

        public MinecraftRecoverData recoverData = new MinecraftRecoverData();

        public bool doThis = true;

        void Start()
        {
            Instance = this;

            location = Plugin.Instance.location + $"\\MapData.json";

            if (File.Exists(location))
                recoverData = JsonUtility.FromJson<MinecraftRecoverData>(File.ReadAllText(location));
        }

        public void SetData()
        {
            MinecraftLogger.Log($"attempting to save mapdata");
            if (MinecraftFunction.Instance.blockLinks.Count == 0)
            {
                doThis = true;
                return;
            }

            recoverData.positions.Clear();
            recoverData.eulerAngles.Clear();
            recoverData.blocks.Clear();
            recoverData.blockIndexs.Clear();
            recoverData.colors.Clear();
            float currentIndex = 0;
            float maxBlocks = MinecraftFunction.Instance.minecraftBlocks.Count;
            for (int i = 0; i < MinecraftFunction.Instance.blockLinks.Count; i++)
            {
                MinecraftBlockLink link = MinecraftFunction.Instance.blockLinks[i];
                
                if (!recoverData.blocks.Contains(link.minecraftObject))
                {
                    currentIndex++;

                    recoverData.blocks.Add(link.minecraftObject);
                    recoverData.blockIndexs.Add(link.blockIndex);
                    recoverData.positions.Add(link.minecraftObject.transform.position);
                    recoverData.eulerAngles.Add(link.minecraftObject.transform.eulerAngles);
                    recoverData.colors.Add(link.blockColour);
                    MinecraftLogger.Log($"added block {currentIndex}/{maxBlocks} at {link.minecraftObject.transform.position.x} {link.minecraftObject.transform.position.y} {link.minecraftObject.transform.position.z}");
                } 
            }

            Plugin.Instance.SetSettings();

            MinecraftLogger.Log($"successfully saved mapdata!");

            File.WriteAllText(location, JsonUtility.ToJson(recoverData));

            doThis = true;
        }

        public void LoadData()
        {
            if (!File.Exists(location))
            {
                doThis = true;
                return;
            }

            if (recoverData.blockIndexs.Count == 0)
            {
                doThis = true;
                return;
            }

            if (MinecraftFunction.Instance.minecraftBlocks.Count != 0)
            {
                while (MinecraftFunction.Instance.minecraftBlocks.Count != 0)
                    MinecraftFunction.Instance.DestroyAllBlocks();
            }

            try
            {
                for (int i = 0; i < recoverData.blockIndexs.Count; i++)
                    MinecraftFunction.Instance.CreateBlock(recoverData.blockIndexs[i], recoverData.positions[i], recoverData.eulerAngles[i], recoverData.colors[i]);
            }
            catch (Exception e)
            {
                MinecraftLogger.LogError($"{e}");
                doThis = true;
                return;
            }

            Plugin.Instance.GetSettings();
            MinecraftLogger.Log("placed block data!");

            doThis = true;
        }
    }
}
