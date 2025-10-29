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
        public event Action<int> AttackPressedEvent;
        
        // Gameplay Variables
        private Vector2 moveInput;
        private bool attackOnePressed;
        private bool attackTwoPressed;
        private bool attackThreePressed;
        private bool attackFourPressed;
        private bool dashPressed;
        private bool interactPressed;
        private bool inventoryPressed;
        
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
        
            playerInput.Gameplay.AttackOne.performed += OnAttackOne;
            playerInput.Gameplay.AttackTwo.performed += OnAttackTwo;
            playerInput.Gameplay.AttackThree.performed += OnAttackThree;
            playerInput.Gameplay.AttackFour.performed += OnAttackFour;
            playerInput.Gameplay.Dash.performed += OnDash;
            playerInput.Gameplay.Interact.performed += OnInteract;
            playerInput.Gameplay.Inventory.performed += OnInventory;
            
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
        
            playerInput.Gameplay.AttackOne.performed -= OnAttackOne;
            playerInput.Gameplay.AttackTwo.performed -= OnAttackTwo;
            playerInput.Gameplay.AttackThree.performed -= OnAttackThree;
            playerInput.Gameplay.AttackFour.performed -= OnAttackFour;
            playerInput.Gameplay.Dash.performed -= OnDash;
            playerInput.Gameplay.Interact.performed -= OnInteract;
            playerInput.Gameplay.Inventory.performed -= OnInventory;
            
            // UI Input
            playerInput.UI.Submit.performed -= OnSubmit;
            playerInput.UI.Cancel.performed -= OnCancel;
        }

        // Any
        private void OnAny(InputAction.CallbackContext ctx) => AnyPressedEvent?.Invoke();
        
        // Gameplay Input
        private void OnAttackOne(InputAction.CallbackContext ctx)
        {
            AttackPressedEvent?.Invoke(0);
            attackOnePressed = true;
        }

        private void OnAttackTwo(InputAction.CallbackContext ctx)
        {
            AttackPressedEvent?.Invoke(1);
            attackTwoPressed = true;
        }
            
        private void OnAttackThree(InputAction.CallbackContext ctx)
        {
            AttackPressedEvent?.Invoke(2);
            attackThreePressed = true;
        }

        private void OnAttackFour(InputAction.CallbackContext ctx)
        {
            AttackPressedEvent?.Invoke(3);
            attackFourPressed = true;
        }
        private void OnDash(InputAction.CallbackContext ctx) => dashPressed = true;

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            InteractPressedEvent?.Invoke();
            interactPressed = true;
        }

        private void OnInventory(InputAction.CallbackContext ctx) => inventoryPressed = true;


        public float MoveHorizontal => moveInput.x;
        public float MoveVertical => moveInput.y;
        
        public bool AttackOnePressed => Consume(ref attackOnePressed);
        public bool AttackTwoPressed => Consume(ref attackTwoPressed);
        public bool AttackThreePressed => Consume(ref attackThreePressed);
        public bool AttackFourPressed => Consume(ref attackFourPressed);
        public bool DashPressed => Consume(ref dashPressed);
        public bool InteractPressed => Consume(ref interactPressed);
        public bool InventoryPressed => Consume(ref inventoryPressed);
        
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
