using Combat;
using UnityEngine;

public class GainAttack : MonoBehaviour, IInteractable
{
    [SerializeField] private AttackData attackToGrant;
    [SerializeField] private bool destroyAfterUse;
    
    public void Interact(GameObject interactor)
    {
        interactor.GetComponent<PlayerAttackDataHandler>()?.AddAttack(attackToGrant);
        
        if (destroyAfterUse)
            Destroy(gameObject);
    }
}