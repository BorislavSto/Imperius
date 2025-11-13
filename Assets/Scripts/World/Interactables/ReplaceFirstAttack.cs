using Combat;
using UnityEngine;

public class ReplaceFirstAttack : MonoBehaviour, IInteractable
{
    [SerializeField] private AttackData attackToGrant;
    [SerializeField] private bool destroyAfterUse;
    
    public void Interact(GameObject interactor)
    {
        interactor.GetComponent<PlayerAttackDataHandler>()?.ReplaceAttack(0, attackToGrant);
        
        if (destroyAfterUse)
            Destroy(gameObject);
    }
}