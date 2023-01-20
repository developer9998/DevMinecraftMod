using UnityEngine;

namespace DevMinecraftMod.Base
{
    public class AutoDelete : MonoBehaviour
    {
        public float DestroyTime = 2;

        public void Update()
        {
            DestroyTime -= Time.deltaTime;
            if (DestroyTime <= 0) Destroy(gameObject);
        }
    }
}
