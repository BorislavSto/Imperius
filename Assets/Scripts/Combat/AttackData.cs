using UnityEngine;

namespace Combat
{
    public abstract class AttackData : ScriptableObject
    {
        [Header("Visuals")] 
        public Sprite icon;
        public string attackName;
        
        [Header("Common Attack Data")]
        public string animationTrigger = "Attack";
        public float cooldown = 1.0f;
        public float windup = 0.2f;
        public float recovery = 0.4f;
        public int damage = 10;
        public int manaCost = 20;
        public LayerMask hitMask;
        public AudioClip castSfx;
        public AudioClip hitSfx;

        public abstract Attack CreateAttack(AttackHandler attackHandler);
    }
}
