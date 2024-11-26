using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxForwardSpeed;    // Speed moving forward
    [SerializeField] private float _maxBackwardSpeed;   // Speed moving backward
    [SerializeField] private float _maxStrafeSpeed;     // Speed strafing left/right
    [SerializeField] private float _maxLookUpAngle;     // Max angle for looking up
    [SerializeField] private float _maxLookDownAngle;   // Max angle for looking down

    private CharacterController _controller;            // Handles player movement
    private Transform _head;                            // Reference to camera (head)
    private Vector3 _headRotation;                      // Tracks camera rotation
    private Vector3 _velocity;                          // Player movement speed
    private Vector3 _motion;                            // Final motion applied

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _head = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;       // Lock the cursor to the screen
    }

    void Update()
    {
        UpdateRotation();    // Player rotation
        UpdateHead();        // Camera (head) rotation
    }

    private void UpdateRotation()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);
    }

    private void UpdateHead()
    {
        _headRotation = _head.localEulerAngles;
        _headRotation.x -= Input.GetAxis("Mouse Y");

        // Clamp the camera's up/down angles
        if (_headRotation.x > 180f)
            _headRotation.x = Mathf.Max(_maxLookUpAngle, _headRotation.x);
        else
            _headRotation.x = Mathf.Min(_maxLookDownAngle, _headRotation.x);

        _head.localEulerAngles = _headRotation;
    }

    void FixedUpdate()
    {
        UpdateVelocity();    // Calculate movement
        UpdatePosition();    // Apply movement
    }

    private void UpdateVelocity()
    {
        // Movement input and speed calculations
        float forwardAxis = Input.GetAxis("Vertical");
        float strafeAxis = Input.GetAxis("Horizontal");

        _velocity.z = forwardAxis >= 0f 
                      ? forwardAxis * _maxForwardSpeed 
                      : forwardAxis * _maxBackwardSpeed;

        _velocity.x = strafeAxis * _maxStrafeSpeed;

        // Cap movement speed
        if (_velocity.magnitude > _maxForwardSpeed)
            _velocity = _velocity.normalized * _maxForwardSpeed;
    }

    private void UpdatePosition()
    {
        _motion = transform.TransformVector(_velocity * Time.fixedDeltaTime);
        _controller.Move(_motion);
    }
}

