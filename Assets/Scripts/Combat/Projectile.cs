using System.Collections;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private HitEffect hitEffectPrefab;
        [SerializeField] private bool destroyOnHit = true;
        [SerializeField] private float gravity = 5f;

        private AudioClip hitSound;
        private Rigidbody rb;
        private IEnumerator destroyCoroutine;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            DamageRelay relay = GetComponent<DamageRelay>();
            if (relay != null)
            {
                relay.OnDamageDealt += HandleHitEffects;
                relay.OnTerrainHit += HandleHitEffects;
            }
        }

        private void Update()
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

        private void HandleHitEffects(Vector3 hitPoint, Vector3 hitNormal)
        {
            if (hitEffectPrefab != null)
            {
                Quaternion rotation;

                if (hitNormal == Vector3.zero)
                    rotation = Quaternion.identity;
                else
                    rotation = Quaternion.LookRotation(hitNormal);

                HitEffect hitEffectObject = Instantiate(hitEffectPrefab, hitPoint, rotation);
                
                if (hitSound != null) 
                    hitEffectObject.PlayHitEffect(hitSound);
            }

            if (destroyOnHit)
                Destroy(gameObject);
        }

        public void DestroyAfterLifetime(float lifetime)
        {
            destroyCoroutine = DestroyAfterLifetimeCo(lifetime);
            StartCoroutine(destroyCoroutine);
        }

        private IEnumerator DestroyAfterLifetimeCo(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            
            if (hitEffectPrefab != null)
            {
                Quaternion rotation = Quaternion.identity;
                HitEffect hitEffectObject = Instantiate(hitEffectPrefab, gameObject.transform.position, rotation);
                
                if (hitSound != null) 
                    hitEffectObject.PlayHitEffect(hitSound);
            }
            
            Destroy(gameObject);        
        }

        public void SetSound(AudioClip hitSFX)
        {
            hitSound = hitSFX;
        }

        private void OnDestroy()
        {
            if (destroyCoroutine != null)
                StopCoroutine(destroyCoroutine);
        }
    }
}
