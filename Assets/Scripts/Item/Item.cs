using UnityEngine;
using DG.Tweening;
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
    private Tweener m_RotateTweener;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private bool m_IsGrounded;
    protected void OnEnable()
    {
        TryGetComponent( out m_ItemRaycastAction );
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_Rigidbody.isKinematic = true;
    }
    public abstract void MainAction();
    protected void Update()
    {
        if ( !m_IsDropState ) return;
        if ( !m_IsGrounded && CheckGround() )
        {
            m_IsGrounded = true;
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.useGravity = false;
            m_Collider.enabled = true;
            m_Collider.isTrigger = true;
            if ( !m_RotateTweener.IsPlaying() ) m_RotateTweener.Play();
            return;
        }

    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.TryGetComponent( out PlayerAction playerAction ) )
        {
            playerAction.Store( this );
        }
    }

    public void DropState()
    {
        m_Rigidbody.isKinematic = false;
        m_RotateTweener = transform.DOLocalRotate( new Vector3( 0, 360, 0 ), 1, RotateMode.FastBeyond360 ).SetLoops( -1 ).SetRelative( true ).SetEase( Ease.Linear );
        m_RotateTweener.Pause();
        m_IsDropState = true;
        transform.SetParent( null );
        transform.localScale = Vector3.one * .25f;
        m_Rigidbody.AddForce( 10f * Camera.main.transform.forward, ForceMode.VelocityChange );

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