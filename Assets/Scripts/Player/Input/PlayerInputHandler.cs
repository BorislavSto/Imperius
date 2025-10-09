using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour, IInputHandler
    {
        private PlayerInputActions playerInput;

        // Any
        public event Action AnyPressedEvent;
        
        // Gameplay Events
        private Action<InputAction.CallbackContext> movePerformed;
        private Action<InputAction.CallbackContext> moveCanceled;
        public event Action InteractPressedEvent;
        
        // Gameplay Variables
        private Vector2 moveInput;
        private bool attackPressed;
        private bool dashPressed;
        private bool interactPressed;
        private bool inventoryPressed;
        private bool special1Pressed;
        private bool special2Pressed;
        private bool special3Pressed;
        
        // UI Events
        public event Action CancelPressedEvent;        
        public event Action SubmitPressedEvent;        
        
        private void OnEnable()
        {
            playerInput = new PlayerInputActions();
            playerInput.Enable();
            
            // Any
            playerInput.Gameplay.AnyInput.performed += OnAny;
            
            // Gameplay Input
            movePerformed = ctx => moveInput = ctx.ReadValue<Vector2>();
            moveCanceled = ctx => moveInput = Vector2.zero;
            
            playerInput.Gameplay.Move.performed += movePerformed;
            playerInput.Gameplay.Move.canceled += moveCanceled;
        
            playerInput.Gameplay.Attack.performed += OnAttack;
            playerInput.Gameplay.Dash.performed += OnDash;
            playerInput.Gameplay.Interact.performed += OnInteract;
            playerInput.Gameplay.Inventory.performed += OnInventory;
            playerInput.Gameplay.Special1.performed += OnSpecial1;
            playerInput.Gameplay.Special2.performed += OnSpecial2;
            playerInput.Gameplay.Special3.performed += OnSpecial3;
            
            // UI Input
            playerInput.UI.Submit.performed += OnSubmit;
            playerInput.UI.Cancel.performed += OnCancel;
        }
        
        private void OnDisable()
        {
            playerInput.Disable();
            
            // Any
            playerInput.Gameplay.AnyInput.performed -= OnAny;
            
            // Gameplay Input
            playerInput.Gameplay.Move.performed -= movePerformed;
            playerInput.Gameplay.Move.canceled -= moveCanceled;
        
            playerInput.Gameplay.Attack.performed -= OnAttack;
            playerInput.Gameplay.Dash.performed -= OnDash;
            playerInput.Gameplay.Interact.performed -= OnInteract;
            playerInput.Gameplay.Inventory.performed -= OnInventory;
            playerInput.Gameplay.Special1.performed -= OnSpecial1;
            playerInput.Gameplay.Special2.performed -= OnSpecial2;
            playerInput.Gameplay.Special3.performed -= OnSpecial3;
            
            // UI Input
            playerInput.UI.Submit.performed -= OnSubmit;
            playerInput.UI.Cancel.performed -= OnCancel;
        }

        // Any
        private void OnAny(InputAction.CallbackContext ctx) => AnyPressedEvent?.Invoke();
        
        // Gameplay Input
        private void OnAttack(InputAction.CallbackContext ctx) => attackPressed = true;
        private void OnDash(InputAction.CallbackContext ctx) => dashPressed = true;

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            InteractPressedEvent?.Invoke();
            interactPressed = true;
        }

        private void OnInventory(InputAction.CallbackContext ctx) => inventoryPressed = true;
        private void OnSpecial1(InputAction.CallbackContext ctx) => special1Pressed = true;
        private void OnSpecial2(InputAction.CallbackContext ctx) => special2Pressed = true;
        private void OnSpecial3(InputAction.CallbackContext ctx) => special3Pressed = true;

        public float MoveHorizontal => moveInput.x;
        public float MoveVertical => moveInput.y;
        
        public bool AttackPressed => Consume(ref attackPressed);
        public bool DashPressed => Consume(ref dashPressed);
        public bool InteractPressed => Consume(ref interactPressed);
        public bool InventoryPressed => Consume(ref inventoryPressed);
        public bool Skill1Pressed => Consume(ref special1Pressed);
        public bool Skill2Pressed => Consume(ref special2Pressed);
        public bool Skill3Pressed => Consume(ref special3Pressed);

        public Vector3 GetAimDirection(Vector3 originPosition)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 dir = hit.point - originPosition;
                dir.y = 0;
                return dir.normalized;
            }
            return Vector3.forward;
        }
        
        // UI Input
        private void OnSubmit(InputAction.CallbackContext ctx) => SubmitPressedEvent?.Invoke();
        private void OnCancel(InputAction.CallbackContext ctx) => CancelPressedEvent?.Invoke();

        private bool Consume(ref bool flag)
        {
            bool temp = flag;
            flag = false;
            return temp;
        }
    }
}
