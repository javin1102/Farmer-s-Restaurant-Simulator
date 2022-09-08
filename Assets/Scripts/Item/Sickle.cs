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
    private ActionSlotsController m_ActionSlotsController;
    private InventoryController m_InventoryController;

    public ItemData harvestCropData;

    private void Awake()
    {
        m_ActionSlotsController = GetComponentInParent<ActionSlotsController>();
        m_InventoryController = GetComponentInParent<InventoryController>();
    }

    public override void MainAction()
    {
        if (selectedCrop!=null)
        {
            harvestCropData = selectedCrop.GetComponentInParent<PlantGrowHandler>().cropData;
            Debug.Log(harvestCropData);

            AddCropToInventory(harvestCropData);
            Destroy(selectedCrop);
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

    public void AddCropToInventory(ItemData item)
    {
        Debug.Log("CROP HARVESTED!!!");
        if (m_ActionSlotsController.Store(item)) return;
        if (m_InventoryController.Store(item)) return;

    }
}
