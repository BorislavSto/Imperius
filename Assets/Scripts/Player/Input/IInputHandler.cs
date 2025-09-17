using UnityEngine;

namespace Player.Input
{
    public interface IInputHandler
    {
        // Movement
        float MoveHorizontal { get; }
        float MoveVertical { get; }

        // Actions
        bool AttackPressed { get; }
        bool DashPressed { get; }
        bool InteractPressed { get; }
        bool InventoryPressed { get; }
        bool Skill1Pressed { get; }
        bool Skill2Pressed { get; }
        bool Skill3Pressed { get; }

        // Aiming character
        Vector3 GetAimDirection(Vector3 originPosition);
    }
}
