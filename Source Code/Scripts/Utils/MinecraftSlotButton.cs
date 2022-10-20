using Photon.Pun;
using UnityEngine;
using Valve.VR;

namespace DevMinecraftMod.Base
{
    public class MinecraftSlotButton : MonoBehaviour
    {
        public int slot;

        public float debounceTime = 0.25f * 1.25f;

        public float touchTime;
        public bool eq = true;

        void Start()
        {
            gameObject.layer = 18;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!(touchTime + debounceTime < Time.time))
            {
                return;
            }

            if (OpenVR.Overlay != null && OpenVR.Overlay.IsDashboardVisible())
                return;

            touchTime = Time.time;

            if (!(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null))
                return;

            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();

            if (component.isLeftHand)
                return;

            GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.35f, GorillaTagger.Instance.tapHapticDuration * 0.75f);
            GorillaTagger.Instance.offlineVRRig.tagSound.PlayOneShot(MinecraftFunction.Instance.clip, 0.75f);

            MinecraftFunction.Instance.currentBlock = slot;
            MinecraftFunction.Instance.SetSlot(MinecraftFunction.Instance.currentBlock);

        }
    }
}
