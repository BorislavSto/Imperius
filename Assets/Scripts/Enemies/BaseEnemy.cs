using System.Collections.Generic;
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

public static class EnemyPatrolPointsGenerator
{
    public static List<GameObject> GeneratePatrolPoints(Transform center, float radius, int numPoints, LayerMask groundMask)
    {
        List<GameObject> points = new List<GameObject>();
        
        // have them all under one PatrolPointsGO
        GameObject container = new GameObject("PatrolPoints");
        container.transform.position = center.position;
        
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-radius, radius),
                10f,
                Random.Range(-radius, radius)
            );

            Vector3 spawnPos = center.position + randomOffset;

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f, groundMask))
            {
                spawnPos = hit.point;
            }
            else
            {
                spawnPos = center.position;
            }

            GameObject pointObj = new GameObject("PatrolPoint");
            pointObj.transform.position = spawnPos;
            pointObj.transform.parent = container.transform;
            points.Add(pointObj);
        }

        return points;
    }
}