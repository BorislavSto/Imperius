using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(IInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float dashSpeed = 10f;

        private IInputHandler inputHandler;
        private Transform cam;

        private void Awake()
        {
            inputHandler = GetComponent<IInputHandler>();
            cam = Camera.main.transform; // Make better
        }

        private void Update()
        {
            HandleMovement();
            HandleActions();
        }

        private void HandleMovement()
        {
            throw new NotImplementedException();
        }
        
        private void HandleActions()
        {
            throw new NotImplementedException();
        }
    }
}
