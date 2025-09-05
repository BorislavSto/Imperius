using UnityEngine;

public class MeleeAttack : Attack
{
    [SerializeField] private Collider damageArea;

    private void Awake()
    {
        if (damageArea != null) damageArea.enabled = false;
    }

    public Collider DamageArea => damageArea;
}
