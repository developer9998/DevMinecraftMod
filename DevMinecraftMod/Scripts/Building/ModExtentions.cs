using System.Threading.Tasks;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Building
{
    public static class ModExtentions
    {
        public static async Task<T> LoadDevAsset<T>(this AssetBundle bundle, string assetName) where T : Object
        {
            var outcome = await MinecraftMod.Instance.LoadAsset<T>(assetName, bundle);
            return outcome;
        }
    }
}
