using Combat;
using EventBus;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerAttackDataHandler))]
    public class PlayerAttackHandler : AttackHandler
    {
        private const string EnemyTag = "Enemy";
        
        [Header("Player Attack Configuration")] 
        [SerializeField] private float radius = 5f;
        private Transform playerTransform;
        
        private PlayerInputHandler inputHandler;
        private PlayerController playerController;
        private PlayerCharacter playerCharacter;

        private void Start()
        {
            inputHandler = InputManager.Instance.InputHandler;
            playerController = GetComponent<PlayerController>();
            playerCharacter = GetComponent<PlayerCharacter>();
            
            if (playerTransform == null)
                playerTransform = transform;

            inputHandler.AttackPressedEvent += HandleAttackInput;
        }

        private void OnDestroy()
        { 
            inputHandler.AttackPressedEvent -= HandleAttackInput;
        }

        private void HandleAttackInput(int attackNumber)
        {
            if (inputHandler == null)
                inputHandler = InputManager.Instance.InputHandler;

            ExecuteAttackByIndex(attackNumber);
        }

        private void ExecuteAttackByIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= AttackDatas.Length)
            {
                Debug.LogWarning($"Attack index {slotIndex} is out of range. Available attacks: {AttackDatas.Length}");
                return;
            }

            if (AttackDatas[slotIndex] == null)
            {
                Debug.LogWarning($"Attack index {slotIndex} is null/empty.");
                return;
            }
            
            if (IsSlotOnCooldown(slotIndex))
            {
                float remaining = GetSlotCooldown(slotIndex);
                Debug.Log($"Attack slot {slotIndex} is on cooldown: {remaining:F1}s remaining");
                return;
            }

            if (IsAttacking)
                return;
            
            AttackData attackData = AttackDatas[slotIndex];

            if (!playerCharacter.UseMana(attackData.manaCost))
            {
                Debug.Log($"Not Enough mana to cast {attackData}. Mana to cast {attackData.manaCost}/ available mana {playerCharacter.CurrentMana}");
                return;
            }
            
            AttackContext ctx = CreateAttackContext();
            
            // Notify UI that attack was used
            EventBus<AttackUsed>.Raise(new AttackUsed(slotIndex));
            
            // Execute the attack (cooldown starts automatically in base class)
            AttackByIndex(slotIndex, ctx, OnAttackFinished);
        }

        private AttackContext CreateAttackContext()
        {
            return new AttackContext
            {
                Animator = GetComponentInChildren<Animator>(),
                Audio = GetComponent<AudioSource>(),
                Attacker = gameObject,
                TargetLocation = CheckAroundPlayerForEnemies(),
            };
        }
        
        private Vector3 CheckAroundPlayerForEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(playerTransform.position, radius);
            Vector3 closestEnemy = Vector3.zero;
            float closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
            {
                if (hit.CompareTag(EnemyTag))
                {
                    float distance = Vector3.Distance(playerTransform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = hit.transform.position;
                    }
                }
            }
            
            if (closestEnemy == Vector3.zero)
                closestEnemy = playerTransform.position + playerTransform.forward * 10f; // just a point in front of the player
            else
                playerController.SetRotationToTarget(closestEnemy);

            return closestEnemy;
        }

        protected override void OnAttackFinished()
        {
            base.OnAttackFinished();
            playerController.ClearRotationToTarget();
        }
        
        private void OnDrawGizmos()
        {
            if (playerTransform == null)
                playerTransform = transform;
            
            Gizmos.color = Color.chartreuse;
            Gizmos.DrawWireSphere(playerTransform.position, radius);
        }
    }
}
