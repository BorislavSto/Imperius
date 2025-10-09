using System;
using Player;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactSphereRange = 3f;
    [SerializeField] private LayerMask interactableLayerMask;

    private void OnEnable()
    {
        InputManager.Instance.InputHandler.InteractPressedEvent += OnInteract;
    }

    private void OnDestroy()
    {
        InputManager.Instance.InputHandler.InteractPressedEvent -= OnInteract;
    }

    private void OnInteract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactSphereRange, interactableLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log("Interacting with " + hitCollider.name);
                interactable.Interact();
                break;
            }
        }
    }
    
    // For testing purposes, visualize the interaction sphere in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.goldenRod;
        Gizmos.DrawWireSphere(transform.position, interactSphereRange);
    }
}