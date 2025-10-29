using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Combat
{
    public enum AttackerType
    {
        Melee,
        Ranged,
        AllTypes,
    }

    public class AttackHandler : MonoBehaviour
    {
        [SerializeField] protected AttackerType attackerType = AttackerType.Melee;
        private bool IsMelee => attackerType == AttackerType.Melee;
        private bool IsRanged => attackerType == AttackerType.Ranged;
        private bool IsAllTypes => attackerType == AttackerType.AllTypes;

        [Header("Melee Exclusive config")] 
        [ShowIf(EConditionOperator.Or, "IsMelee", "IsAllTypes")] 
        [SerializeField] private Collider damageArea;
        [ShowIf(EConditionOperator.Or, "IsMelee", "IsAllTypes")] 
        [SerializeField] private DamageRelay dmgRelay;
        public Collider DamageArea => damageArea;
        public DamageRelay DmgRelay => dmgRelay;

        [Header("Ranged Exclusive config")] 
        [ShowIf(EConditionOperator.Or, "IsRanged", "IsAllTypes")] 
        [SerializeField] private Transform shootOrigin;
        public Transform ShootOrigin => shootOrigin;

        // IsAttacking checking before trying to attack has to be handled per attack handler,
        // a bug(?) makes it so if its checked here the second attack (or more) is executed even if IsAttacking should return
        public bool IsAttacking { get; private set;}
        
        protected AttackData[] AttackDatas = new AttackData[4];
        
        // Track cooldowns by slot index
        private Dictionary<int, float> cooldownTimers = new();

        
        protected void Awake()
        {
            InitializeCooldowns();
        }

        private void InitializeCooldowns()
        {
            if (AttackDatas == null)
                return;
            
            cooldownTimers.Clear();
            for (int i = 0; i < AttackDatas.Length; i++)
                cooldownTimers[i] = 0f;
        }

        private void InitializeNewCooldowns()
        {
            if (AttackDatas == null)
                return;

            for (int i = 0; i < AttackDatas.Length; i++)
            {
                if (cooldownTimers.ContainsKey(i) && cooldownTimers[i] > 0f)
                    continue;
                
                cooldownTimers[i] = 0f;
            }
        }

        protected virtual void Update()
        {
            var keys = new List<int>(cooldownTimers.Keys);
            
            foreach (var slotIndex in keys)
            {
                if (cooldownTimers[slotIndex] > 0f)
                {
                    cooldownTimers[slotIndex] -= Time.deltaTime;
                    if (cooldownTimers[slotIndex] < 0f)
                        cooldownTimers[slotIndex] = 0f;
                }
            }
        }

        protected void AttackByIndex(int slotIndex, AttackContext ctx, Action onFinish = null)
        {
            if (slotIndex < 0 || slotIndex >= AttackDatas.Length)
                return;

            if (IsSlotOnCooldown(slotIndex))
                return;

            AttackData data = AttackDatas[slotIndex];
            Attack attack = data.CreateAttack(this);
            
            if (attack == null)
            {
                Debug.LogError($"Failed to create attack for slot {slotIndex}");
                return;
            }

            IsAttacking = true;
            
            StartCoroutine(attack.ExecuteAttack(ctx, onFinish));
            cooldownTimers[slotIndex] = data.cooldown;
        }

        // Only used by the enemy AI - uses first available attack
        public virtual void Attack(AttackContext ctx, System.Action onFinish = null)
        {
            for (int i = 0; i < AttackDatas.Length; i++)
            {
                if (!IsSlotOnCooldown(i))
                {
                    AttackByIndex(i, ctx, onFinish);
                    return;
                }
            }
        }

        public bool IsSlotOnCooldown(int slotIndex)
        {
            return cooldownTimers.ContainsKey(slotIndex) && cooldownTimers[slotIndex] > 0f;
        }

        public bool IsAnySlotOnCooldown()
        {
            for (int i = 0; i < cooldownTimers.Count; i++)
            {
                if (cooldownTimers.ContainsKey(i) && cooldownTimers[i] > 0f)
                    return true;
            }

            return false;
        }

        public float GetSlotCooldown(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= AttackDatas.Length)
                return 0f;
            
            return cooldownTimers.GetValueOrDefault(slotIndex, 0f);
        }
        
        public void SetMeleeConfig(Collider area, DamageRelay relay)
        {
            damageArea = area;
            dmgRelay = relay;
        }
        
        public void SetRangedConfig(Transform origin)
        {
            shootOrigin = origin;
        }

        public void UpdateAttackSlots(AttackData[] newAttacks)
        {
            AttackDatas = newAttacks;
            InitializeNewCooldowns();
        }

        public AttackData GetAttackData(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= AttackDatas.Length)
                return null;
            return AttackDatas[slotIndex];
        }

        protected virtual void OnAttackFinished()
        {
            IsAttacking = false;
        }
    }
}