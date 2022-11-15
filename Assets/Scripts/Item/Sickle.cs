using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sickle : Item,IRaycastAction
{
    public GameObject selectedCrop;
    private ItemDatabase m_ItemDatabase;
    //private ActionSlotsController m_ActionSlotsController;
    //private InventoryController m_InventoryController;

    public ItemData harvestCropData;

    private SeedData m_SeedData; 

    private float dropChance;
    private bool IsDrop = false;

    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private MaterialChanger previewTileMaterialChanger;
    //private new void OnEnable()
    //{
    //    base.OnEnable();
    //}

    private new void Awake()
    {
        base.Awake();
        m_ItemDatabase = transform.root.GetComponent<ItemDatabase>();
        //m_TileManager = TileManager.instance;
        //m_ActionSlotsController = GetComponentInParent<ActionSlotsController>();
        //m_InventoryController = GetComponentInParent<InventoryController>();
    }

    public override void MainAction()
    {
        if (selectedCrop!=null)
        {
            m_SeedData = selectedCrop.GetComponentInParent<PlantGrowHandler>().m_SeedData;

            float rand = Random.Range(1f, 10f);
            if (rand <= m_SeedData.dropChance) AddSeedToInventory(m_SeedData);

            AddCropToInventory(m_SeedData.harverstedCropData);
            selectedCrop.GetComponentInParent<Tile>().IsUsed = false;
            Destroy(selectedCrop.transform.parent.gameObject);

            // play sickle sound effect
            StartCoroutine(PlaySFX());
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        // if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out PlantGrowHandler plantGrowHandler))
        if (hitInfo.collider.CompareTag(Utils.CROP_TAG))
        {
            selectedCrop = hitInfo.transform.gameObject;

            previewTileMaterialChanger = selectedCrop.transform.parent.GetComponentInParent<MaterialChanger>();
            m_PreviewTileMesh = selectedCrop.transform.parent.GetComponentInParent<MeshFilter>().sharedMesh;

            Vector3 tilePos = m_TileManager.WorldToTilePos(hitInfo.point);
            tilePos.Set(tilePos.x, .11f, tilePos.z);
            Quaternion tileRot = Quaternion.Euler(90f, 0, 0);

            m_TileMatrix = Matrix4x4.TRS(tilePos, tileRot, Vector3.one);
            previewTileMaterialChanger.ChangePreviewMaterialColor(true);
            Graphics.DrawMesh(m_PreviewTileMesh, m_TileMatrix, previewTileMaterialChanger.PreviewMaterial, 0);
            UIManager.Instance.ShowActionHelperPrimary("Left", "To Use Sickle...");
            return;
        }
        if( previewTileMaterialChanger != null )previewTileMaterialChanger.ChangePreviewMaterialColor(false);
        UIManager.Instance.HideActionHelper();
        return;
    }

    public void AddSeedToInventory(SeedData seed)
    {
        Debug.Log("seedinventory");
        m_ItemDatabase.Store( seed );
        //if (m_ActionSlotsController.Store(seed)) return;
        //if (m_InventoryController.Store(seed)) return;
    }

    public void AddCropToInventory(ItemData item)
    {
        m_ItemDatabase.Store( item );
        //if (m_ActionSlotsController.StoreHarvestedCrop(item)) return;
        //if (m_InventoryController.Store(item)) return;
    }

    IEnumerator PlaySFX()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
