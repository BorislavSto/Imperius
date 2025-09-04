using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "ScriptableObjects/BaseEnemyData", order = 1)]
public class BaseEnemyData : ScriptableObject
{
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private float health = 30;
    [SerializeField] private float damage;
    [SerializeField] public float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 4;
    [SerializeField] private float visionRange;
    
    public float GetWalkingSpeed() => walkingSpeed;
    public float GetRunningSpeed() => runningSpeed;
}