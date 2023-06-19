using DevMinecraftMod.Scripts.Utils;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Building
{
    public class Recover : MonoBehaviourPunCallbacks
    {
        public static Recover Instance { get; private set; }

        public string location;

        public RecoverData recoverData = new RecoverData();

        public bool doThis = true;

        void Start()
        {
            Instance = this;

            location = Plugin.Instance.location + $"\\MapData.devmoddata";

            if (File.Exists(location))
                recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
        }

        public void SetData()
        {
            MinecraftLogger.Log($"attempting to save mapdata");
            if (MinecraftMod.Instance.blockLinks.Count == 0)
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
            float maxBlocks = MinecraftMod.Instance.minecraftBlocks.Count;
            for (int i = 0; i < MinecraftMod.Instance.blockLinks.Count; i++)
            {
                MinecraftBlock link = MinecraftMod.Instance.blockLinks[i];

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

            if (MinecraftMod.Instance.minecraftBlocks.Count != 0)
            {
                while (MinecraftMod.Instance.minecraftBlocks.Count != 0)
                    MinecraftMod.Instance.DestroyAllBlocks();
            }

            try
            {
                for (int i = 0; i < recoverData.blockIndexs.Count; i++)
                    MinecraftMod.Instance.CreateBlock(recoverData.blockIndexs[i], recoverData.positions[i], recoverData.eulerAngles[i], recoverData.colors[i]);
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
