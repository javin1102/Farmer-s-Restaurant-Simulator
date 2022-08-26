using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Furniture : Item, IRaycastAction
{
    [SerializeField] private ItemMainActionChannel m_DecreaseableEvent;
    [SerializeField] private InputActionReference m_ObjRotationInputRef;

    protected GameObject m_TempGO;
    private float m_ObjRot;
    private MaterialChanger m_MaterialChanger;
    private Matrix4x4 m_PreviewMatrix;
    private Mesh m_PreviewMesh;
    private bool m_IsInstantiable;


    private void Start()
    {
        m_PreviewMesh = GetComponent<MeshFilter>().sharedMesh;

    }

    private new void OnEnable()
    {
        base.OnEnable();
        m_ObjRotationInputRef.action.performed += RotateObj;
    }

    private void OnDisable()
    {
        m_ObjRotationInputRef.action.performed -= RotateObj;
    }

    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        m_DecreaseableEvent.RaiseEvent();

        m_TempGO = Instantiate( gameObject );
        m_TempGO.name = m_Data.id;
        m_TempGO.layer = 8;
        m_TempGO.SetActive( true );
        m_TempGO.transform.SetParent( null );

        Vector3 pos = m_PreviewMatrix.MultiplyPoint3x4( Vector3.zero );
        pos.Set( pos.x, pos.y, pos.z );
        m_TempGO.transform.SetPositionAndRotation( pos, m_PreviewMatrix.rotation );
        m_TempGO.transform.localScale = m_PreviewMatrix.lossyScale;
        m_TempGO.GetComponent<Collider>().enabled = true;
        ResetProps();
    }
    public void PerformRaycastAction( RaycastHit hitInfo )
    {
        if ( m_MaterialChanger == null ) m_MaterialChanger = GetComponent<MaterialChanger>();
        if ( hitInfo.collider.CompareTag( Utils.RESTAURANT_GROUND_TAG ) )
        {
            Vector3 objPos = m_TileManager.WorldToTilePos( hitInfo.point ) + Vector3.up * m_PreviewMesh.bounds.size.y / 2;

            //float rotPressedValue = m_ObjRotationInputRef.action.ReadValue<float>();
            //m_ObjRot += rotPressedValue * 40 * Time.deltaTime;
            Quaternion objRotation = Quaternion.Euler( 0, m_ObjRot, 0 );

            m_PreviewMatrix = Matrix4x4.TRS( objPos, objRotation, transform.localScale );

            bool m_Collided = Physics.CheckBox( objPos,
                m_PreviewMesh.bounds.size / 2 - Vector3.one * .01f, objRotation, Utils.RaycastableMask | Utils.FarmGroundMask | Utils.GroundMask | Utils.RestaurantMask );

            if ( m_Collided )
            {
                m_MaterialChanger.ChangePreviewMaterialColor( false );
                m_IsInstantiable = false;
            }
            else
            {
                m_MaterialChanger.ChangePreviewMaterialColor( true );
                m_IsInstantiable = true;
            }
            Graphics.DrawMesh( m_PreviewMesh, m_PreviewMatrix, m_MaterialChanger.PreviewMaterial, 0 );
            return;
        }

        m_IsInstantiable = false;


    }
    private void RotateObj( InputAction.CallbackContext context )
    {
        m_ObjRot += context.ReadValue<float>() * 90;
        print( m_ObjRot );
    }
    private void ResetProps()
    {
        m_IsInstantiable = false;
    }
}
