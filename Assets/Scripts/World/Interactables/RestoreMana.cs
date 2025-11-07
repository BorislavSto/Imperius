using Player;
using UnityEngine;

public class RestoreMana : MonoBehaviour, IInteractable
{
    [SerializeField] private int maxFill = 100;
    
    public void Interact(GameObject interactor)
    {
        interactor.GetComponent<PlayerCharacter>()?.GainMana(maxFill);
    }
}