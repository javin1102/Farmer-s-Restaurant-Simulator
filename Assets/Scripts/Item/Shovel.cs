using UnityEngine;
using UnityEngine.Events;
public class Shovel : Item, IRaycastAction
{
    private static Transform tileParent;
    [SerializeField] private GameObject m_TilePrefab;
    [SerializeField][Readonly] private GameObject m_PreviewTile;
    [SerializeField] private bool m_IsFarmGroundTag;
    [SerializeField] private bool m_Collided;


    private Mesh m_PreviewTileMesh;
    private Matrix4x4 m_TileMatrix;

    private void Start()
    {
        tileParent = GameObject.FindGameObjectWithTag( Utils.TILE_PARENT_TAG ).transform;
        m_PreviewTileMesh = m_TilePrefab.GetComponent<MeshFilter>().sharedMesh;
        m_TileManager = TileManager.instance;
    }
    private void OnDisable()
    {
        if ( m_PreviewTile != null ) m_PreviewTile.SetActive( false );
    }

    private void OnDestroy()
    {
        Destroy( m_PreviewTile );
        m_PreviewTile = null;
    }


    public override void MainAction()
    {
        if ( !m_IsFarmGroundTag || m_Collided ) return;

        GameObject tileCopyGO = Instantiate( m_TilePrefab, m_TileMatrix.MultiplyPoint3x4( Vector3.zero ), m_TileMatrix.rotation );
        tileCopyGO.name = "Tile";
        tileCopyGO.transform.parent = tileParent;

        MaterialChanger materialChanger = tileCopyGO.GetComponent<MaterialChanger>();
        materialChanger.ChangeToFinalMaterial();

        BoxCollider boxCollider = tileCopyGO.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        boxCollider.isTrigger = true;

        tileCopyGO.layer = 8;

    }

    public void PerformRaycastAction( RaycastHit hitInfo )
    {
        if ( hitInfo.collider == null ) return;
        m_IsFarmGroundTag = hitInfo.collider.CompareTag( Utils.FARM_GROUND_TAG );
        m_Collided = hitInfo.collider.CompareTag( Utils.TILE_TAG );
        MaterialChanger previewTileMaterialChanger = m_TilePrefab.GetComponent<MaterialChanger>();


        if ( m_IsFarmGroundTag )
        {
            Vector3 tilePos = m_TileManager.WorldToTilePos( hitInfo.point );
            tilePos.Set( tilePos.x, .11f, tilePos.z );
            Quaternion tileRot = Quaternion.Euler( 90f, 0, 0 );

            m_TileMatrix = Matrix4x4.TRS( tilePos, tileRot, Vector3.one );
            previewTileMaterialChanger.ChangePreviewMaterialColor( true );
            Graphics.DrawMesh( m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0 );
            return;
        }

        previewTileMaterialChanger.ChangePreviewMaterialColor( false );
        return;
    }



}
