using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// SCRIPT BUAT HANDLE ITEM WATER CAN
/// 
/// COMPARETAG TILE -> SIMPAN SELECTEDOBJECT -> SIMPAN RENDERER UTK UBAH MATERIAL -> KALO DIKLIK UBAH TAG SAMA MATERIAL
/// 
/// </summary>

public class WateringCan : Item,IRaycastAction
{

    public GameObject selectedTile;
    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private TileManager m_TileManager;
    private MaterialChanger previewTileMaterialChanger;


    private void Start()
    {
        m_PreviewTileMesh = selectedTile.GetComponent<MeshFilter>().sharedMesh;
        m_TileManager = TileManager.instance;
    }

    public override void MainAction()
    {
        if (selectedTile != null && !selectedTile.CompareTag(Utils.TILE_WET_TAG))
        {
            Debug.Log("WATER SCRIPT : WATERED");
            selectedTile.GetComponent<Tile>().SwitchStatus(Tile.TileStatus.WATERED);
            
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {

        if (hitInfo.collider.CompareTag(Utils.TILE_TAG))
        {
            // save tile gameobject to variable
            selectedTile = hitInfo.transform.gameObject;

            previewTileMaterialChanger = selectedTile.GetComponent<MaterialChanger>();

            Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point);
            tilePos.Set(tilePos.x, .11f, tilePos.z);
            Quaternion tileRot = Quaternion.Euler(90f, 0, 0);

            m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
            previewTileMaterialChanger.ChangePreviewMaterialColor(true);
            Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
            return;
        }
        previewTileMaterialChanger.ChangePreviewMaterialColor(false);
        return;
    }
}
