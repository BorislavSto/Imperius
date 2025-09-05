using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enemy Attacks Player", story: "[Agent] Attacks [Target] [Animator]", category: "Action", id: "7bbdd14cc1f9702b68f98db1bedd5459")]
public partial class EnemyAttacksPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    private bool attackFinished;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
            return Status.Failure;

        var attack = Agent.Value.GetComponent<Attack>();
        if (attack == null)
            return Status.Failure;

        var ctx = new AttackContext
        {
            Animator = Animator.Value,
            Audio = Agent.Value.GetComponent<AudioSource>(),
            Attacker = Agent.Value.transform,
            Target = Target.Value
        };

        Agent.Value.GetComponent<Attack>().StartCoroutine(attack.ExecuteAttack(ctx, () => attackFinished = true));

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return attackFinished ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

