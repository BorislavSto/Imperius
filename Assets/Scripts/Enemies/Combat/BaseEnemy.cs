using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

namespace Enemies.Combat
{
    public class BaseEnemy : Entity
    {
        [SerializeField] private BehaviorGraphAgent enemyGraph;
        [SerializeField] private BaseEnemyData enemyData;

        private Health enemyHealth;

        protected override void Start()
        {
            SetupBehaviorGraph();
            base.Start();
            Sensor sensor = GetComponent<Sensor>();
            sensor.SetSensorRadius(enemyData.VisionRange);
        }

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
            base.InitializeEntity();
        }

        protected override int GetMaxHealth() => enemyData.Health;
    }
}
