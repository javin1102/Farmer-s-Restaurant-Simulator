using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void MainAction()
    {
        if (selectedCrop!=null)
        {
            AddCropToInventory();
            Destroy(selectedCrop);
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
       // Debug.Log("SICKLE SCRIPT : RAYCAST HIT GAMEOBJECT " + hitInfo.transform.gameObject.name);

        if (hitInfo.collider.CompareTag(Utils.CROP_TAG))
        {
            var selectObject = hitInfo.transform;
            if (selectObject != null)
            {
                selectedCrop = hitInfo.transform.gameObject;

                //   Debug.Log("Sickle script : destroy : " + selectedCrop.name);
            }
        }
    }

    public void AddCropToInventory()
    {
        Debug.Log("CROP HARVESTED!!!");
        
    }
}
