using KinematicCharacterController;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        // Cached animator hashes
        private static readonly int SpeedMagnitudeHash = Animator.StringToHash("SpeedMagnitude");
        private static readonly int DashTriggerHash = Animator.StringToHash("Dash");
        private static readonly int ResetTriggerHash = Animator.StringToHash("Reset");
        
        [Header("Movement Settings")]
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float stableMovementSharpness = 15f;
        [SerializeField] private float rotationSharpness = 15f;
        [SerializeField] private float maxAirMoveSpeed = 4f;
        [SerializeField] private float airAccelerationSpeed = 5f;
        [SerializeField] private float drag = 0.1f;
        [SerializeField] private Vector3 gravity = new(0, -20f, 0);

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;
        
        [Header("Animation Settings")]
        [SerializeField] private float animationSpeedDamping = 0.1f;
        
        [Header("References")]
        [SerializeField] private Animator animator;

        private KinematicCharacterMotor characterMotor;
        private IInputHandler inputHandler;
        private Camera mainCamera;
        
        private Vector3 moveInputVector;
        private Vector3 cameraRelativeMoveInput;
        
        // Dash state
        private bool isDashing;
        private float dashTimer;
        private float dashCooldownTimer;
        private Vector3 dashDirection;
        
        // Rotation state
        private bool isFacingTarget;
        private Vector3 targetDirection;
        
        // Set by the player attack handler,
        // used to stop the player from moving when attacking
        private bool isAttacking;

        private void Awake()
        {
            characterMotor = GetComponent<KinematicCharacterMotor>();
            inputHandler = InputManager.Instance.InputHandler;
            mainCamera = Camera.main;
            
            characterMotor.CharacterController = this;
        }
      
        private void Update()
        {
            UpdateAnimations();
            
            if (isAttacking)
                return;
            
            HandleInput();
            HandleDash();
        }

        private void HandleInput()
        {
            // Raw input
            moveInputVector = new Vector3(inputHandler.MoveHorizontal, 0f, inputHandler.MoveVertical);
            
            // Convert to camera-relative movement
            if (mainCamera != null)
            {
                Vector3 camForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
                Vector3 camRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up).normalized;

                cameraRelativeMoveInput = (camForward * inputHandler.MoveVertical) + (camRight * inputHandler.MoveHorizontal);
                cameraRelativeMoveInput = Vector3.ClampMagnitude(cameraRelativeMoveInput, 1f);
            }
            else
            {
                cameraRelativeMoveInput = Vector3.ClampMagnitude(moveInputVector, 1f);
            }
        }

        private void HandleDash()
        {
            dashCooldownTimer -= Time.deltaTime;
            
            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    isDashing = false;
                }
            }
            else if (inputHandler.DashPressed && dashCooldownTimer <= 0f && CanDash())
            {
                StartDash();
            }
        }

        private bool CanDash()
        {
            // Only allow dash if there's movement input
            return cameraRelativeMoveInput.sqrMagnitude > 0.01f;
        }

        private void StartDash()
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashDirection = cameraRelativeMoveInput.normalized;
            
            // Trigger dash animation
            if (animator != null)
                animator.SetTrigger(DashTriggerHash);
        }

        private void UpdateAnimations()
        {
            if (animator == null) 
                return;

            if (!isDashing)
            {
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterMotor.Velocity, characterMotor.CharacterUp);
                float normalizedSpeed = horizontalVelocity.magnitude / runSpeed;
                
                animator.SetFloat(SpeedMagnitudeHash, normalizedSpeed, animationSpeedDamping, Time.deltaTime);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (isAttacking)
                currentVelocity = Vector3.zero;
            
            if (isDashing)
            {
                ApplyDashVelocity(ref currentVelocity);
                return;
            }
            
            if (characterMotor.GroundingStatus.IsStableOnGround)
                ApplyGroundedMovement(ref currentVelocity, deltaTime);
            else
                ApplyAirMovement(ref currentVelocity, deltaTime);
        }

        private void ApplyDashVelocity(ref Vector3 currentVelocity)
        {
            if (characterMotor.GroundingStatus.IsStableOnGround)
            {
                Vector3 inputRight = Vector3.Cross(dashDirection, characterMotor.CharacterUp);
                Vector3 reorientedDash = Vector3.Cross(characterMotor.GroundingStatus.GroundNormal, inputRight).normalized 
                                         * dashDirection.magnitude;
                currentVelocity = reorientedDash * dashSpeed;
            }
            else
            {
                currentVelocity = dashDirection * dashSpeed;
            }
        }

        private void ApplyGroundedMovement(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = characterMotor.GetDirectionTangentToSurface(currentVelocity, characterMotor.GroundingStatus.GroundNormal) 
                              * currentVelocity.magnitude;

            Vector3 targetVelocity = Vector3.zero;
            
            if (cameraRelativeMoveInput.sqrMagnitude > 0f)
            {
                Vector3 inputRight = Vector3.Cross(cameraRelativeMoveInput, characterMotor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(characterMotor.GroundingStatus.GroundNormal, inputRight).normalized 
                                          * cameraRelativeMoveInput.magnitude;
                targetVelocity = reorientedInput * runSpeed;
            }

            // Smooth interpolation to target velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 
                1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
        }

        private void ApplyAirMovement(ref Vector3 currentVelocity, float deltaTime)
        {
            if (cameraRelativeMoveInput.sqrMagnitude > 0f)
            {
                Vector3 targetVelocity = cameraRelativeMoveInput * maxAirMoveSpeed;

                // Handle movement against slopes/walls in air
                if (characterMotor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 obstructionNormal = Vector3.Cross(
                        Vector3.Cross(characterMotor.CharacterUp, characterMotor.GroundingStatus.GroundNormal),
                        characterMotor.CharacterUp).normalized;

                    targetVelocity = Vector3.ProjectOnPlane(targetVelocity, obstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetVelocity - currentVelocity, gravity);
                currentVelocity += velocityDiff * airAccelerationSpeed * deltaTime;
            }

            currentVelocity += gravity * deltaTime;
            currentVelocity *= 1f / (1f + drag * deltaTime);
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            Vector3 lookDirection = GetLookDirection();
            
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, characterMotor.CharacterUp);
                currentRotation = Quaternion.Slerp(currentRotation, targetRotation, deltaTime * rotationSharpness);
            }
        }

        private Vector3 GetLookDirection()
        {
            if (isFacingTarget)
                return targetDirection;
            
            if (isDashing)
                return dashDirection;
            
            return cameraRelativeMoveInput;
        }
        
        public void SetRotationToTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
    
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, characterMotor.CharacterUp);
                characterMotor.RotateCharacter(targetRotation);
                
                isFacingTarget = true;
                targetDirection = direction.normalized;
            }
        }
        
        public void ClearRotationToTarget()
        {
            isFacingTarget = false;
        }
        
        public void SetAttacking(bool attacking)
        {
            isAttacking = attacking;
            
            if (attacking)
            {
                moveInputVector = Vector3.zero;
                cameraRelativeMoveInput = Vector3.zero;

                characterMotor.ForceUnground();
            }
        }

        public void ResetAnimation()
        {
            if (animator != null)
                animator.SetTrigger(ResetTriggerHash);
        }
        
        // ICharacterController interface implementations
        public void BeforeCharacterUpdate(float deltaTime) { }
        public void PostGroundingUpdate(float deltaTime) { }
        public void AfterCharacterUpdate(float deltaTime) { }
        public bool IsColliderValidForCollisions(Collider coll) => true;
        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
    }
}