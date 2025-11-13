using UnityEngine;

namespace Combat
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayHitEffect(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
            Destroy(gameObject, clip.length);
        }
    }
}