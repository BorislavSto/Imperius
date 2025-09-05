using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected AttackAsset[] attackAssets;

    public IEnumerator ExecuteAttack(AttackContext ctx, System.Action onFinished = null)
    {
        if (Assets == null || Assets.Length == 0) yield break;
        
        AttackAsset currentAsset = Assets[Random.Range(0, Assets.Length)];
        var behavior = currentAsset.CreateBehavior(this);

        yield return behavior.ExecuteAttack(ctx, onFinished);
    }

    private AttackAsset[] Assets => attackAssets;
}
