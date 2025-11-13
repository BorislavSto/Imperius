using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Patrol Points", story: "[Patrol] points are null", category: "Conditions", id: "66d3554b66c8ce187783768016cbdd43")]
public partial class CheckPatrolPointsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Patrol;

    public override bool IsTrue()
    {
        if (Patrol.Value == null || Patrol.Value.Count == 0)
            return true;

        if (Patrol.Value.Count != 0)
        {
            foreach (GameObject patrolPoint in Patrol.Value)
            {
                // Have to use the Unity == check to check if the GameObject is null, if C# "is" is used it will check
                // if there is no reference at all, so if "is" is used it will return false if == is used it will return true
                if (patrolPoint == null) 
                    return true;
            }
        }

        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
