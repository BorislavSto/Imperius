using System;
using Combat;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enemy Attacks Player", story: "[Agent] will [Attack] [Target] ,animating with [EnemyAnimator]", category: "Action", id: "7bbdd14cc1f9702b68f98db1bedd5459")]
public partial class EnemyAttacksPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<AttackHandler> Attack;
    [SerializeReference] public BlackboardVariable<Animator> EnemyAnimator;
    private bool attackFinished;

    protected override Status OnStart()
    {
        if (Agent.Value is null || Target.Value is null || Attack.Value is null)
            return Status.Failure;

        var ctx = new AttackContext
        {
            Animator = EnemyAnimator.Value,
            Audio = Agent.Value.GetComponent<AudioSource>(),
            Attacker = Agent.Value,
            TargetLocation = Target.Value.transform.position,
        };
        
        Attack.Value.Attack(ctx, () => attackFinished = true);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return attackFinished ? Status.Success : Status.Running;
    }
}

