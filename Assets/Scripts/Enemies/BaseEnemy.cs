using Unity.Behavior;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent enemyGraph;
    [SerializeField] private BaseEnemyData enemyData;

    private Health enemyHealth;
    
    void Start()
    {
        SetUpEnemy();
    }

    private void SetUpEnemy()
    {
        enemyGraph.BlackboardReference.SetVariableValue("WalkingSpeed", enemyData.WalkingSpeed);
        enemyGraph.BlackboardReference.SetVariableValue("RunningSpeed", enemyData.RunningSpeed);
        
        // this sets up all the components so the components can be used by anything else not just enemies

        enemyHealth = GetComponent<Health>(); 
        enemyHealth.Init(enemyData.Health);
        
        // 
    }
}