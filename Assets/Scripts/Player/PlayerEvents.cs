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
        public readonly int SlotIndex;

        public AttackUsed(int slotIndex)
        {
            SlotIndex = slotIndex;
        }
    }

    public struct PlayerDataEvent : IEvent
    {
        public readonly PlayerGameplayData PlayerData;

        public PlayerDataEvent(PlayerGameplayData playerData)
        {
            PlayerData  = playerData;
        }
    }

    public struct PlayerGameplayData
    {
        public readonly int Health;
        public readonly int Mana;
        public readonly float ManaRechargeRate;
        public readonly int ManaRechargeAmount;

        public PlayerGameplayData(int health, int mana, float rechargeRate, int rechargeAmount)
        {
            Health = health;
            Mana = mana;
            ManaRechargeRate = rechargeRate;
            ManaRechargeAmount = rechargeAmount;
        }
    }
}