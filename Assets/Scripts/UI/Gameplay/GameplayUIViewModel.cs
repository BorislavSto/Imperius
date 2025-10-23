using EventBus;
using Player;
using UnityEngine;

namespace UI
{
    public class GameplayUIViewModel : ViewModel
    {
        private GameplayUIView view;
        private GameplayUIModel model;
        
        private EventBinding<PlayerAttacksChanged> attacksChangedBinding;
        private EventBinding<AttackUsed> attackUsedBinding;
        
        public GameplayUIViewModel(GameplayUIView view, GameplayUIModel model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
            
            view.ShowView();
            
            attacksChangedBinding = new EventBinding<PlayerAttacksChanged>(OnPlayerAttacksChanged);
            attackUsedBinding = new EventBinding<AttackUsed>(OnPlayerAttackUsed);
            
            EventBus<PlayerAttacksChanged>.Register(attacksChangedBinding);
            EventBus<AttackUsed>.Register(attackUsedBinding);
        }
        
        private void OnPlayerAttacksChanged(PlayerAttacksChanged obj)
        {
            Debug.Log(obj.NewAttacks.Length);
            view.UpdateUIAddingAttack(obj.NewAttacks);
        }

        private void OnPlayerAttackUsed(AttackUsed obj)
        {
            view.UpdateUIAttackSlotCooldown(obj.SlotIndex);
        }

        public override void Dispose()
        {
            EventBus<PlayerAttacksChanged>.Deregister(attacksChangedBinding);
            EventBus<AttackUsed>.Deregister(attackUsedBinding);
            base.Dispose();
        }
    }
}