using Combat;
using EventBus;

namespace Player
{
    public struct PlayerAttacksChanged : IEvent
    {
        public readonly AttackData[] NewAttacks;
        
        public PlayerAttacksChanged(AttackData[] newAttacks)
        {
            NewAttacks = newAttacks;
        }
    }
    
    public struct AttackUsed : IEvent
    {
        public readonly AttackData Attack;
        public readonly int SlotIndex;

        public AttackUsed(AttackData attack, int slotIndex)
        {
            Attack = attack;
            SlotIndex = slotIndex;
        }
    }
}