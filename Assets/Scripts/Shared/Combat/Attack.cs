using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected AttackData[] attackDatas;
    protected AttackData CurrentData;

    public virtual IEnumerator ExecuteAttack(AttackContext ctx, System.Action onFinished = null)
    {
        if (attackDatas == null || attackDatas.Length == 0) yield break;

        CurrentData = attackDatas[Random.Range(0, attackDatas.Length)];
    }
}
