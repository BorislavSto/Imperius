using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField] private PlayerInputActions playerInput;

        private Vector2 moveInput;
        private bool attackPressed;
        private bool dashPressed;
        private bool interactPressed;
        private bool inventoryPressed;
        private bool special1Pressed;
        private bool special2Pressed;
        private bool special3Pressed;

        private void OnEnable()
        {
            playerInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            playerInput.Player.Attack.performed += ctx => attackPressed = true;
            playerInput.Player.Dash.performed += ctx => dashPressed = true;
            playerInput.Player.Interact.performed += ctx => interactPressed = true;
            playerInput.Player.Inventory.performed += ctx => inventoryPressed = true;
            playerInput.Player.Special1.performed += ctx => special1Pressed = true;
            playerInput.Player.Special2.performed += ctx => special2Pressed = true;
            playerInput.Player.Special3.performed += ctx => special3Pressed = true;
        }

        private void OnDisable()
        {
            playerInput.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Move.canceled -= ctx => moveInput = Vector2.zero;

            playerInput.Player.Attack.performed -= ctx => attackPressed = true;
            playerInput.Player.Dash.performed -= ctx => dashPressed = true;
            playerInput.Player.Interact.performed -= ctx => interactPressed = true;
            playerInput.Player.Inventory.performed -= ctx => inventoryPressed = true;
            playerInput.Player.Special1.performed -= ctx => special1Pressed = true;
            playerInput.Player.Special2.performed -= ctx => special2Pressed = true;
            playerInput.Player.Special3.performed -= ctx => special3Pressed = true;
        }

        public float MoveHorizontal => moveInput.x;
        public float MoveVertical => moveInput.y;

        public bool AttackPressed { get { bool temp = attackPressed; attackPressed = false; return temp; } }
        public bool DashPressed { get { bool temp = dashPressed; dashPressed = false; return temp; } }
        public bool InteractPressed { get { bool temp = interactPressed; interactPressed = false; return temp; } }
        public bool InventoryPressed { get { bool temp = inventoryPressed; inventoryPressed = false; return temp; } }
        public bool Skill1Pressed { get { bool temp = special1Pressed; special1Pressed = false; return temp; } }
        public bool Skill2Pressed { get { bool temp = special2Pressed; special2Pressed = false; return temp; } }
        public bool Skill3Pressed { get { bool temp = special3Pressed; special3Pressed = false; return temp; } }

        public Vector3 AimDirection
        {
            get
            {
                Vector3 mousePos = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 dir = hit.point - transform.position;
                    dir.y = 0;
                    return dir.normalized;
                }

                return transform.forward;
            }
        }
    }
}
