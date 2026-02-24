using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    
    private const float GRAVITY = 9.8f;
    private float _yVeocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    private float _currentStamina;
    public bool IsSprinting { get; private set; }
    public float CurrentStamina => _currentStamina;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _currentStamina = _owner.Stat.MaxStamina;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    
        
        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();

        bool isMoving = direction.magnitude > 0.1f;
        bool trySprint = Input.GetKey(KeyCode.LeftShift);
        IsSprinting = isMoving && trySprint && _currentStamina > 0f;

        if (IsSprinting)
        {
            _currentStamina -= _owner.Stat.StaminaDrainRate * Time.deltaTime;
            _currentStamina = Mathf.Max(_currentStamina, 0f);
        }
        else
        {
            _currentStamina += _owner.Stat.StaminaRecoveryRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _owner.Stat.MaxStamina);
        }
        
        direction = Camera.main.transform.TransformDirection(direction);

        _animator.SetFloat("Move", direction.magnitude);

        _yVeocity -= GRAVITY * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = _owner.Stat.JumpPower;
        }
        
        direction.y = _yVeocity;

        float speed = _owner.Stat.MoveSpeed;
        if (IsSprinting)
        {
            speed *= _owner.Stat.SprintSpeedMultiplier;
        }
        
        _characterController.Move(direction * Time.deltaTime * speed);
    }
}