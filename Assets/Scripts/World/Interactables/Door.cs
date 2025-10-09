using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen;
    
    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Door opened." : "Door closed.");
    }
}
