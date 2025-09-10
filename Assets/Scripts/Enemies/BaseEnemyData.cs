using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "ScriptableObjects/BaseEnemyData", order = 1)]
public class BaseEnemyData : ScriptableObject
{
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private int health = 30;
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 4;
    [SerializeField] private float visionRange = 5;
    [SerializeField] private float minAttackRange = 5;

    public string EnemyName => enemyName;
    public int Health => health;
    public float WalkingSpeed => walkingSpeed;
    public float RunningSpeed => runningSpeed;
    public float VisionRange => visionRange;
    public float MinAttackRange => minAttackRange;
}