using Combat;
using UnityEngine;

public class TestInteractableTwo : MonoBehaviour, IInteractable
{
    [SerializeField] private AttackData attackToGrant;
    private bool isOpen;
    
    public void Interact(GameObject interactor)
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "'Door' opened." : "'Door' closed.");
        
        interactor.GetComponent<PlayerAttackDataHandler>()?.ReplaceAttack(0, attackToGrant);
    }
}