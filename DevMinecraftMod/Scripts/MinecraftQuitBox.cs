using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace DevMinecraftMod.Scripts
{
    public class MinecraftQuitBox : GorillaTriggerBox
    {
        void Start()
        {
            gameObject.layer = 15;
        }

        public override void OnBoxTriggered()
        {
            Vector3 target = new Vector3(-64f, 12.534f, -83.014f);

            Traverse.Create(Player.Instance).Field("lastPosition").SetValue(target);
            Traverse.Create(Player.Instance).Field("lastLeftHandPosition").SetValue(target);
            Traverse.Create(Player.Instance).Field("lastRightHandPosition").SetValue(target);
            Traverse.Create(Player.Instance).Field("lastHeadPosition").SetValue(target);

            Player.Instance.leftControllerTransform.position = target;
            Player.Instance.rightControllerTransform.position = target;
            Player.Instance.bodyCollider.attachedRigidbody.transform.position = target;

            Player.Instance.GetComponent<Rigidbody>().position = target;
            Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
