using System;
using System.Collections;

public interface IAttackBehavior
{
    IEnumerator ExecuteAttack(AttackContext ctx, Action onFinished = null);
}
