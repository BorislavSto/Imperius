using KinematicCharacterController;
using UnityEngine;
using Core;

namespace Player
{
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        private KinematicCharacterMotor characterMotor;
        private IInputHandler inputHandler;
        private Camera mainCamera;
        
        [Header("Movement Settings")]
        [SerializeField] private float maxStableMoveSpeed = 6f;
        [SerializeField] private float stableMovementSharpness = 15f;
        [SerializeField] private float maxAirMoveSpeed = 4f;
        [SerializeField] private float airAccelerationSpeed = 5f;
        [SerializeField] private float drag = 0.1f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -20f, 0);

        private Vector3 moveInputVector;

        private void Awake()
        {
            characterMotor = GetComponent<KinematicCharacterMotor>();
            inputHandler = InputManager.Instance.InputHandler;
            mainCamera = Camera.main;
            characterMotor.CharacterController = this;
        }
      
        private void Update()
        {
            /// used just for TEMP for testing
              if (inputHandler is null)
                inputHandler = InputManager.Instance.InputHandler;
            ///
            
            moveInputVector = new Vector3(inputHandler.MoveHorizontal, 0f, inputHandler.MoveVertical);

            Vector3 camForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
            Vector3 camRight   = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up).normalized;

            moveInputVector = (camForward * inputHandler.MoveVertical) + (camRight * inputHandler.MoveHorizontal);
            moveInputVector = Vector3.ClampMagnitude(moveInputVector, 1f);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (characterMotor.GroundingStatus.IsStableOnGround)
            {
                // Reorient velocity along slope
                currentVelocity = characterMotor.GetDirectionTangentToSurface(currentVelocity, characterMotor.GroundingStatus.GroundNormal) 
                                  * currentVelocity.magnitude;

                // Target velocity
                Vector3 inputRight = Vector3.Cross(moveInputVector, characterMotor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(characterMotor.GroundingStatus.GroundNormal, inputRight).normalized 
                                          * moveInputVector.magnitude;

                Vector3 targetVelocity = reorientedInput * maxStableMoveSpeed;

                // Smooth velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 
                    1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
            }
            else
            {
                if (moveInputVector.sqrMagnitude > 0f)
                {
                    Vector3 targetVelocity = moveInputVector * maxAirMoveSpeed;

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
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            Vector3 aimDir = inputHandler.GetAimDirection(transform.position);
            if (aimDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(aimDir, characterMotor.CharacterUp);
                currentRotation = Quaternion.Slerp(currentRotation, targetRot, deltaTime * 15f);
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll) => true;

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}
