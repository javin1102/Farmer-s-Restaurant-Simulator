using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public abstract class Item : MonoBehaviour
{
    public IRaycastAction ItemRaycastAction { get => m_ItemRaycastAction; }
    public ItemData Data { get => m_Data; }
    public int DropQuantity { get => m_DropQuantity; set => m_DropQuantity = value; }
    public bool IsDropState { get => m_IsDropState; set => m_IsDropState = value; }
    [SerializeField] protected ItemData m_Data;
    [Space(5)]
    [Header("Hand Orientation")]
    [SerializeField] protected Vector3 m_HandPosOffset;
    [SerializeField] protected Vector3 m_HandEulerAngles;
    [SerializeField] protected Vector3 m_HandScale = new(.3f, .3f, .3f);
    protected IRaycastAction m_ItemRaycastAction;
    [Space(35)]
    [SerializeField] protected TileManager m_TileManager;
    private bool m_IsDropState;
    protected UIManager m_UIManager;

    //Drop Vars
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshRenderer m_MeshRenderer;
    private bool m_IsGrounded;
    private static readonly int m_ShaderFloatProperty = Shader.PropertyToID("_Float");
    private MaterialPropertyBlock m_Mpb;
    private int m_DropQuantity;
    private bool m_CanBeStored;


    protected void Awake()
    {
        m_Mpb = new();
    }
    protected void OnEnable()
    {
        TryGetComponent(out m_ItemRaycastAction);
        m_UIManager = UIManager.Instance;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_Mpb.SetFloat(m_ShaderFloatProperty, 0);
        if (TryGetComponent(out m_MeshRenderer)) m_MeshRenderer.SetPropertyBlock(m_Mpb);
        m_Rigidbody.isKinematic = true;
        m_TileManager = TileManager.instance;
    }
    public abstract void MainAction();
    protected void Update()
    {
        if (!m_IsDropState) return;
        if (!m_IsGrounded && CheckGround())
        {
            m_Collider.enabled = true;
            m_IsGrounded = true;
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.useGravity = false;
            m_Collider.enabled = true;
            m_Collider.isTrigger = true;
            m_Rigidbody.velocity = Vector3.zero;
            m_MeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            return;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_CanBeStored && m_IsGrounded && other.TryGetComponent(out PlayerAction playerAction))
        {
            playerAction.Store(this, m_DropQuantity);
        }
    }

    public void DropState(int dropQuantity = 1)
    {
        StartCoroutine(StoreCooldown());
        if (m_Collider.GetType() == typeof(MeshCollider))
        {
            MeshCollider meshCollider = (MeshCollider)m_Collider;
            Destroy(meshCollider);
            m_Collider = gameObject.AddComponent<BoxCollider>();
        }


        if (transform.TryGetComponent(out Hoverable hoverable))
        {
            hoverable.IsHoverable = false;
        }
        m_DropQuantity = dropQuantity;
        m_Mpb.SetFloat(m_ShaderFloatProperty, 1);
        m_Collider.enabled = true;
        m_Rigidbody.isKinematic = false;
        m_Collider.isTrigger = false;
        if (m_MeshRenderer) m_MeshRenderer.SetPropertyBlock(m_Mpb);
        m_IsDropState = true;
        transform.SetParent(null);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * .25f;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_Rigidbody.AddForce(10f * Camera.main.transform.forward + PlayerAction.Instance.transform.up * 3.25f, ForceMode.Impulse);
        Destroy(GetComponent<NavMeshObstacle>());
    }

    private IEnumerator StoreCooldown()
    {
        m_CanBeStored = false;
        yield return new WaitForSeconds(.2f);
        m_CanBeStored = true;
    }

    public virtual void SetHandTf()
    {
        transform.localPosition = m_HandPosOffset;
        transform.localScale = m_HandScale;
        transform.gameObject.layer = Utils.HandLayer;
        transform.localEulerAngles = m_HandEulerAngles;
    }


    private bool CheckGround() => Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, .2f, ~Utils.PlayerMask);

}

