using GorillaLocomotion;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Building
{
    public class SlimeBlock : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 9;
        }

        async void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "GorillaPlayer")
            {
                Rigidbody body = Player.Instance.GetComponent<Rigidbody>();
                if (body.velocity.y <= -2.5f)
                {
                    float oldY = Mathf.Clamp(body.velocity.y, -25, 0);
                    float rand = Random.Range(1, 3);
                    float force = Mathf.Clamp(oldY * -1 * 0.98f, 2, 15);

                    Player.Instance.transform.position += new Vector3(0, 0.15f, 0);

                    if (body.velocity.x > 3.85f || body.velocity.x < -3.85f || body.velocity.z > 3.85f || body.velocity.z < -3.85f || body.velocity.y <= -7.5f)
                    {
                        body.AddForceAtPosition(new Vector3(0, force * 1.75f, 0), transform.position + new Vector3(0, -0.05f, 0), ForceMode.VelocityChange); // stronger force if you're going directional
                    }
                    else
                    {
                        body.AddForceAtPosition(new Vector3(0, force * 1.12f, 0), transform.position + new Vector3(0, -0.1f, 0), ForceMode.VelocityChange); // lighter force if you're going slow
                    }

                    if (rand != 2)
                    {
                        GameObject soundObjectTemp = Instantiate(await MinecraftMod.Instance.MainResourceBundle.LoadDevAsset<GameObject>("SoundExample"));
                        soundObjectTemp.transform.position = gameObject.transform.position;
                        AudioSource audioSourceTemp = soundObjectTemp.GetComponent<AudioSource>();

                        audioSourceTemp.PlayOneShot(await MinecraftMod.Instance.AltResourceBundle.LoadDevAsset<AudioClip>($"SlimeJump{Random.Range(1, 3)}"), 0.5f);

                        audioSourceTemp.transform.SetParent(MinecraftMod.Instance.objectStorage.transform, false);
                        audioSourceTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 3;
                    }

                    GameObject particleObjectTemp = Instantiate(await MinecraftMod.Instance.MainResourceBundle.LoadDevAsset<GameObject>("BlockParticle"));
                    particleObjectTemp.transform.position = gameObject.transform.position + new Vector3(0, 0.25f, 0);

                    Material mat = await MinecraftMod.Instance.AltResourceBundle.LoadDevAsset<Material>("SlimeBlock");
                    foreach (ParticleSystem partS in particleObjectTemp.transform.GetComponentsInChildren<ParticleSystem>())
                    {
                        ParticleSystemRenderer psr = partS.GetComponent<ParticleSystemRenderer>();

                        psr.material = mat;
                        psr.material.mainTextureScale = new Vector2(0.2f, 0.2f);

                        partS.Play();
                    }

                    particleObjectTemp.transform.SetParent(MinecraftMod.Instance.objectStorage.transform, false);
                    particleObjectTemp.gameObject.AddComponent<AutoDelete>().DestroyTime = 2;
                }
            }
        }

    }
}
