using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// SCRIPT BUAT HANDLE ITEM SICKLE [WIP]
/// 
/// COMPARE TAG -> SIMPAN SELECTED OBJECT -> KALO DI KLIK KIRI DESTROY
/// 
/// 
/// 
/// </summary>

    
        /*
        --- TODO ---
        * add inventory
        * add possibilities to drop seed
        * jarak raycast kykny hrus dikurangi biar logis [DARI JAUH BISA SICKLE CROP]
        * 
        * 
        */
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
    private void Awake()
    {
        m_ItemDatabase = transform.root.GetComponent<ItemDatabase>();
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

            Debug.Log("random : " + rand + " " + m_SeedData.dropChance + " " + m_SeedData.id);

            AddCropToInventory(m_SeedData.harverstedCropData);
            selectedCrop.GetComponentInParent<Tile>().IsUsed = false;
            Destroy(selectedCrop.transform.parent.gameObject);
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
       Debug.Log("SICKLE SCRIPT : RAYCAST HIT GAMEOBJECT " + hitInfo.transform.gameObject.name);

       // if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out PlantGrowHandler plantGrowHandler))
        if(hitInfo.collider.CompareTag(Utils.CROP_TAG))
        {
            var selectObject = hitInfo.transform;
            if (selectObject != null)
            {
                Debug.Log("performraycast sickle");
                selectedCrop = hitInfo.transform.gameObject;

                //   Debug.Log("Sickle script : destroy : " + selectedCrop.name);
            }
        }
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
}
