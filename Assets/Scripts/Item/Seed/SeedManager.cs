using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : Seed, IRaycastAction
{
    private Tile m_tile;
    [SerializeField] private ItemMainActionChannel m_DecreseableEvent;
    [SerializeField] private Material plantedMaterial;
    [SerializeField] private Transform m_SFXTf;
    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private MaterialChanger previewTileMaterialChanger;

    public override void MainAction()
    {
        if ( m_tile != null && !m_tile.IsUsed )
        {

            m_tile.crop = m_seedData.cropPrefab;
            m_tile.SpawnCrop();

            this.m_tile.SwitchStatus( Tile.TileStatus.PLANTED );

            // play planted soound effect
            StartCoroutine( PlaySFX() );

            m_DecreseableEvent.RaiseEvent();
        }
    }

    public void PerformRaycastAction( RaycastHit hitInfo )
    {
        if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Tile tile ) )
        {
            m_tile = tile;

            m_PreviewTileMesh = m_tile.GetComponent<MeshFilter>().sharedMesh;
            previewTileMaterialChanger = m_tile.GetComponent<MaterialChanger>();
            Vector3 tilePos = m_TileManager.WorldToTilePos( hitInfo.point );
            tilePos.Set( tilePos.x, .0015f, tilePos.z );
            Quaternion tileRot = Quaternion.Euler( 90f, 0, 0 );

            m_TileMatrix = Matrix4x4.TRS( tilePos, tileRot, Vector3.one );
            previewTileMaterialChanger.ChangePreviewMaterialColor( true );
            Graphics.DrawMesh( m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0 );
            UIManager.Instance.ShowActionHelperPrimary( "Left", "Tanam" );
            return;
        }
        if ( previewTileMaterialChanger != null ) previewTileMaterialChanger.ChangePreviewMaterialColor( false );
        UIManager.Instance.HideActionHelper();
        return;
    }

    IEnumerator PlaySFX()
    {
        // child 1 beda dgn yg lains
        //transform.GetChild( 1 ).gameObject.SetActive( true );
        //yield return new WaitForSeconds( 0.4f );
        //transform.GetChild( 1 ).gameObject.SetActive( false );
        m_SFXTf.gameObject.SetActive( true );
        yield return new WaitForSeconds( 0.4f );
        m_SFXTf.gameObject.SetActive( true );
    }
}
