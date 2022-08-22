using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( CharacterController ) )]
public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField][Range( 10, 25 )] private float m_MoveSpeed;
    [SerializeField] private float m_MouseHorizontalSensitivity;
    [SerializeField] private float m_JumpForce;
    [SerializeField][Readonly] private bool m_IsGrounded;

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private InputAction m_RotateAction;
    private InputAction m_JumpAction;

    private CharacterController m_CharacterController;

    private float m_JumpVelocity;
    private float m_Gravity;
    private int m_GroundMask;


    void Start()
    {
        m_GroundMask = Utils.FarmGroundMask | Utils.RestaurantGroundMask | Utils.GroundMask;

        m_PlayerInput = GetComponent<PlayerInput>();
        m_RotateAction = m_PlayerInput.actions[Utils.PLAYER_ROTATION_ACTION];
        m_MoveAction = m_PlayerInput.actions[Utils.MOVE_ACTION];
        m_JumpAction = m_PlayerInput.actions[Utils.JUMP_ACTION];

        m_CharacterController = GetComponent<CharacterController>();
        m_Gravity = Physics.gravity.y;
    }

    void Update()
    {
        //Move in X and Z axis
        Vector2 moveValue = m_MoveAction.ReadValue<Vector2>();
        Vector3 moveZ = transform.forward * ( moveValue.y * Time.deltaTime * m_MoveSpeed );
        Vector3 moveX = transform.right * ( moveValue.x * Time.deltaTime * m_MoveSpeed );
        m_CharacterController.Move( moveZ + moveX );


        //Rotate around Y Axis
        //Mouse sensitivity only apply for rot around Y
        Vector2 rotationValue = m_RotateAction.ReadValue<Vector2>() * ( m_MouseHorizontalSensitivity * Time.deltaTime );
        transform.Rotate( Vector3.up * rotationValue.x );

        //Jump
        m_IsGrounded = Physics.Raycast( transform.position, Vector3.down, 1.1f, m_GroundMask );
        if ( m_IsGrounded && m_JumpVelocity <= 0 )
        {
            m_JumpVelocity = 0;
        }

        if ( m_JumpAction.triggered && m_IsGrounded )
        {
            m_JumpVelocity += Mathf.Sqrt( m_JumpForce * -3.0f * m_Gravity * Time.deltaTime );
        }

        m_JumpVelocity += m_Gravity * Time.deltaTime;
        m_CharacterController.Move( Vector3.up * ( m_JumpVelocity * Time.deltaTime ) );

    }
}