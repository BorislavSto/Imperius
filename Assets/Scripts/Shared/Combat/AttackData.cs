using UnityEngine;

public abstract class AttackData : ScriptableObject
{
    [Header("Common Attack Data")]
    public string animationTrigger = "Attack";
    public float cooldown = 1.0f;
    public float windup = 0.2f;
    public float recovery = 0.4f;
    public int damage = 10;
    public LayerMask hitMask;
    public AudioClip sfx;
}
