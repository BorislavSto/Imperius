using UnityEngine;

public class AttackContext
{
    public Animator Animator;
    public AudioSource Audio;
    public Transform Attacker;
    public GameObject Target;

    public void FaceTarget()
    {
        if (Target == null || Attacker == null) return;
        Vector3 dir = (Target.transform.position - Attacker.position).normalized;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            Attacker.forward = dir;
    }
}
