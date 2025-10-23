using System.Collections.Generic;
using Combat;
using Unity.Behavior;
using UnityEngine;

namespace Enemies.Combat
{
    public class BaseEnemy : Entity
    {
        [SerializeField] private BehaviorGraphAgent enemyGraph;
        [SerializeField] private BaseEnemyData enemyData;
        [SerializeField] private Sensor sensor; // reference to be set in inspector so as to not use GetComponentInChildren
        
        protected override void Start()
        {
            SetupBehaviorGraph();
            
            base.Start();
            
            sensor.SetSensorRadius(enemyData.VisionRange);
        }

        // TODO: COULD set up animator, and all other components here too
        private void SetupBehaviorGraph()
        {
            enemyGraph.BlackboardReference.SetVariableValue("WalkingSpeed", enemyData.WalkingSpeed);
            enemyGraph.BlackboardReference.SetVariableValue("RunningSpeed", enemyData.RunningSpeed);
            enemyGraph.BlackboardReference.SetVariableValue("AttackRange", enemyData.MinAttackRange);

            List<GameObject> patrolPoints = EnemyPatrolPointsGenerator.GeneratePatrolPoints(
                transform,
                10f,
                5,
                LayerMask.GetMask("Default")
            );

            enemyGraph.BlackboardReference.SetVariableValue("PatrolPoints", patrolPoints);
        }

        protected override void InitializeEntity()
        {
            // this sets up all the components so the components can be used by anything else not just enemies
            Debug.Log("BaseEnemy InitializeEntity");
            base.InitializeEntity();
        }

        protected override void HealthOnDamaged(float damageAmount)
        {
            Debug.Log("BaseEnemy HealthOnDamaged");
        }

        protected override void HealthOnDeath()
        {
            Debug.Log("BaseEnemy HealthOnDeath");
            SetIsDead();
        }

        protected override int GetMaxHealth() => enemyData.Health;
    }
}
