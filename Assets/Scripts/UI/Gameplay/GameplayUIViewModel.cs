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
        private EventBinding<PlayerDataEvent> playerDataEventBinding;
        
        public GameplayUIViewModel(GameplayUIView view, GameplayUIModel model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
            
            view.ShowView();
            
            attacksChangedBinding = new EventBinding<PlayerAttacksChanged>(OnPlayerAttacksChanged);
            attackUsedBinding = new EventBinding<AttackUsed>(OnPlayerAttackUsed);
            playerDataEventBinding = new EventBinding<PlayerDataEvent>(OnPlayerDataUpdated);
            
            EventBus<PlayerAttacksChanged>.Register(attacksChangedBinding);
            EventBus<AttackUsed>.Register(attackUsedBinding);
            EventBus<PlayerDataEvent>.Register(playerDataEventBinding);
        }
        
        private void OnPlayerAttacksChanged(PlayerAttacksChanged obj)
        {
            view.UpdateUIAddingAttack(obj.NewAttacks);
        }

        private void OnPlayerAttackUsed(AttackUsed obj)
        {
            view.UpdateUIAttackSlotCooldown(obj.SlotIndex);
        }
        
        private void OnPlayerDataUpdated(PlayerDataEvent obj)
        {
            view.UpdateUIPlayerData(obj.PlayerData);
        }

        public override void Dispose()
        {
            EventBus<PlayerAttacksChanged>.Deregister(attacksChangedBinding);
            EventBus<AttackUsed>.Deregister(attackUsedBinding);
            EventBus<PlayerDataEvent>.Deregister(playerDataEventBinding);
            base.Dispose();
        }
    }
}