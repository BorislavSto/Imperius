using Combat;
using EventBus;
using UnityEngine;

namespace Player
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private int maxHealth = 50;
        [SerializeField] private int maxMana = 50;
        [SerializeField] private int manaRechargeAmount = 1;
        [SerializeField] private float manaRechargeRate = 1;
        public int CurrentMana { get; private set; }

        private float manaTimer;

        protected override void Start()
        {
            base.Start();
            CurrentMana = maxMana;
            EventBus<PlayerDataEvent>.Raise(new PlayerDataEvent(CreatePlayerGameplayData()));
        }

        private void Update()
        {
            RechargeMana();
        }

        protected override void HealthOnDamaged(float obj)
        {
            EventBus<PlayerDataEvent>.Raise(new PlayerDataEvent(CreatePlayerGameplayData()));
        }

        protected override void HealthOnDeath()
        {
        }

        public bool UseMana(int manaUsed)
        {
            if ((CurrentMana - manaUsed) < 0)
                return false;

            CurrentMana -= manaUsed;
            EventBus<PlayerDataEvent>.Raise(new PlayerDataEvent(CreatePlayerGameplayData()));
            return true;
        }

        public void GainMana(int gain)
        {
            if ((CurrentMana + gain) > maxMana)
                CurrentMana = maxMana;
            else 
                CurrentMana += gain;

            EventBus<PlayerDataEvent>.Raise(new PlayerDataEvent(CreatePlayerGameplayData()));
        }

        private void RechargeMana()
        {
            if (CurrentMana >= maxMana) 
                return;

            manaTimer += Time.deltaTime;
            if (manaTimer >= manaRechargeRate)
            {
                CurrentMana += manaRechargeAmount;
                manaTimer = 0f;

                if (CurrentMana > maxMana)
                    CurrentMana = maxMana;
            }
        }

        private PlayerGameplayData CreatePlayerGameplayData()
        {
            return new PlayerGameplayData(health.currentHealth, CurrentMana, manaRechargeRate, manaRechargeAmount);
        }

        protected override int SetMaxHealthInHealth() => maxHealth;
    }
}
