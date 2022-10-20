using UnityEngine;
using GorillaLocomotion;
using Steamworks;

namespace DevMinecraftMod.Base
{
    public class MinecraftSlimeBlock : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 9;
        }

        void OnCollisionEnter(Collision col)
        {
            Rigidbody body = Player.Instance.GetComponent<Rigidbody>();
            if (col.gameObject.name == "GorillaPlayer")
            {
                if (body.velocity.y <= -2.5f)
                {
                    float oldY = Mathf.Clamp(body.velocity.y, -12, 0);
                    float rand = Random.Range(1, 3);
                    float force = Mathf.Clamp(oldY * -1 * 0.98f, 2, 15);

                    MinecraftLogger.Log(oldY.ToString() + ", " + force.ToString());

                    body.velocity *= 0.5f;

                    Player.Instance.transform.position += new Vector3(0, 0.12f, 0);

                    if (rand != 2)
                    {
                        GameObject soundObjectTemp = Instantiate(MinecraftFunction.Instance.blockBundle.LoadAsset<GameObject>("SoundExample"));
                        soundObjectTemp.transform.position = gameObject.transform.position;
                        AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

                        audioSourceTemp.PlayOneShot(MinecraftFunction.Instance.blockBundleAlt.LoadAsset<AudioClip>($"SlimeJump{Random.Range(1, 3)}"), 0.5f);

                        audioSourceTemp.transform.SetParent(MinecraftFunction.Instance.objectStorage.transform, false);
                        audioSourceTemp.gameObject.AddComponent<MinecraftAutoDelete>().DestroyTime = 3;
                    }

                    GameObject particleObjectTemp = Instantiate(MinecraftFunction.Instance.blockBundle.LoadAsset<GameObject>("BlockParticle"));
                    particleObjectTemp.transform.position = gameObject.transform.position + new Vector3(0, 0.25f, 0);

                    ParticleSystem ps = particleObjectTemp.GetComponent<ParticleSystem>(); // play station
                    ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();

                    psr.material = MinecraftFunction.Instance.blockBundleAlt.LoadAsset<Material>("SlimeBlock");
                    psr.material.mainTextureScale = new Vector2(0.2f, 0.2f);

                    ps.Play();

                    particleObjectTemp.transform.SetParent(MinecraftFunction.Instance.objectStorage.transform, false);
                    particleObjectTemp.gameObject.AddComponent<MinecraftAutoDelete>().DestroyTime = 2;

                    body.AddForce(new Vector3(0f, force * 0.5f, 0), ForceMode.VelocityChange);
                    body.AddForce(new Vector3(0f, force * 0.5f, 0), ForceMode.VelocityChange);
                }
            }
        }

    }
}
