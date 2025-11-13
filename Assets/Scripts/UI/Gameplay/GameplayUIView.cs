using System.Collections.Generic;
using Combat;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayUIView : View
    {
        private GameplayUIViewModel viewModel;
        
        [SerializeField] private Image background;
        
        [Header("Health and Mana")]
        [SerializeField] private StatusBarUI healthBar;
        [SerializeField] private StatusBarUI manaBar;
        
        [Header("Attacks")]
        [SerializeField] private List<AttackUISlot> attackButtons = new();
        
        public void UpdateUIAddingAttack(AttackData[] attacks)
        {
            for (int i = 0; i < attackButtons.Count; i++)
            {
                if (i < attacks.Length && attacks[i] != null)
                {
                    if (attackButtons[i].HasAttack(attacks[i]) && attackButtons[i].IsOnCooldown())
                        continue;
        
                    attackButtons[i].Initialize(attacks[i], i + 1);
                }
                else
                {
                    attackButtons[i].Clear();
                }
            }
        }
        
        public void UpdateUIAttackSlotCooldown(int attackIndex)
        {
            if (attackIndex < 0 || attackIndex >= attackButtons.Count)
                return;
            
            attackButtons[attackIndex].TriggerCooldown();
        }
        
        public void UpdateUIPlayerData(PlayerGameplayData data)
        {
            if (!healthBar.IsSetUp)
                healthBar.SetStats(data.Health);

            if (!manaBar.IsSetUp)
                manaBar.SetStats(data.Mana, true, data.ManaRechargeRate, data.ManaRechargeAmount);
            
            if (healthBar.FillAmount != data.Health)
                healthBar.SetFillAmount(data.Health);

            if (manaBar.FillAmount != data.Mana)
                manaBar.SetFillAmount(data.Mana);
        }
    }
}
