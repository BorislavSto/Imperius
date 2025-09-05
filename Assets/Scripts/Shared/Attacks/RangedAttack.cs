using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private Transform shootOrigin;
    
    public Transform ShootOrigin => shootOrigin;
}
