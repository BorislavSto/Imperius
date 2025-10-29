using UnityEngine;

namespace Player
{
    public interface IInputHandler
    {
        // Movement
        float MoveHorizontal { get; }
        float MoveVertical { get; }

        // Actions
        bool AttackOnePressed { get; }
        bool DashPressed { get; }
        bool InteractPressed { get; }
        bool InventoryPressed { get; }
        bool AttackTwoPressed { get; }
        bool AttackThreePressed { get; }
        bool AttackFourPressed { get; }
    }
}
