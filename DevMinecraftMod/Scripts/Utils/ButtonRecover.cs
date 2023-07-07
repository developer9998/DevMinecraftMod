using DevMinecraftMod.Scripts.Building;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Utils
{
    public class ButtonRecover : MonoBehaviour
    {
        public float debounceTime = 0.75f;

        public float touchTime;
        public bool eq;

        void Start()
        {
            gameObject.layer = 18;
            GetComponent<BoxCollider>().size *= 1.4f;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!(touchTime + debounceTime < Time.time))
            {
                return;
            }

            if (GorillaLocomotion.Player.Instance.inOverlay)
                return;

            touchTime = Time.time;

            if (!(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null))
                return;

            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();

            if (component.isLeftHand)
                return;

            GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.35f, GorillaTagger.Instance.tapHapticDuration * 0.75f);
            GorillaTagger.Instance.offlineVRRig.tagSound.PlayOneShot(MinecraftMod.Instance.clip, 0.75f);

            if (eq)
                Recover.Instance.LoadData();
            else
                Recover.Instance.SetData();

        }
    }
}
