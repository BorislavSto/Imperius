using Unity.Behavior;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent enemyGraph;
    [SerializeField] private BaseEnemyData enemyData;
    
    void Start()
    {
        SetUpEnemy();
    }

    private void SetUpEnemy()
    {
        enemyGraph.BlackboardReference.SetVariableValue("WalkingSpeed", enemyData.GetWalkingSpeed());
        enemyGraph.BlackboardReference.SetVariableValue("RunningSpeed", enemyData.GetRunningSpeed());
    }
}