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
    private MaterialChanger previewTileMaterialChanger;
    public override void MainAction()
    {
        if (selectedTile != null && !selectedTile.CompareTag(Utils.TILE_WET_TAG))
        {
            Debug.Log("WATER SCRIPT : WATERED");
            selectedTile.GetComponent<PlantTile>().SwitchStatus(PlantTile.TileStatus.WATERED);

            // play water can sound effect
            StartCoroutine(PlaySFX());
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {

        if (hitInfo.collider.CompareTag(Utils.TILE_TAG))
        {
            // save tile gameobject to variable
            selectedTile = hitInfo.transform.gameObject;
            m_PreviewTileMesh = selectedTile.GetComponent<MeshFilter>().sharedMesh;
            previewTileMaterialChanger = selectedTile.GetComponent<MaterialChanger>();

            Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point) + Vector3.up * .01f;
            Quaternion tileRot = Quaternion.Euler(90f, 0, 0);

            m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
            previewTileMaterialChanger.ChangePreviewMaterialColor(true);
            Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
            UIManager.Instance.ShowActionHelperPrimary("Left" , "To Use Water Can...");
            return;
        }
        if( previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
        UIManager.Instance.HideActionHelper();
        return;
    }

    IEnumerator PlaySFX()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
