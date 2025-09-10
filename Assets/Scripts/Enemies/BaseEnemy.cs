using Unity.Behavior;
using UnityEngine;

public class BaseEnemy : Entity
{
    [SerializeField] private BehaviorGraphAgent enemyGraph;
    [SerializeField] private BaseEnemyData enemyData;

    private Health enemyHealth;
    
    protected override void Start()
    {
        SetupBehaviorGraph();
        base.Start();
    }

    private void SetupBehaviorGraph()
    {
        enemyGraph.BlackboardReference.SetVariableValue("WalkingSpeed", enemyData.WalkingSpeed);
        enemyGraph.BlackboardReference.SetVariableValue("RunningSpeed", enemyData.RunningSpeed);
        enemyGraph.BlackboardReference.SetVariableValue("AttackRange", enemyData.MinAttackRange);
    }

    public void SetAttackCooldown(float cooldown)
    {
        enemyGraph.BlackboardReference.SetVariableValue("AttackCooldown", cooldown);
    }
    
    protected override void InitializeEntity()
    {
        // this sets up all the components so the components can be used by anything else not just enemies
        base.InitializeEntity();
    }
    
    protected override int GetMaxHealth() => enemyData.Health;
}