using UnityEngine;

namespace DevMinecraftMod.Scripts.Building
{
    public class AutoDelete : MonoBehaviour
    {
        public float DestroyTime = 2;
        private float tempTime;

        void Start()
        {
            tempTime = Time.time + DestroyTime;
        }

        void Update()
        {
            if (Time.time >= tempTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
