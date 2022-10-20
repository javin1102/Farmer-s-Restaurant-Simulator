using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : Seed,IRaycastAction
{
    private Tile m_tile;
    [SerializeField] private ItemMainActionChannel m_DecreseableEvent;
    [SerializeField] private Material plantedMaterial;

    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private TileManager m_TileManager;
    private MaterialChanger previewTileMaterialChanger;

    private void Start()
    {
        m_TileManager = TileManager.instance;
    }

    public override void MainAction()
    {
        if (m_tile != null && !m_tile.IsUsed)
        {

            m_tile.crop = m_seedData.cropPrefab;
            m_tile.SpawnCrop();
            m_DecreseableEvent.RaiseEvent();

            this.m_tile.SwitchStatus(Tile.TileStatus.PLANTED);
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Tile tile))
        {
            m_tile = tile;

            m_PreviewTileMesh = m_tile.GetComponent<MeshFilter>().sharedMesh;
            previewTileMaterialChanger = m_tile.GetComponent<MaterialChanger>();
            Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point);
            tilePos.Set(tilePos.x, .11f, tilePos.z);
            Quaternion tileRot = Quaternion.Euler(90f, 0, 0);

            m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
            previewTileMaterialChanger.ChangePreviewMaterialColor(true);
            Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
            UIManager.Instance.ShowActionHelper("Left", "To Plant The Seed...");
            return;    
        }
            previewTileMaterialChanger.ChangePreviewMaterialColor(false);
            UIManager.Instance.HideActionHelper();
            return;
    }
}
