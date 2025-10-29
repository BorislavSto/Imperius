using Combat;
using UnityEngine;

public class TestInteractableThree : MonoBehaviour, IInteractable
{
    private bool isOpen;
    
    public void Interact(GameObject interactor)
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "'Door' opened." : "'Door' closed.");
        
        interactor.GetComponent<PlayerAttackDataHandler>()?.RemoveAttack(1);
    }
}