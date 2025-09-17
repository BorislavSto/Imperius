using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.Combat
{
    public class EnemyAttackHandler : AttackHandler
    {
        public override void Attack(AttackContext ctx, Action onFinish = null)
        {
            AttackData data = ChooseAttack(ctx);

            Attack(data, ctx, onFinish);
        }

        private AttackData ChooseAttack(AttackContext ctx)
        {
            AttackData bestAttack = null;
            float bestScore = float.MinValue;

            foreach (var data in attackDatas)
            {
                if (cooldownTimers[data] > 0f)
                    continue;

                float score = EvaluateAttack(data, ctx);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAttack = data;
                }
            }

            // fallback if nothing ready
            return bestAttack ?? attackDatas[Random.Range(0, attackDatas.Length)];
        }

        private float EvaluateAttack(AttackData data, AttackContext ctx)
        {
            float score = 0f;

            score += data.damage; // higher damage = better
            score -= data.windup * 2f; // slower attacks get penalized

            float distance = Vector3.Distance(ctx.Attacker.transform.position, ctx.Target.position);
            if (distance < 3f && attackerType == AttackerType.Melee)
                score += 10f; // bonus for melee when close
            else if (distance > 5f && attackerType == AttackerType.Ranged)
                score += 10f; // bonus for ranged when far

            return score;
        }
    }
}
