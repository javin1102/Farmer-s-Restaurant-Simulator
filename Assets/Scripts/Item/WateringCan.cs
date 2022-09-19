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

    public Material selectedMaterial;

    private Renderer selectObjectRenderer;


    public override void MainAction()
    {
        if (selectedTile != null && !selectedTile.CompareTag(Utils.TILE_WET_TAG))
        {
            selectedTile.tag = Utils.TILE_WET_TAG;
            Debug.Log("WATER SCRIPT : WATERED");
            selectObjectRenderer.material = selectedMaterial;
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag(Utils.TILE_TAG))
        {
            var selectedObject = hitInfo.transform;
            if (selectedObject!=null)
            { 
                selectedTile = hitInfo.transform.gameObject;
                selectObjectRenderer = selectedTile.GetComponent<Renderer>();
            }
        }
    }
 

}
