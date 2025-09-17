using System;
using Enemies.Combat;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SmartPlayerDetection", story: "[Sensor] detects [Target] with memory for [MemoryDuration] seconds", category: "Action", id: "00c10d20692652f12ff35fdea0ca3aa7")]
public partial class SmartPlayerDetectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Sensor> Sensor;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPosition;
    [SerializeReference] public BlackboardVariable<bool> PlayerSpotted;
    [SerializeReference] public BlackboardVariable<float> MemoryDuration = new(3f);

    private float lastSeenTime;
    private bool hasEverSeenPlayer;

    protected override Status OnUpdate()
    {
        Transform currentTarget = Sensor.Value.GetClosestTarget("Player");

        if (currentTarget != null)
        {
            // Player is currently detected
            Target.Value = currentTarget.gameObject;
            LastKnownPosition.Value = currentTarget.position;
            lastSeenTime = Time.time;
            hasEverSeenPlayer = true;
            PlayerSpotted.Value = true;
            return Status.Success;
        }

        if (hasEverSeenPlayer && (Time.time - lastSeenTime) < MemoryDuration.Value)
        {
            // Player not currently visible, but still in "memory"
            PlayerSpotted.Value = true;
            return Status.Success;
        }

        // Memory expired or has never seen player
        PlayerSpotted.Value = false;
        Target.Value = null;
        hasEverSeenPlayer = false;
        return Status.Failure;
    }
}

