using UnityEngine;
using DG.Tweening;
using static UnityEditor.Progress;

[RequireComponent( typeof( Rigidbody ) )]
public abstract class Item : MonoBehaviour
{
    public IRaycastAction ItemRaycastAction { get => m_ItemRaycastAction; }
    public ItemData Data { get => m_Data; }

    [SerializeField] protected ItemData m_Data;

    protected IRaycastAction m_ItemRaycastAction;
    protected TileManager m_TileManager;
    protected bool m_IsDropState;

    //Drop Vars
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshRenderer m_MeshRenderer;
    private bool m_IsGrounded;
    private static int m_ShaderFloatProperty = Shader.PropertyToID( "_Float" );
    private MaterialPropertyBlock m_Mpb;
    protected void Awake()
    {
        m_Mpb = new();
    }
    protected void OnEnable()
    {
        TryGetComponent( out m_ItemRaycastAction );
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_Rigidbody.isKinematic = true;
        m_Mpb.SetFloat( m_ShaderFloatProperty, 0 );
        m_MeshRenderer.SetPropertyBlock( m_Mpb );
    }
    public abstract void MainAction();
    protected void Update()
    {
        if ( !m_IsDropState ) return;
        if ( !m_IsGrounded && CheckGround() )
        {
            m_Collider.enabled = true;
            m_IsGrounded = true;
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.useGravity = false;
            m_Collider.enabled = true;
            float posY = transform.localPosition.y + .15f;
            m_MeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //if ( !m_RotateTweener.IsPlaying() )
            //{
            //    m_RotateTweener.Play();
            //    transform.DOLocalMoveY( posY, 1f ).SetLoops( -1, LoopType.Yoyo ).SetEase( Ease.InOutSine );
            //}
            return;
        }

    }

    private void OnTriggerEnter( Collider other )
    {
        if ( m_IsGrounded && other.TryGetComponent( out PlayerAction playerAction ) )
        {
            playerAction.Store( this );
        }
    }

    public void DropState()
    {
        if ( m_Collider.GetType() == typeof( MeshCollider ) )
        {
            MeshCollider meshCollider = ( MeshCollider ) m_Collider;
            meshCollider.convex = true;
        }
        
        if ( transform.TryGetComponent( out Hoverable hoverable ) )
        {
            hoverable.IsHoverable = false;
        }

        m_Mpb.SetFloat( m_ShaderFloatProperty, 1 );
        m_Collider.enabled = false;
        m_Collider.isTrigger = true;
        m_Rigidbody.isKinematic = false;
        m_MeshRenderer.SetPropertyBlock( m_Mpb );
        m_IsDropState = true;
        transform.SetParent( null );

            
        transform.localScale = Vector3.one * .25f;
        m_Rigidbody.AddForce( 10f * Camera.main.transform.forward, ForceMode.VelocityChange );

    }
    public virtual void SetHandTf()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one / 2;
        transform.localRotation = Quaternion.identity;
        transform.gameObject.layer = Utils.HandLayer;
        transform.forward = -transform.forward;
    }
    private bool CheckGround() => Physics.Raycast( transform.position, Vector3.down, .5f );
}

public enum ItemType
{
    Equipment,
    Ingredient,
    Furniture,
    Seed
}