using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpForce = 2.5f;
    
    private const float GRAVITY = 9.81f;
    private float _yVelocity = 0f;
    private Transform _cameraTransform;
    
    private CharacterController _characterController;
    
    private Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDir = forward * v + right * h;

        if (_characterController.isGrounded)
        {
            _yVelocity = 0f;

            if (Input.GetButton("Jump"))
            {
                _yVelocity = JumpForce;
            }
        }

        _yVelocity -= GRAVITY * Time.deltaTime;
        moveDir.y = _yVelocity;

        _characterController.Move(moveDir * MoveSpeed * Time.deltaTime);
        
        _animator.SetFloat("Move", moveDir.magnitude);
    }
}
