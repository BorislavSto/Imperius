using UnityEngine;

namespace Combat
{
    public class AttackContext
    {
        public Animator Animator;
        public AudioSource Audio;
        public GameObject Attacker;
        public Vector3 TargetLocation = Vector3.zero;

        public void FaceTarget() // TODO: has to be changed. Attack context should not take care of the rotation
        {
            if (TargetLocation == Vector3.zero || Attacker is null) 
                return;

            Vector3 dir = (TargetLocation - Attacker.transform.position).normalized;
            dir.y = 0;

            if (dir.sqrMagnitude > 0.001f)
                Attacker.transform.forward = dir;
        }
    }
}
