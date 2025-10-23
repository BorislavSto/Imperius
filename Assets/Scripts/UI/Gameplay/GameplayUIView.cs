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
        [SerializeField] private List<AttackUISlot> attackButtons = new();
        
        // here i can do things such as change the textures, health etc
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
    }
}
