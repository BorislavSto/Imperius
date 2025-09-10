using UnityEngine;

public class AttackContext
{
    public Animator Animator;
    public AudioSource Audio;
    public GameObject Attacker;
    public GameObject Target;
    public System.Action<string, float> SetCooldown;

    public void FaceTarget()
    {
        if (Target is null || Attacker is null) return;
        
        Vector3 dir = (Target.transform.position - Attacker.transform.position).normalized;
        dir.y = 0;
        
        if (dir.sqrMagnitude > 0.001f)
            Attacker.transform.forward = dir;
    }
}
