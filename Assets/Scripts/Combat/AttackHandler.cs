using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum AttackerType
{
    Melee,
    Ranged,
    AllTypes,
}

public class AttackHandler : MonoBehaviour
{
    [SerializeField] protected AttackData[] attackDatas;
    
    [SerializeField] protected AttackerType attackerType = AttackerType.Melee;
    private bool IsMelee => attackerType == AttackerType.Melee;
    private bool IsRanged => attackerType == AttackerType.Ranged;
    private bool IsAllTypes => attackerType == AttackerType.AllTypes;
    
    [Header("Melee Exclusive config")] 
    [ShowIf(EConditionOperator.Or, "IsMelee", "IsAllTypes")]
    [SerializeField] private Collider damageArea;
    [ShowIf(EConditionOperator.Or, "IsMelee", "IsAllTypes")]
    [SerializeField] private DamageRelay dmgRelay;
    public Collider DamageArea => damageArea;
    public DamageRelay DmgRelay => dmgRelay;
    
    [Header("Ranged Exclusive config")]
    [ShowIf(EConditionOperator.Or, "IsRanged", "IsAllTypes")]
    [SerializeField] private Transform shootOrigin;
    public Transform ShootOrigin => shootOrigin;
    
    protected Dictionary<AttackData, float> cooldownTimers = new();

    private void Awake()
    {
        foreach (var data in attackDatas)
            cooldownTimers[data] = 0f;
    }

    protected virtual void Update()
    {
        var keys = new List<AttackData>(cooldownTimers.Keys);
        foreach (var key in keys)
        {
            if (cooldownTimers[key] > 0f)
                cooldownTimers[key] -= Time.deltaTime;
        }
    }
    
    protected void Attack(AttackData data, AttackContext ctx, System.Action onFinish = null)
    {
        if (!cooldownTimers.ContainsKey(data)) return;

        if (cooldownTimers[data] > 0f)
        {
            Debug.Log($"{data.name} is on cooldown for {cooldownTimers[data]:F1}s");
            return;
        }

        var attack = data.CreateAttack(this);
        if (attack == null) return;

        StartCoroutine(attack.ExecuteAttack(ctx, onFinish));

        cooldownTimers[data] = data.cooldown;
    }

    public virtual void Attack(AttackContext ctx, System.Action onFinish = null)
    {
        foreach (AttackData data in attackDatas)
        {
            if (cooldownTimers[data] <= 0f)
            {
                Attack(data, ctx, onFinish);
                return;
            }
        }

        Debug.Log("No attacks ready!");
    }
}