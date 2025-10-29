using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private bool destroyOnHit = true;

        void Start()
        {
            DamageRelay relay = GetComponent<DamageRelay>();
            if (relay != null)
            {
                relay.OnDamageDealt += HandleHitEffects;
                relay.OnTerrainHit += HandleHitEffects;
            }
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

                Instantiate(hitEffectPrefab, hitPoint, rotation);
            }
            
            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}
