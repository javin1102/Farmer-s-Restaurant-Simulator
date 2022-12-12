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
    public SeedData SeedData;


    public void GrowProgression()
    {
        if (statePlant >= 2) return;
        statePlant++;
        SetPlant();
    }

    public void SetPlant()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
        transform.GetChild(Mathf.Clamp(statePlant, 0, 2)).gameObject.SetActive(true);
    }
}
