using System;
using System.Collections;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private Transform shootOrigin;

    public override IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null)
    {
        yield return base.ExecuteAttack(ctx, onFinished);
        
        ctx.FaceTarget();
        
        RangedAttackData data = CurrentData as RangedAttackData;
        
        if (data == null) throw new ArgumentNullException(nameof(data));
        
        yield return new WaitForSeconds(data.windup);

        if (data.projectilePrefab != null && ctx.Target != null)
        {
            GameObject proj = Instantiate(data.projectilePrefab, shootOrigin.position, Quaternion.identity);

            Vector3 dir = (ctx.Target.transform.position - shootOrigin.position).normalized;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * data.projectileSpeed;
            }

            Destroy(proj, data.projectileLifetime);
        }

        yield return new WaitForSeconds(data.recovery);

        onFinished?.Invoke();
    }
}
