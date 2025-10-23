using System.Collections.Generic;
using EventBus;
using Player;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(PlayerAttackHandler))]
    public class PlayerAttackDataHandler : MonoBehaviour
    {
        [Header("Attack Configuration")]
        [SerializeField] private AttackData[] startingAttacks;
        [SerializeField] private int maxAttackSlots = 4;
        
        private PlayerAttackHandler playerAttackHandler;
        private List<AttackData> currentAttacks = new(4);
        
        private void Awake()
        {
            if (playerAttackHandler == null)
                playerAttackHandler = GetComponent<PlayerAttackHandler>();

            InitializeAttacks();
        }

        private void InitializeAttacks()
        {
            currentAttacks.Clear();
            
            if (startingAttacks != null && startingAttacks.Length > 0)
            {
                foreach (var attack in startingAttacks)
                {
                    if (attack != null)
                        currentAttacks.Add(attack);
                }
            }

            UpdateAttackHandler();
        }

        public bool AddAttack(AttackData newAttack)
        {
            if (newAttack == null)
            {
                Debug.LogWarning("Cannot add null attack");
                return false;
            }

            for (int i = 0; i < currentAttacks.Count; i++)
            {
                if (currentAttacks[i] == null)
                {
                    currentAttacks[i] = newAttack;
                    Debug.Log($"Filled empty slot {i} with {newAttack.name}");
                    UpdateAttackHandler();
                    return true;
                }
            }

            if (currentAttacks.Count < maxAttackSlots)
            {
                currentAttacks.Add(newAttack);
                Debug.Log($"Added new attack {newAttack.name} to slot {currentAttacks.Count - 1}");
                UpdateAttackHandler();
                return true;
            }

            Debug.LogWarning("No available slots for new attack");
            return false;
        }

        public bool ReplaceAttack(int slotIndex, AttackData newAttack)
        {
            if (slotIndex < 0 || slotIndex >= currentAttacks.Count)
            {
                Debug.LogWarning($"Invalid slot index: {slotIndex}");
                return false;
            }

            if (newAttack == null)
            {
                Debug.LogWarning("Cannot replace with null attack");
                return false;
            }

            if (playerAttackHandler.IsSlotOnCooldown(slotIndex))
            {
                float remaining = playerAttackHandler.GetSlotCooldown(slotIndex);
                Debug.LogWarning($"Cannot replace attack at slot {slotIndex}: Attack is on cooldown ({remaining:F1}s remaining)");
                return false;
            }
            
            AttackData oldAttack = currentAttacks[slotIndex];
            currentAttacks[slotIndex] = newAttack;
            UpdateAttackHandler();
            Debug.Log($"Replaced attack at slot {slotIndex}: {oldAttack?.name} -> {newAttack.name}");
            return true;
        }

        public bool RemoveAttack(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= currentAttacks.Count)
            {
                Debug.LogWarning($"Invalid slot index: {slotIndex}");
                return false;
            }

            if (playerAttackHandler.IsSlotOnCooldown(slotIndex))
            {
                float remaining = playerAttackHandler.GetSlotCooldown(slotIndex);
                Debug.LogWarning($"Cannot remove attack at slot {slotIndex}: Attack is on cooldown ({remaining:F1}s remaining)");
                return false;
            }

            AttackData removed = currentAttacks[slotIndex];
            currentAttacks[slotIndex] = null;
            //currentAttacks.RemoveAt(slotIndex);
            UpdateAttackHandler();
            Debug.Log($"Removed attack from slot {slotIndex}: {removed?.name}");
            return true;
        }

        public bool SwapAttacks(int slotA, int slotB)
        {
            if (slotA < 0 || slotA >= currentAttacks.Count || 
                slotB < 0 || slotB >= currentAttacks.Count)
            {
                Debug.LogWarning($"Invalid slot indices: {slotA}, {slotB}");
                return false;
            }

            if (playerAttackHandler.IsSlotOnCooldown(slotA))
            {
                float remaining = playerAttackHandler.GetSlotCooldown(slotA);
                Debug.LogWarning($"Cannot swap: Slot {slotA} is on cooldown ({remaining:F1}s remaining)");
                return false;
            }

            if (playerAttackHandler.IsSlotOnCooldown(slotB))
            {
                float remaining = playerAttackHandler.GetSlotCooldown(slotB);
                Debug.LogWarning($"Cannot swap: Slot {slotB} is on cooldown ({remaining:F1}s remaining)");
                return false;
            }

            (currentAttacks[slotA], currentAttacks[slotB]) = (currentAttacks[slotB], currentAttacks[slotA]);
            UpdateAttackHandler();
            Debug.Log($"Swapped attacks: slot {slotA} <-> slot {slotB}");
            return true;
        }

        // TODO: the whole attack system needs a redo for insert to work
        // public bool InsertAttack(int slotIndex, AttackData newAttack)
        // {
        //     if (newAttack == null)
        //     {
        //         Debug.LogWarning("Cannot insert null attack");
        //         return false;
        //     }
        //
        //     if (currentAttacks.Count >= maxAttackSlots)
        //     {
        //         Debug.LogWarning($"Cannot insert attack: Max slots ({maxAttackSlots}) reached");
        //         return false;
        //     }
        //
        //     if (slotIndex < 0)
        //         slotIndex = 0;
        //     else if (slotIndex > currentAttacks.Count)
        //         slotIndex = currentAttacks.Count;
        //
        //     currentAttacks.Insert(slotIndex, newAttack);
        //     UpdateAttackHandler();
        //     Debug.Log($"Inserted attack: {newAttack.name} at slot {slotIndex}");
        //     return true;
        // }

        public void ClearAllAttacks()
        {
            currentAttacks.Clear();
            UpdateAttackHandler();
            Debug.Log("Cleared all attacks");
        }

        public void SetAttacks(AttackData[] newAttacks)
        {
            currentAttacks.Clear();
            
            if (newAttacks != null)
            {
                int slotsToAdd = Mathf.Min(newAttacks.Length, maxAttackSlots);
                for (int i = 0; i < slotsToAdd; i++)
                {
                    if (newAttacks[i] != null)
                        currentAttacks.Add(newAttacks[i]);
                }
            }

            UpdateAttackHandler();
            Debug.Log($"Set new attacks: {currentAttacks.Count} attacks loaded");
        }

        public AttackData GetAttack(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= currentAttacks.Count)
                return null;
            
            return currentAttacks[slotIndex];
        }

        public AttackData[] GetAllAttacks()
        {
            return currentAttacks.ToArray();
        }

        public int GetAttackCount()
        {
            return currentAttacks.Count;
        }

        public bool HasAvailableSlots()
        {
            return currentAttacks.Count < maxAttackSlots;
        }

        public int GetAvailableSlots()
        {
            return maxAttackSlots - currentAttacks.Count;
        }

        private void UpdateAttackHandler()
        {
            if (playerAttackHandler == null)
            {
                Debug.LogError("AttackHandler reference is missing!");
                return;
            }

            // Update the attackhandler
            playerAttackHandler.UpdateAttackSlots(currentAttacks.ToArray());

            // Only place which Raises the event for UI updates
            EventBus<PlayerAttacksChanged>.Raise(new PlayerAttacksChanged(currentAttacks.ToArray()));
        }

#if UNITY_EDITOR
        [ContextMenu("Log Current Attacks")]
        private void LogCurrentAttacks()
        {
            Debug.Log($"Current Attacks ({currentAttacks.Count}/{maxAttackSlots})");
            for (int i = 0; i < currentAttacks.Count; i++)
            {
                var attack = currentAttacks[i];
                bool onCooldown = playerAttackHandler != null && playerAttackHandler.IsSlotOnCooldown(i);
                float cooldown = playerAttackHandler != null ? playerAttackHandler.GetSlotCooldown(i) : 0f;
                
                string status = onCooldown ? $"[COOLDOWN: {cooldown:F1}s]" : "[READY]";
                Debug.Log($"Slot {i}: {(attack != null ? attack.name : "NULL")} {status}");
            }
        }
#endif
    }
}