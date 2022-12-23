using System.Collections;
using UnityEngine;
public class WateringCan : Item, IRaycastAction
{

    public GameObject selectedTile;
    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private MaterialChanger previewTileMaterialChanger;
    private AudioSource m_WateringAudioSource;
    private new void Awake()
    {
        base.Awake();
        m_WateringAudioSource = GetComponent<AudioSource>();
    }
    public override void MainAction()
    {
        if (selectedTile != null && !selectedTile.CompareTag(Utils.TILE_WET_TAG))
        {
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
            UIManager.Instance.ShowActionHelperPrimary("Left", "Siram");
            return;
        }
        if (previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
        UIManager.Instance.HideActionHelper();
        selectedTile = null;
        return;
    }

    IEnumerator PlaySFX()
    {
        StopAllCoroutines();
        m_WateringAudioSource.Play();
        yield return new WaitForSeconds(0.4f);
        m_WateringAudioSource.Stop();
    }
}
