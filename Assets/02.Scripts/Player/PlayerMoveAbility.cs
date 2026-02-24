using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    
    private const float GRAVITY = 30f;
    private float _yVeocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    public bool IsSprinting { get; private set; }
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _owner.Stat.CurrentStamina = _owner.Stat.MaxStamina;
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
        IsSprinting = isMoving && trySprint && _owner.Stat.CurrentStamina > 0f;

        if (IsSprinting)
        {
            _owner.Stat.CurrentStamina -= _owner.Stat.StaminaDrainRate * Time.deltaTime;
            _owner.Stat.CurrentStamina = Mathf.Max(_owner.Stat.CurrentStamina, 0f);
        }
        else
        {
            _owner.Stat.CurrentStamina += _owner.Stat.StaminaRecoveryRate * Time.deltaTime;
            _owner.Stat.CurrentStamina = Mathf.Min(_owner.Stat.CurrentStamina, _owner.Stat.MaxStamina);
        }
        
        direction = Camera.main.transform.TransformDirection(direction);

        _animator.SetFloat("Move", direction.magnitude);

        _yVeocity -= GRAVITY * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = _owner.Stat.JumpPower;
        }

        float speed = _owner.Stat.MoveSpeed;
        if (IsSprinting)
        {
            speed *= _owner.Stat.SprintSpeedMultiplier;
        }

        direction *= speed;
        direction.y = _yVeocity;
        _characterController.Move(direction * Time.deltaTime);
    }
}