using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace DevMinecraftMod.Base
{
    public class MinecraftColourSlotButton : MonoBehaviour
    {
        public int slot;

        public float debounceTime = 0.25f * 1.25f;

        public float touchTime;
        public bool eq;

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

            MinecraftFunction.Instance.ClearColourSlots();

            MinecraftFunction.Instance.currentColourMode = slot;

            if (eq)
                MinecraftFunction.Instance.itemIndicator.transform.Find("CurrentColourText").GetComponent<Text>().text = "Slot: " + slot;

            if (eq)
                MinecraftFunction.Instance.SetEquipSlot(MinecraftFunction.Instance.currentColourMode, true);
            else
                MinecraftFunction.Instance.SetGetSlot(MinecraftFunction.Instance.currentColourMode, true);

        }
    }
}
