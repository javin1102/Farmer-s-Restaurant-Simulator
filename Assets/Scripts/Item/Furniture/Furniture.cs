using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MaterialChanger))]
[RequireComponent(typeof(Hoverable))]
public abstract class Furniture : Item, IRaycastAction
{
    [SerializeField] private ItemMainActionChannel m_DecreaseableEvent;
    [SerializeField] private InputActionReference m_ObjRotationInputRef;

    protected bool m_IsInstantiable;
    protected Mesh m_PreviewMesh;
    protected GameObject m_InstantiatedGO;
    protected RestaurantManager m_Restaurant;
    [SerializeField] protected bool m_IsInstantiated;
    [SerializeField] protected Hoverable m_Hoverable;
    private Vector3 m_InstantiatedSize;
    private float m_ObjRot;
    private MaterialChanger m_MaterialChanger;
    private Matrix4x4 m_PreviewMatrix;
    protected new void OnEnable()
    {
        m_Restaurant = RestaurantManager.Instance;
        m_PreviewMesh = GetComponent<MeshFilter>().sharedMesh;
        m_Hoverable = GetComponent<Hoverable>();
        m_TileManager = TileManager.instance;
        base.OnEnable();
        m_ObjRotationInputRef.action.performed += RotateObj;
        m_InstantiatedSize = transform.localScale;
    }

    protected void OnDisable()
    {
        m_ObjRotationInputRef.action.performed -= RotateObj;
    }

    public override void MainAction()
    {
        if (!gameObject.activeInHierarchy || !m_IsInstantiable) return;
        m_DecreaseableEvent.RaiseEvent();
        Vector3 pos = m_PreviewMatrix.MultiplyPoint3x4(Vector3.zero);
        SpawnFurniture(pos, m_PreviewMatrix.rotation, m_InstantiatedSize);
    }
    protected void OnDestroy()
    {
        if (m_IsInstantiated)
        {
            m_Hoverable.OnHoverEnter -= ShowHelper;

        }
    }
    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (m_Restaurant == null) m_Restaurant = RestaurantManager.Instance;
        if (m_MaterialChanger == null) m_MaterialChanger = GetComponent<MaterialChanger>();
        if (hitInfo.collider.CompareTag(Utils.RESTAURANT_GROUND_TAG))
        {

            Vector3 objPos = m_TileManager.WorldToTilePos(hitInfo.point) + Vector3.up * m_PreviewMesh.bounds.size.y / 2;
            Quaternion objRotation = Quaternion.Euler(0, m_ObjRot, 0);

            m_PreviewMatrix = Matrix4x4.TRS(objPos, objRotation, m_InstantiatedSize);
            Collider[] colliders = Physics.OverlapBox(objPos, Vector3.Scale(m_PreviewMesh.bounds.size / 2 - Vector3.one * .01f, m_InstantiatedSize), objRotation, Utils.RaycastableMask);
            bool m_Collided = false;

            foreach (var collider in colliders)
            {
                m_Collided = collider.CompareTag(Utils.PROP_TAG);
            }
            //bool isNotInsideBox = hitInfo.point.x < m_Restaurant.GroundCollider2.bounds.min.x || hitInfo.point.x > m_Restaurant.GroundCollider2.bounds.max.x || hitInfo.point.z < m_Restaurant.GroundCollider2.bounds.min.z || hitInfo.point.z > m_Restaurant.GroundCollider2.bounds.max.z;
            if (m_Collided || !m_Restaurant.GroundCollider2.bounds.Contains(hitInfo.point))
            {
                m_IsInstantiable = false;
            }
            else
            {
                Graphics.DrawMesh(m_PreviewMesh, m_PreviewMatrix, m_MaterialChanger.PreviewMaterial, 0);
                m_IsInstantiable = true;
            }
            return;
        }

        m_IsInstantiable = false;


    }
    private void RotateObj(InputAction.CallbackContext context)
    {
        m_ObjRot += context.ReadValue<float>() * 90;
    }

    protected virtual void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary("Left", "Simpan");
    }

    public void SpawnFurniture(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        m_UIManager = UIManager.Instance;
        m_Restaurant = RestaurantManager.Instance;
        m_InstantiatedGO = Instantiate(m_Data.prefab, position, rotation);
        m_InstantiatedGO.transform.localScale = localScale;
        m_InstantiatedGO.name = m_Data.ID;
        m_InstantiatedGO.layer = 8;
        m_InstantiatedGO.SetActive(true);
        m_InstantiatedGO.transform.SetParent(null);
        m_InstantiatedGO.GetComponent<Collider>().enabled = true;
        Furniture furniture = m_InstantiatedGO.GetComponent<Furniture>();
        furniture.m_IsInstantiated = true;
        furniture.m_ObjRotationInputRef.action.performed -= furniture.RotateObj;
        furniture.m_Hoverable.OnHoverEnter += ShowHelper;
        OnInstantiate();
    }
    public void SpawnFurniture(Vector3 position, Vector3 eulerAngles, Vector3 localScale) =>
    SpawnFurniture(position, Quaternion.Euler(eulerAngles), localScale);

    protected abstract void OnInstantiate();





}
