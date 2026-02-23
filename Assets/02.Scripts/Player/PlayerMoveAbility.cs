using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpForce = 2.5f;
    
    private const float GRAVITY = 9.81f;
    private float _yVelocity = 0f;
    
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v);
        moveDir = transform.TransformDirection(moveDir);

        if (_characterController.isGrounded)
        {
            _yVelocity = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                _yVelocity = JumpForce;
            }
        }

        _yVelocity -= GRAVITY * Time.deltaTime;
        moveDir.y = _yVelocity;

        _characterController.Move(moveDir * MoveSpeed * Time.deltaTime);
    }
}
