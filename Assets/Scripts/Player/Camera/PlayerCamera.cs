using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform followTarget;
    
    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float gamepadSensitivity = 100f;
    
    [Header("Camera Settings")]
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private Vector2 verticalAngleLimit = new (-30f, 70f);
    [SerializeField] private float heightOffset = 1.5f;
    
    [Header("Look Ahead (when zoomed out)")]
    [SerializeField] private float maxLookAheadDistance = 3f;
    [SerializeField] private AnimationCurve lookAheadCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [Header("Distance Curve (Zoom based on angle)")]
    [SerializeField] private AnimationCurve distanceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    
    [Header("Smoothing")]
    [SerializeField] private bool useGamepadSmoothing = true;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float rotationSmoothing = 10f;
    
    private IInputHandler inputHandler;
    private float currentHorizontalAngle;
    private float currentVerticalAngle;
    private float targetHorizontalAngle;
    private float targetVerticalAngle;
    private Vector2 smoothedInput;
    private bool isUsingGamepad;
    
    private void Start()
    {
        inputHandler = InputManager.Instance.InputHandler;
        
        if (followTarget == null)
        {
            Debug.LogError("Follow target not assigned!");
            return;
        }
        
        currentHorizontalAngle = followTarget.eulerAngles.y;
        currentVerticalAngle = 20f;
        targetHorizontalAngle = currentHorizontalAngle;
        targetVerticalAngle = currentVerticalAngle;
        
        virtualCamera.Follow = followTarget;
    }
    
    private void Update()
    {
        if (inputHandler == null || followTarget == null) 
            return;
        
        Vector2 lookInput = inputHandler.LookInput;
        
        isUsingGamepad = Gamepad.current != null && Gamepad.current.rightStick.IsActuated();
        
        ProcessRotationInput(lookInput);
    }
    
    private void LateUpdate()
    {
        if (followTarget == null) 
            return;
        
        currentHorizontalAngle = Mathf.Lerp(
            currentHorizontalAngle, 
            targetHorizontalAngle, 
            rotationSmoothing * Time.deltaTime
        );
        
        currentVerticalAngle = Mathf.Lerp(
            currentVerticalAngle, 
            targetVerticalAngle, 
            rotationSmoothing * Time.deltaTime
        );
        
        UpdateCameraTransform();
    }
    
    private void ProcessRotationInput(Vector2 lookInput)
    {
        if (lookInput.sqrMagnitude < 0.001f)
        {
            smoothedInput = Vector2.zero;
            return;
        }
        
        float sensitivity = isUsingGamepad ? gamepadSensitivity : mouseSensitivity;
        float deltaMultiplier = isUsingGamepad ? Time.deltaTime : 1f;
        
        Vector2 input = lookInput;
        if (useGamepadSmoothing && isUsingGamepad)
        {
            smoothedInput = Vector2.Lerp(smoothedInput, lookInput, smoothSpeed * Time.deltaTime);
            input = smoothedInput;
        }
        else if (!isUsingGamepad)
        {
            smoothedInput = Vector2.zero;
        }
        
        // Update target angles
        targetHorizontalAngle += input.x * sensitivity * deltaMultiplier;
        targetVerticalAngle -= input.y * sensitivity * deltaMultiplier; // Inverted Y
        
        // Clamp vertical angle
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, verticalAngleLimit.x, verticalAngleLimit.y);
    }
    
    private void UpdateCameraTransform()
    {
        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        
        // Calculate distance based on vertical angle (BG3-style zoom)
        // Normalize vertical angle to 0-1 range
        float normalizedAngle = Mathf.InverseLerp(verticalAngleLimit.x, verticalAngleLimit.y, currentVerticalAngle);
        float currentDistance = Mathf.Lerp(minDistance, maxDistance, distanceCurve.Evaluate(normalizedAngle));
        
        // Calculate look-ahead offset (look further ahead when zoomed out and looking down)
        // Inverse of normalized angle: more look-ahead when looking down (lower angle)
        float lookAheadAmount = lookAheadCurve.Evaluate(1f - normalizedAngle);
        float currentLookAhead = lookAheadAmount * maxLookAheadDistance;
        
        // Get forward direction based on horizontal rotation only (not vertical)
        Vector3 horizontalForward = Quaternion.Euler(0, currentHorizontalAngle, 0) * Vector3.forward;
        
        // Calculate target position with look-ahead
        Vector3 targetPosition = followTarget.position + Vector3.up * heightOffset;
        Vector3 lookAtPosition = targetPosition + horizontalForward * currentLookAhead;
        
        // Calculate position offset from look-at point
        Vector3 offset = rotation * new Vector3(0, 0, -currentDistance);
        
        // Set camera position and make it look at the offset position
        virtualCamera.transform.position = lookAtPosition + offset;
        virtualCamera.transform.LookAt(lookAtPosition);
    }
    
    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
        virtualCamera.Follow = target;
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
    
    public void SetGamepadSensitivity(float sensitivity)
    {
        gamepadSensitivity = sensitivity;
    }
    
    public void SetDistance(float minDist, float maxDist)
    {
        minDistance = minDist;
        maxDistance = maxDist;
    }
    
    public void ResetRotation()
    {
        if (followTarget != null)
        {
            currentHorizontalAngle = followTarget.eulerAngles.y;
            currentVerticalAngle = 20f;
            targetHorizontalAngle = currentHorizontalAngle;
            targetVerticalAngle = currentVerticalAngle;
        }
    }
}