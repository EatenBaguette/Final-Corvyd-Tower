using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    private CharacterController _controller;
    [SerializeField] private Transform _cameraTransform;
    private Camera _camera;

    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _sprintMultiplier = 2.0f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _rotSpeed = 5.0f;
    [SerializeField] private float _jumpSpeed = 15.0f;
    [SerializeField] float _terminalVelocity = -20.0f;
    [SerializeField] float _minimumFall = -1.5f;

    float _verticalSpeed;
    float _groundCheckDistance;
    private ControllerColliderHit _contact;

    [SerializeField] private bool _isJumping = false;
    

    [SerializeField] private float _sensitivityHor = 9.0f;
    [SerializeField] private float _sensitivityVert = 9.0f;
    [SerializeField] private float _minimumVerticalAngle = -45.0f;
    [SerializeField] private float _maximumVerticalAngle = 45.0f;

    private float _verticalRotation = 0f;

    private Animator _animator;

    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public bool Grounded = false;
    
    [Space(10)]
    [Header("Health Properties")]
    public int Health = 5;
    public int MaxHealth = 10;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
        _animator = GetComponent<Animator>();

        _states = new PlayerStateFactory(this);

        _currentState = _states.Alive;

        _currentState.EnterState();
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.freezeRotation = true;  
        
        _animator = GetComponent<Animator>();

        _verticalSpeed = _minimumFall;
        
        _controller = GetComponent<CharacterController>();
        _controller.radius = 0.4f;

        _groundCheckDistance =
            (_controller.height + _controller.radius) /
            _controller.height * 0.95f;
        
    }

    void Update()
    {
        _currentState.UpdateState();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    public bool CheckInput()
    {
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        
        if (horInput != 0 || vertInput != 0)
        {
            return true;
        }
        
        if (Mathf.Approximately(horInput, 0f) && Mathf.Approximately(vertInput, 0f))
        {
            return false;
        }

        return false;
    }

    public bool CheckJump()
    {
        return _isJumping;
    }

    public bool CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }

        return false;
    }
    

    public void HandleIdle()
    {
        bool hitGround = false;
        _verticalSpeed = _minimumFall;
        
        Vector3 movement = Vector3.zero;
        
        hitGround = false;
        
        if (_verticalSpeed < 0 && Physics.Raycast(
                transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _isJumping = true;
                _verticalSpeed = _jumpSpeed;
            }
            else
            {
                _verticalSpeed = _minimumFall;
            }
        }
        else
        {
            _verticalSpeed += _gravity * 5f * Time.deltaTime;

            if (_verticalSpeed < _terminalVelocity)
            {
                _verticalSpeed = _terminalVelocity;
            }

            if (_controller.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * _moveSpeed;
                }
                else
                {
                    movement += _contact.normal * _moveSpeed;
                }
            }
        }
        movement.y = _verticalSpeed;
        _controller.Move(movement * Time.deltaTime);
    }

    public void HandleJump()
    {
        bool hitGround = false;
        
        _verticalSpeed = _minimumFall;
        Vector3 movement = Vector3.zero;
        
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = _cameraTransform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);

            movement = (right * horInput) + (forward * vertInput);
            movement *= _moveSpeed;
            movement = Vector3.ClampMagnitude(movement, _moveSpeed);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, direction, _rotSpeed * Time.deltaTime);
        }
        
        if (_verticalSpeed < 0 && Physics.Raycast(
                transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }

        if (hitGround)
        {
            _isJumping = false;
        }
        else
        {
            _isJumping = true;
            
            _verticalSpeed = _jumpSpeed;
        
            _verticalSpeed += _gravity * 5f * Time.deltaTime;

            if (_verticalSpeed < _terminalVelocity)
            {
                _verticalSpeed = _terminalVelocity;
            }

            if (_contact != null)
            {
                _animator.SetBool("isJumping", true);
            }
        }
        
        movement.y = _verticalSpeed;
        _controller.Move(movement * Time.deltaTime);
    }

    public void HandleWalk()
    {
        bool hitGround = false;
        Vector3 movement = Vector3.zero;
        
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = _cameraTransform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            
            movement = (right * horInput) + (forward * vertInput);
            movement *= _moveSpeed;
            movement = Vector3.ClampMagnitude(movement, _moveSpeed);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, direction, _rotSpeed * Time.deltaTime);
        }
        
        if (_verticalSpeed < 0 && Physics.Raycast(
                transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _isJumping = true;
            }
            else
            {
                _verticalSpeed = _minimumFall;
            }
        }
        else
        {
            _verticalSpeed += _gravity * 5f * Time.deltaTime;

            if (_verticalSpeed < _terminalVelocity)
            {
                _verticalSpeed = _terminalVelocity;
            }

            if (_controller.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * _moveSpeed;
                }
                else
                {
                    movement += _contact.normal * _moveSpeed;
                }
            }
        }
        
        movement.y = _verticalSpeed;
        _controller.Move(movement * Time.deltaTime);
    }

    public void HandleRun()
    {
        bool hitGround = false;
        Vector3 movement = Vector3.zero;
        
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = _cameraTransform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            
            movement = (right * horInput) + (forward * vertInput);
            movement *= _moveSpeed;
            movement = Vector3.ClampMagnitude(movement, _moveSpeed);
            movement *= _sprintMultiplier;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, direction, _rotSpeed * Time.deltaTime);
        }
        
        if (_verticalSpeed < 0 && Physics.Raycast(
                transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _isJumping = true;
            }
            else
            {
                _verticalSpeed = _minimumFall;
            }
        }
        else
        {
            _verticalSpeed += _gravity * 5f * Time.deltaTime;

            if (_verticalSpeed < _terminalVelocity)
            {
                _verticalSpeed = _terminalVelocity;
            }

            if (_controller.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * _moveSpeed;
                }
                else
                {
                    movement += _contact.normal * _moveSpeed;
                }
            }
        }

        movement.y = _verticalSpeed;
        _controller.Move(movement * Time.deltaTime);
    }

    public void HandleRotation()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * _sensitivityHor, 0);
        _verticalRotation -= Input.GetAxis("Mouse Y") * _sensitivityVert;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minimumVerticalAngle, _maximumVerticalAngle);

        float horizontalRotation = transform.localEulerAngles.y;

        _cameraTransform.localEulerAngles = new Vector3(
            _verticalRotation, horizontalRotation, 0);
    }
    
    
    public void RelativeMovement()
    {
        Vector3 movement = Vector3.zero;
        
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = _cameraTransform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            
            movement = (right * horInput) + (forward * vertInput);
            movement *= _moveSpeed;
            movement = Vector3.ClampMagnitude(movement, _moveSpeed);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, direction, _rotSpeed * Time.deltaTime);
        }

        bool hitGround = false;
        if (_verticalSpeed < 0 && Physics.Raycast(
                transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _verticalSpeed = _jumpSpeed;
            }
            else
            {
                _verticalSpeed = _minimumFall;
                _animator.SetBool("isJumping", false);
            }
        }
        else
        {
            _verticalSpeed += _gravity * Time.deltaTime;

            if (_verticalSpeed < _terminalVelocity)
            {
                _verticalSpeed = _terminalVelocity;
            }

            if (_contact != null)
            {
                _animator.SetBool("isJumping", true);
            }

            if (_controller.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * _moveSpeed;
                }
                else
                {
                    movement += _contact.normal * _moveSpeed;
                }
            }
        }

        movement.y = _verticalSpeed;
        _controller.Move(movement * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
    
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
