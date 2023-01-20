using System;
using System.IO;
using UnityEngine;
using Photon.Pun;

namespace DevMinecraftMod.Base
{
    public class Recover : MonoBehaviourPunCallbacks
    {
        public static Recover Instance { get; private set; }
        public string location;
        public string actedLocation;
        public bool doThis = true;
        public bool useNewEnding;
        public RecoverData recoverData = new RecoverData();

        internal void Start()
        {
            Instance = this;

            Invoke(nameof(CheckLocationsDelay), 1);
        }

        internal void CheckLocationsDelay()
        {
            CheckLocations(out useNewEnding);
        }

        internal void CheckLocations(out bool shouldUseNew)
        {
            location = Path.Combine(Plugin.Instance.location, "MapData.devmoddata");
            actedLocation = Path.Combine(Plugin.Instance.location, "MapData.json");
            shouldUseNew = File.Exists(actedLocation);

            if (File.Exists(location)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
            else if (File.Exists(actedLocation)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));

            string oldLocation = Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "Data");
            string loc = Path.Combine(oldLocation, "MapData.devmoddata");
            string acLoc = Path.Combine(oldLocation, "MapData.json");

            if (File.Exists(loc) && !File.Exists(location))
            {
                File.Move(loc, location);
                Directory.Delete(oldLocation, true);

                location = Path.Combine(Plugin.Instance.location, "MapData.devmoddata");
                actedLocation = Path.Combine(Plugin.Instance.location, "MapData.json");
                shouldUseNew = File.Exists(actedLocation);

                if (File.Exists(location)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
                else if (File.Exists(actedLocation)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
            }
            else if (File.Exists(acLoc) && !File.Exists(actedLocation))
            {
                File.Move(acLoc, actedLocation);
                Directory.Delete(oldLocation, true);

                location = Path.Combine(Plugin.Instance.location, "MapData.devmoddata");
                actedLocation = Path.Combine(Plugin.Instance.location, "MapData.json");
                shouldUseNew = File.Exists(actedLocation);

                if (File.Exists(location)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
                else if (File.Exists(actedLocation)) recoverData = JsonUtility.FromJson<RecoverData>(File.ReadAllText(location));
            }
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

            CheckLocations(out useNewEnding);
            string locationToUse = useNewEnding ? actedLocation : location;
            File.WriteAllText(locationToUse, JsonUtility.ToJson(recoverData));

            doThis = true;
        }

        public void LoadData()
        {
            CheckLocations(out useNewEnding);
            string locationToUse = useNewEnding ? actedLocation : location;

            if (!File.Exists(locationToUse))
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
