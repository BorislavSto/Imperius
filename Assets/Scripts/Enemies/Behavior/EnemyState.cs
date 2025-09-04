using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
    Idle,
	Patrolling,
	Chase,
	Attacking,
	Stunned,
	Dead,
}
