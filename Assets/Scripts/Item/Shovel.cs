using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Shovel : Item, IRaycastAction
{
    private static Transform tileParent;
    [SerializeField] private GameObject m_TilePrefab;
    [SerializeField][Readonly] private GameObject m_PreviewTile;
    [SerializeField] private bool m_IsFarmGroundTag;
    [SerializeField] private bool m_Collided;


    private Mesh m_PreviewTileMesh;
    private Matrix4x4 m_TileMatrix;
    MaterialChanger previewTileMaterialChanger;
    private new void Awake()
    {
        base.Awake();
        previewTileMaterialChanger = m_TilePrefab.GetComponent<MaterialChanger>();
    }
    private void Start()
    {
        tileParent = GameObject.FindGameObjectWithTag( Utils.TILE_PARENT_TAG ).transform;
        m_PreviewTileMesh = m_TilePrefab.GetComponent<MeshFilter>().sharedMesh;
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

        tileCopyGO.layer = 9;
        tileCopyGO.GetComponent<Tile>().SwitchStatus( Tile.TileStatus.HOED );

        // play Hoe sound effect
        // nanti harusnya pake animation event buat active deactive sfx
        StartCoroutine( PlaySFX() );
    }

    public void PerformRaycastAction( RaycastHit hitInfo )
    {
        if ( hitInfo.collider != null )
        {
            m_Collided = hitInfo.collider.CompareTag( Utils.TILE_TAG );
            if ( m_IsFarmGroundTag = hitInfo.collider.CompareTag( Utils.FARM_GROUND_TAG ) )
            {
                Vector3 tilePos = m_TileManager.WorldToTilePos( hitInfo.point );
                tilePos.Set( tilePos.x, .001f, tilePos.z );
                Quaternion tileRot = Quaternion.Euler( 90f, 0, 0 );

                m_TileMatrix = Matrix4x4.TRS( tilePos, tileRot, Vector3.one );
                previewTileMaterialChanger.ChangePreviewMaterialColor( true );
                Graphics.DrawMesh( m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0 );
                m_UIManager.ShowActionHelperPrimary( "Left", "Cangkul" );
            }
            else
            {
                m_UIManager.HideActionHelper();
                if ( previewTileMaterialChanger != null ) previewTileMaterialChanger.ChangePreviewMaterialColor( false );
            }
        }
    }

    IEnumerator PlaySFX()
    {
        transform.GetChild( 0 ).gameObject.SetActive( true );
        yield return new WaitForSeconds( 0.4f );
        transform.GetChild( 0 ).gameObject.SetActive( false );
    }


}
