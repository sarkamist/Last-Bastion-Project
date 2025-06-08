using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Properties — References
    [Header("References")]
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

    [SerializeField, ReadOnly]
    private Vector3 _targetPosition;
    public Vector3 TargetPosition
    {
        get => _targetPosition;
        private set => _targetPosition = value;
    }
    #endregion

    #region Properties — Horizontal Motion
    [Header("Horizontal")]
    [SerializeField]
    private float _maxSpeed = 5f;
    public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }

    [SerializeField, ReadOnly]
    private float _currentSpeed;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    [SerializeField]
    private float _acceleration = 10f;
    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    [SerializeField]
    private float _damping = 15f;
    public float Damping
    {
        get => _damping;
        set => _damping = value;
    }
    #endregion

    #region Properties — Rotation
    [Header("Rotation")]
    [SerializeField]
    private float _maxRotationSpeed = 1f;
    public float MaxRotationSpeed
    {
        get => _maxRotationSpeed;
        set => _maxRotationSpeed = value;
    }
    #endregion

    #region Properties — Edge Motion
    [Header("Edge Motion")]
    [SerializeField]
    private float _edgeTolerance = 0.05f;
    public float EdgeTolerance
    {
        get => _edgeTolerance;
        set => _edgeTolerance = value;
    }

    [SerializeField]
    private bool _useScreenEdge = true;
    public bool UseScreenEdge
    {
        get => _useScreenEdge;
        set => _useScreenEdge = value;
    }
    #endregion

    #region Properties — Zoom Motion
    [Header("Zoom Motion")]
    [SerializeField]
    private float _stepSize = 2f;
    public float StepSize
    {
        get => _stepSize;
        set => _stepSize = value;
    }

    [SerializeField]
    private float _zoomDampening = 7.5f;
    public float ZoomDampening
    {
        get => _zoomDampening;
        set => _zoomDampening = value;
    }

    [SerializeField]
    private float _minHeight = 5f;
    public float MinHeight
    {
        get => _minHeight;
        set => _minHeight = value;
    }

    [SerializeField]
    private float _maxHeight = 50f;
    public float MaxHeight
    {
        get => _maxHeight;
        set => _maxHeight = value;
    }

    [SerializeField]
    private float _zoomSpeed = 2f;
    public float ZoomSpeed
    {
        get => _zoomSpeed;
        set => _zoomSpeed = value;
    }

    [SerializeField, ReadOnly]
    private float _zoomHeight;
    public float ZoomHeight
    {
        get => _zoomHeight;
        set => _zoomHeight = value;
    }

    [Header("Miscellaneous")]
    [SerializeField]
    private Vector3 _horizontalVelocity;
    public Vector3 HorizontalVelocity
    {
        get => _horizontalVelocity;
        set => _horizontalVelocity = value;
    }

    [SerializeField]
    private Vector3 _lastPosition;
    public Vector3 LastPosition
    {
        get => _lastPosition;
        set => _lastPosition = value;
    }

    [SerializeField]
    private Vector3 _startDrag;
    public Vector3 StartDrag
    {
        get => _startDrag;
        set => _startDrag = value;
    }
    #endregion

    void Awake()
    {
        CameraActions = new CameraControlActions();
        CameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        ZoomHeight = CameraTransform.localPosition.y;
        CameraTransform.LookAt(transform);

        LastPosition = transform.position;

        Movement = CameraActions.Camera.Movement;
        CameraActions.Camera.Rotate.performed += RotateCamera;
        CameraActions.Camera.Zoom.performed += ZoomCamera;
        CameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        CameraActions.Camera.Rotate.performed -= RotateCamera;
        CameraActions.Camera.Zoom.performed -= ZoomCamera;
        CameraActions.Camera.Disable();
    }

    void Update()
    {
        GetKeyboardMovement();
        CheckMouseAtScreenEdge();
        DragCamera();

        UpdateVelocity();
        UpdateBasePosition();
        UpdateCameraPosition();
    }

    private void UpdateVelocity()
    {
        Vector3 velocity = (transform.position - LastPosition) / Time.deltaTime;
        velocity.y = 0f;
        HorizontalVelocity = velocity;
        LastPosition = transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 input =
            Movement.ReadValue<Vector2>().x * GetCameraRight()
            + Movement.ReadValue<Vector2>().y * GetCameraForward();

        input = input.normalized;

        if (input.sqrMagnitude > 0.1f) TargetPosition += input;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed) return;

        //Raycast into plane
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                StartDrag = ray.GetPoint(distance);
            }
            else
            {
                TargetPosition += StartDrag - ray.GetPoint(distance);
            }
        }
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        //Horizontal Scrolling
        if (mousePosition.x < EdgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - EdgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        //Vertical Scrolling
        if (mousePosition.y < EdgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - EdgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        TargetPosition += moveDirection;
    }

    private void UpdateBasePosition()
    {
        if (TargetPosition.sqrMagnitude > 0.1f)
        {
            //Accelerate camera as you keep moving
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, Time.deltaTime * Acceleration);
            transform.position += TargetPosition * CurrentSpeed * Time.deltaTime;
        }
        else
        {
            //Slow down smoothly
            HorizontalVelocity = Vector3.Lerp(HorizontalVelocity, Vector3.zero, Time.deltaTime * Damping);
            transform.position += HorizontalVelocity * Time.deltaTime;
        }

        TargetPosition = Vector3.zero;
    }

    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        float inputValue = -obj.ReadValue<Vector2>().y;

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            ZoomHeight = CameraTransform.localPosition.y + inputValue * StepSize;

            if (ZoomHeight < MinHeight)
            {
                ZoomHeight = MinHeight;
            }
            else if (ZoomHeight > MaxHeight)
            {
                ZoomHeight = MaxHeight;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(CameraTransform.localPosition.x, ZoomHeight, CameraTransform.localPosition.z);

        //Vector for forward/backward zoom
        zoomTarget -= ZoomSpeed * (ZoomHeight - CameraTransform.localPosition.y) * Vector3.forward;

        CameraTransform.localPosition = Vector3.Lerp(CameraTransform.localPosition, zoomTarget, Time.deltaTime * ZoomDampening);
        CameraTransform.LookAt(this.transform);
    }

    private void RotateCamera(InputAction.CallbackContext obj)
    {
        if (!Mouse.current.middleButton.isPressed) return;

        float inputValue = obj.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, inputValue * MaxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = CameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = CameraTransform.right;
        right.y = 0f;
        return right;
    }
}
