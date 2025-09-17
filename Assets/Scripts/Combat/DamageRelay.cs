using System;
using UnityEngine;

namespace Combat
{
    public class DamageRelay : MonoBehaviour
    {
        [SerializeField] private Collider damageArea;
        [SerializeField] private LayerMask terrainLayer;

        private GameObject damager;
        private LayerMask hitMask;
        private int damage;

        public event Action<Vector3, Vector3> OnDamageDealt; // hitPoint, hitNormal
        public event Action<Vector3, Vector3> OnTerrainHit; // hitPoint, hitNormal

        public void EnableDamage(AttackData data, GameObject attacker)
        {
            damager = attacker;
            damage = data.damage;
            damageArea.enabled = true;
            hitMask = data.hitMask;
        }

        public void DisableDamage()
        {
            damageArea.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            int otherLayer = 1 << other.gameObject.layer;

            // Check if it's an enemy we can damage
            if ((otherLayer & hitMask) != 0)
            {
                IDamageable dmg = other.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(new HitInfo
                    {
                        Source = damager,
                        DamageAmount = damage,
                    });

                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (transform.position - hitPoint).normalized;
                    OnDamageDealt?.Invoke(hitPoint, hitNormal);

                    DisableDamage();
                }
            }
            // Check if it's terrain/environment
            else if ((otherLayer & terrainLayer) != 0)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - hitPoint).normalized;
                OnTerrainHit?.Invoke(hitPoint, hitNormal);

                DisableDamage();
            }
        }
    }
}
