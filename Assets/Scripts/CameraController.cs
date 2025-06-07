using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Properties — References
    [SerializeField, ReadOnly]
    private CameraControlActions _cameraActions;
    public CameraControlActions CameraActions
    {
        get => _cameraActions;
        private set => _cameraActions = value;
    }

    [SerializeField, ReadOnly]
    private InputAction _movement;
    public InputAction Movement
    {
        get => _movement;
        private set => _movement = value;
    }

    [SerializeField, ReadOnly]
    private Transform _cameraTransform;
    public Transform CameraTransform
    {
        get => _cameraTransform;
        private set => _cameraTransform = value;
    }
    #endregion

    #region Properties — Horizontal Motion
    [SerializeField]
    private float _maxSpeed = 5f;
    public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }
    [SerializeField]
    private float _currentSpeed;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }
    [SerializeField]
    private float _acceleration;
    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }
    [SerializeField]
    private float _damping;
    public float Damping
    {
        get => _damping;
        set => _damping = value; 
    }
    #endregion

    #region Properties — Rotation
    [SerializeField]
    private float _maxRotationSpeed = 1f;
    public float MaxRotationSpeed
    {
        get => _maxRotationSpeed;
        set => _maxRotationSpeed = value;
    }
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
