using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "ScriptableObjects/BaseEnemyData", order = 1)]
public class BaseEnemyData : ScriptableObject
{
    public string EnemyName { get; private set; } = "Enemy";
    public float Health { get; private set; } = 30;
    public float WalkingSpeed { get; private set; } = 2;
    public float RunningSpeed { get; private set; } = 4;
    public float VisionRange { get; private set; } = 5;
}