using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField][Range(5, 25)] private float m_MoveSpeed;
    [SerializeField] private float m_JumpForce;
    [SerializeField][Readonly] private bool m_IsGrounded;

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;

    private CharacterController m_CharacterController;

    private float m_JumpVelocity;
    private float m_Gravity;
    private Camera m_Cam;

    void Start()
    {

        m_PlayerInput = GetComponent<PlayerInput>();
        m_MoveAction = m_PlayerInput.actions[Utils.MOVE_ACTION];
        m_JumpAction = m_PlayerInput.actions[Utils.JUMP_ACTION];

        m_CharacterController = GetComponent<CharacterController>();
        m_Gravity = Physics.gravity.y;
        m_Cam = Camera.main;
    }

    void Update()
    {
        //Move in X and Z axis
        Vector2 moveValue = m_MoveAction.ReadValue<Vector2>();
        Vector3 moveZ = transform.forward * (moveValue.y * Time.deltaTime * m_MoveSpeed);
        Vector3 moveX = transform.right * (moveValue.x * Time.deltaTime * m_MoveSpeed);
        m_CharacterController.Move(moveZ + moveX);

        //Jump
        m_IsGrounded = Physics.Raycast(transform.position, Vector3.down, .85f, ~Utils.PlayerMask);




        if (m_JumpAction.triggered && m_IsGrounded) m_JumpVelocity += Mathf.Sqrt(m_JumpForce * -3.0f * m_Gravity);

        m_JumpVelocity += m_Gravity * Time.deltaTime;

        if (m_IsGrounded && m_JumpVelocity < 0) m_JumpVelocity = 0;
        m_CharacterController.Move(Vector3.up * (m_JumpVelocity * Time.deltaTime));

    }

    private void LateUpdate()
    {
        transform.eulerAngles = Vector3.up * m_Cam.transform.eulerAngles.y;
    }
}