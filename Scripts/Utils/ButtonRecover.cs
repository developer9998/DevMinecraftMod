using UnityEngine;
using Valve.VR;

namespace DevMinecraftMod.Base
{
    public class ButtonRecover : MonoBehaviour
    {
        public float debounceTime = 0.75f;

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
            GorillaTagger.Instance.offlineVRRig.tagSound.PlayOneShot(MinecraftMod.Instance.clip, 0.75f);

            if (eq)
                Recover.Instance.LoadData();
            else
                Recover.Instance.SetData();

        }
    }
}
