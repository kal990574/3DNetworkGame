using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 30f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    public bool IsSprinting { get; private set; }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
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
        IsSprinting = isMoving && trySprint && _owner.Stat.Stamina.Current > 0f;

        if (IsSprinting)
        {
            _owner.Stat.Stamina.Drain(_owner.Stat.Stamina.DrainRate * Time.deltaTime);
        }

        direction = Camera.main.transform.TransformDirection(direction);

        _animator.SetFloat("Move", direction.magnitude);

        _yVelocity -= GRAVITY * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)
            && _characterController.isGrounded
            && _owner.Stat.Stamina.TryConsume(_owner.Stat.Stamina.JumpCost))
        {
            _yVelocity = _owner.Stat.JumpPower;
        }

        float speed = _owner.Stat.MoveSpeed;
        if (IsSprinting)
        {
            speed *= _owner.Stat.SprintSpeedMultiplier;
        }

        direction *= speed;
        direction.y = _yVelocity;
        _characterController.Move(direction * Time.deltaTime);
    }
}