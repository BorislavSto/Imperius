using UnityEngine;

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
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
        }
        
        if (destroyOnHit) 
        {
            Destroy(gameObject);
        }
    }
}