using System.Collections.Generic;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Building
{
    [System.Serializable]
    public class RecoverData
    {
        public List<GameObject> blocks = new List<GameObject>();
        public List<int> blockIndexs = new List<int>();
        public List<Vector3> positions = new List<Vector3>();
        public List<Vector3> eulerAngles = new List<Vector3>();
        public List<Color> colors = new List<Color>();
    }
}
