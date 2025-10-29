using System;
using Combat;
using UnityEngine;

namespace Enemies.Combat
{
    public class EnemyAttackHandler : AttackHandler
    {
        [SerializeField] private AttackData[] enemyAttackDatas;

        private new void Awake()
        {
            AttackDatas = enemyAttackDatas;
            base.Awake();
        }
        
        public override void Attack(AttackContext ctx, Action onFinish = null)
        {
            int bestSlotIndex = ChooseAttackSlot(ctx);

            if (bestSlotIndex >= 0)
                AttackByIndex(bestSlotIndex, ctx, () =>
                {
                    OnAttackFinished();
                    onFinish?.Invoke();
                });
            else
                Debug.Log("No attacks available for enemy!");
        }

        private int ChooseAttackSlot(AttackContext ctx)
        {
            int bestSlotIndex = -1;
            float bestScore = float.MinValue;

            for (int i = 0; i < AttackDatas.Length; i++)
            {
                if (IsSlotOnCooldown(i))
                    continue;

                float score = EvaluateAttack(AttackDatas[i], ctx);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestSlotIndex = i;
                }
            }

            // Fallback: if nothing ready, pick a random slot (even if on cooldown)
            if (bestSlotIndex < 0 && AttackDatas.Length > 0)
            {
                bestSlotIndex = UnityEngine.Random.Range(0, AttackDatas.Length);
            }

            return bestSlotIndex;
        }

        private float EvaluateAttack(AttackData data, AttackContext ctx)
        {
            float score = 0f;

            score += data.damage; // higher damage = better
            score -= data.windup * 2f; // slower attacks get penalized

            float distance = Vector3.Distance(ctx.Attacker.transform.position, ctx.TargetLocation);
            if (distance < 3f && attackerType == AttackerType.Melee)
                score += 10f; // bonus for melee when close
            else if (distance > 5f && attackerType == AttackerType.Ranged)
                score += 10f; // bonus for ranged when far

            return score;
        }
        
        protected override void OnAttackFinished()
        {
            base.OnAttackFinished();
        }
    }
}
