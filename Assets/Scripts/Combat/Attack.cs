using System.Collections;

namespace Combat
{
    public abstract class Attack
    {
        protected AttackData data;

        protected Attack(AttackData data)
        {
            this.data = data;
        }

        public abstract IEnumerator ExecuteAttack(AttackContext ctx, System.Action onFinished = null);
    }
}
