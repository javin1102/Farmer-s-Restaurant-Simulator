using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// SCRIPT BUAT HANDLE GROW PROGRESSION [WIP] 
/// 
/// HARUSNYA CROP DITANAM TUMBUH TRUS KERING DAN MESTI DISIRAM LAGI BARU STATE NAMBAH
/// 
/// </summary>

public class PlantGrowHandler : MonoBehaviour
{
    public int statePlant = 0; // 0 : seed , 1: half , 2: ready to harvest

    [SerializeField] private float growTime = 2f;

    private bool _isWet;

    private Tile tile;
    private Renderer tileRenderer;

    public Material tileDefaultMaterial;


    private void Start()
    {
        tile = GetComponentInParent<Tile>();
        tileRenderer = GetComponentInParent<Renderer>();

        InvokeRepeating("StartGrow", 1f, 1f);

    }

      
    public void StartGrow()
    {
        Debug.Log("GROW SCRIPT : stateplant = " + statePlant);
        if (tile.CompareTag(Utils.TILE_WET_TAG))
        {
            GrowProgression();
        }
    }

    public void GrowProgression()
    {
        if (statePlant != 3)//3 = max growth
        {
            gameObject.transform.GetChild(statePlant).gameObject.SetActive(true);
        }
        if (statePlant > 0 && statePlant < 3)// set false -1 current state and set true to current state
        {
            gameObject.transform.GetChild(statePlant - 1).gameObject.SetActive(false);
        }
        if (statePlant < 3)
        {
             statePlant++;
            //StartCoroutine(WaitGrow());
        }
    }
    
/*    private IEnumerator WaitGrow()
    {
        yield return new WaitForSeconds(2f);
        statePlant++;
        tile.tag = Utils.TILE_TAG;
        tileRenderer.material = tileDefaultMaterial;
    }*/
    

}
