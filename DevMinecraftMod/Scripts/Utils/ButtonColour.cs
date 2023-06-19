using DevMinecraftMod.Scripts.Building;
using UnityEngine;
using UnityEngine.UI;

namespace DevMinecraftMod.Scripts.Utils
{
    public class ButtonColour : MonoBehaviour
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

            if (GorillaTagger.Instance.myVRRig != null)
            {
                if (!GorillaTagger.Instance.myVRRig.mainSkin.enabled)
                {
                    return;
                }
            }

            touchTime = Time.time;

            if (!(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null))
                return;

            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();

            if (component.isLeftHand)
                return;

            GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.35f, GorillaTagger.Instance.tapHapticDuration * 0.75f);
            GorillaTagger.Instance.offlineVRRig.tagSound.PlayOneShot(MinecraftMod.Instance.clip, 0.75f);

            MinecraftMod.Instance.ClearColourSlots();

            MinecraftMod.Instance.currentColourMode = slot;

            if (eq)
                MinecraftMod.Instance.itemIndicator.transform.Find("CurrentColourText").GetComponent<Text>().text = "Slot: " + slot;

            if (eq)
                MinecraftMod.Instance.SetEquipSlot(MinecraftMod.Instance.currentColourMode, true);
            else
                MinecraftMod.Instance.SetGetSlot(MinecraftMod.Instance.currentColourMode, true);

        }
    }
}
