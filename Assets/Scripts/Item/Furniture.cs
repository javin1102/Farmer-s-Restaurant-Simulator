using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Furniture : Item, IRaycastAction
{
    [SerializeField] private ItemMainActionChannel m_DecreaseableEvent;
    [SerializeField] private InputActionReference m_ObjRotationInputRef;
    private float m_ObjRot;
    private MaterialChanger m_MaterialChanger;
    private new MeshCollider collider;
    private Matrix4x4 m_PreviewMatrix;
    private Mesh m_PreviewMesh;
    private bool m_IsInstantiable;

    private void Start()
    {
        collider = GetComponent<MeshCollider>();
        m_PreviewMesh = GetComponent<MeshFilter>().sharedMesh;
    }

    private void Update()
    {
        // Item is selected by player
        if ( m_IsSelected )
            collider.enabled = false;
        else
            collider.enabled = true;
    }

    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        m_DecreaseableEvent.RaiseEvent();

        gameObject.name = "Table";
        gameObject.layer = 8;
        gameObject.SetActive( true );
        transform.SetParent( null );

        Vector3 pos = m_PreviewMatrix.MultiplyPoint3x4( Vector3.zero );
        pos.Set( pos.x, pos.y, pos.z );
        transform.SetPositionAndRotation( pos, m_PreviewMatrix.rotation );
        transform.localScale = Vector3.one;

        ResetProps();
    }

    public void PerformRaycastAction( RaycastHit hitInfo )
    {
        if ( m_MaterialChanger == null ) m_MaterialChanger = GetComponent<MaterialChanger>();
        if ( hitInfo.collider.CompareTag( Utils.RESTAURANT_GROUND_TAG ) )
        {
            Vector3 objPos = hitInfo.point + Vector3.up * .5f;

            float rotPressedValue = m_ObjRotationInputRef.action.ReadValue<float>();
            m_ObjRot += rotPressedValue * 40 * Time.deltaTime;
            Quaternion objRotation = Quaternion.Euler( 0, m_ObjRot, 0 );

            m_PreviewMatrix = Matrix4x4.TRS( objPos, objRotation, Vector3.one );

            bool m_Collided = Physics.CheckBox( objPos,
                transform.localScale / 2, objRotation, Utils.RaycastableMask | Utils.FarmGroundMask | Utils.GroundMask );

            if ( m_Collided )
            {
                m_MaterialChanger.ChangePreviewMaterialColor( false );
                m_IsInstantiable = false;
            }
            else {
                m_MaterialChanger.ChangePreviewMaterialColor( true );
                m_IsInstantiable = true;
            } 
            Graphics.DrawMesh( m_PreviewMesh, m_PreviewMatrix, m_MaterialChanger.PreviewMaterial, 0 );
            return;
        }

        m_IsInstantiable = false;


    }
    private void ResetProps()
    {
        m_IsSelected = false;
        m_IsInstantiable = false;
    }
}
