using System.Collections;
using UnityEngine;


public class Seed : Item, IRaycastAction
{

    protected SeedData m_SeedData;
    private PlantTile m_tile;
    [SerializeField] private ItemMainActionChannel m_DecreseableEvent;
    [SerializeField] private Transform m_SFXTf;
    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private MaterialChanger previewTileMaterialChanger;
    private AudioSource m_PlantAudioSource;
    private new void Awake()
    {
        base.Awake();
        m_PlantAudioSource = GetComponent<AudioSource>();
    }
    public override void MainAction()
    {
        m_SeedData = m_Data as SeedData;
        if (m_tile != null && !m_tile.IsUsed)
        {

            m_tile.SpawnCrop(m_SeedData.cropPrefab);

            this.m_tile.SwitchStatus(PlantTile.TileStatus.PLANTED);

            // play planted soound effect
            StartCoroutine(PlaySFX());

            m_DecreseableEvent.RaiseEvent();
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out PlantTile tile))
        {
            m_tile = tile;
            if (m_tile.crop != null) return;
            m_PreviewTileMesh = m_tile.GetComponent<MeshFilter>().sharedMesh;
            previewTileMaterialChanger = m_tile.GetComponent<MaterialChanger>();
            Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point);
            tilePos.Set(tilePos.x, .02f, tilePos.z);
            Quaternion tileRot = Quaternion.Euler(90f, 0, 0);

            m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
            previewTileMaterialChanger.ChangePreviewMaterialColor(true);
            Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
            UIManager.Instance.ShowActionHelperPrimary("Left", "Tanam");
            return;
        }
        if (previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
        UIManager.Instance.HideActionHelper();
        return;
    }

    IEnumerator PlaySFX()
    {
        StopAllCoroutines();
        m_PlantAudioSource.Play();
        yield return new WaitForSeconds(0.4f);
        m_PlantAudioSource.Stop();
    }

}
