using UnityEngine;

namespace Aunirik.BreakableMug
{
    public class BreakableMug : MonoBehaviour
    {
        static private int clipIndex = 0;

        [Header("Settings")]
        [SerializeField, Min(0.0f)]
        private float requiredForceToBreak = 2.5f;
        [SerializeField, Min(0.0f)]
        private float removeAfter = 2.0f;

        [Header("Assets")]
        [SerializeField]
        private GameObject breakableMugBrokenCarrierPrefab;

        [SerializeField]
        private AudioClip[] mugBrokenClips;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude >= requiredForceToBreak)
            {
                Break();
            }
        }

        public void Break()
        {
            gameObject.SetActive(false);

            GameObject carrier = Instantiate(breakableMugBrokenCarrierPrefab, transform.position, transform.rotation);
            AudioSource audioSource = carrier.GetComponent<AudioSource>();

            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.volume = Random.Range(0.8f, 1.0f);

            if (mugBrokenClips.Length > 0)
            {
                clipIndex %= mugBrokenClips.Length;
                AudioClip clip = mugBrokenClips[clipIndex++];
                audioSource.PlayOneShot(clip);

                if (removeAfter > 0.0f)
                {
                    Destroy(carrier, clip.length + removeAfter);
                }
            }

            Destroy(gameObject);
        }
    }
}
