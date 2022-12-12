using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(BaseFarmObject))]
public class Shovel : Item, IRaycastAction
{
    private static Transform tileParent;
    [SerializeField] private int m_TileResourcesIndex;
    private GameObject m_TileGO;
    [SerializeField][Readonly] private GameObject m_PreviewTile;
    [SerializeField] private bool m_IsFarmGroundTag;
    [SerializeField] private bool m_Collided;


    private Mesh m_PreviewTileMesh;
    private Matrix4x4 m_TileMatrix;
    MaterialChanger previewTileMaterialChanger;
    private BoxCollider m_FarmGroundBoundsCollider;
    private ResourcesLoader m_ResourceLoader;
    private FarmGround m_FarmGround;
    private AudioSource m_HoeAudioSource;
    private new void Awake()
    {
        base.Awake();
        m_HoeAudioSource = GetComponent<AudioSource>();
        m_ResourceLoader = ResourcesLoader.Instance;
        m_FarmGround = FarmGround.Instance;
        m_TileResourcesIndex = m_ResourceLoader.GetFarmObjectIndex<PlantTile>();
        m_TileGO = m_ResourceLoader.FarmObjects[m_TileResourcesIndex];
        previewTileMaterialChanger = m_TileGO.GetComponent<MaterialChanger>();
    }
    private void Start()
    {
        tileParent = GameObject.FindGameObjectWithTag(Utils.TILE_PARENT_TAG).transform;
        m_PreviewTileMesh = m_TileGO.GetComponent<MeshFilter>().sharedMesh;
    }
    private void OnDisable()
    {
        if (m_PreviewTile != null) m_PreviewTile.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(m_PreviewTile);
        m_PreviewTile = null;
    }


    public override void MainAction()
    {
        if (!m_IsFarmGroundTag || m_Collided) return;

        GameObject tileCopyGO = Instantiate(m_TileGO, m_TileMatrix.MultiplyPoint3x4(Vector3.zero), m_TileMatrix.rotation);
        tileCopyGO.name = "Tile";
        tileCopyGO.transform.parent = tileParent;

        MaterialChanger materialChanger = tileCopyGO.GetComponent<MaterialChanger>();
        materialChanger.ChangeToFinalMaterial();

        BoxCollider boxCollider = tileCopyGO.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        boxCollider.isTrigger = true;

        tileCopyGO.layer = 9;
        tileCopyGO.GetComponent<PlantTile>().SwitchStatus(PlantTile.TileStatus.HOED);

        int tileIndex = m_FarmGround.GetUniqueIdx(tileCopyGO.transform.position);
        PlantTile tile = tileCopyGO.GetComponent<PlantTile>();
        tile.Set(m_TileResourcesIndex, tileIndex);
        m_FarmGround.FarmObjects.Add(tileIndex, tile);

        // play Hoe sound effect
        // nanti harusnya pake animation event buat active deactive sfx
        StartCoroutine(PlaySFX());
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null)
        {
            m_Collided = hitInfo.collider.CompareTag(Utils.TILE_TAG);
            if (m_IsFarmGroundTag = hitInfo.collider.CompareTag(Utils.FARM_GROUND_TAG))
            {
                if (m_FarmGroundBoundsCollider == null) m_FarmGroundBoundsCollider = hitInfo.collider.transform.GetChild(0).GetComponent<BoxCollider>();
                Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point) + Vector3.up * .01f;
                Quaternion tileRot = Quaternion.Euler(90f, 0, 0);
                Collider[] colliders = Physics.OverlapBox(tilePos, Vector3.one / 4, tileRot);
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(Utils.TREE_OBSTACLE_TAG))
                    {
                        m_Collided = true;
                        m_UIManager.HideActionHelper();
                        if (previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
                        return;
                    }
                }
                if (!m_FarmGroundBoundsCollider.bounds.Contains(tilePos))
                {
                    m_Collided = true;
                    m_UIManager.HideActionHelper();
                    if (previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
                    return;
                }
                m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
                previewTileMaterialChanger.ChangePreviewMaterialColor(true);
                Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
                m_UIManager.ShowActionHelperPrimary("Left", "Cangkul");
            }
            else
            {
                m_UIManager.HideActionHelper();
                if (previewTileMaterialChanger != null) previewTileMaterialChanger.ChangePreviewMaterialColor(false);
            }
        }
    }

    IEnumerator PlaySFX()
    {
        StopAllCoroutines();
        m_HoeAudioSource.Play();
        yield return new WaitForSeconds(0.4f);
        m_HoeAudioSource.Stop();
    }


}
