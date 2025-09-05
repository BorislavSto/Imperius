using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class RangedAttackBehavior : IAttackBehavior
{
    private RangedAttackAsset asset;
    private RangedAttack executor;

    public RangedAttackBehavior(RangedAttackAsset asset, RangedAttack executor)
    {
        this.asset = asset;
        this.executor = executor;
    }

    public IEnumerator ExecuteAttack(AttackContext ctx, Action onComplete)
    {
        yield return new WaitForSeconds(asset.windup);

        if (asset.projectilePrefab != null && ctx.Target != null)
        {
            GameObject proj = Object.Instantiate(asset.projectilePrefab, executor.ShootOrigin.position, Quaternion.identity);

            Vector3 dir = (ctx.Target.transform.position - executor.ShootOrigin.position).normalized;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * asset.projectileSpeed;
            }

            Object.Destroy(proj, asset.projectileLifetime);
        }

        yield return new WaitForSeconds(asset.recovery);

        onComplete?.Invoke();
    }
}
