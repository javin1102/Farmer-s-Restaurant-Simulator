using UnityEngine;

/// <summary>
/// 
/// SCRIPT BUAT SPAWN SEED [WIP]
/// 
/// HARUSNYA BUAT TILE DOANG JANGAN LGSUNG INSTATIATE SEEDNYA
/// 
/// </summary>
public class Tile : MonoBehaviour
{
    public GameObject cornGO;
 
    private void Awake()
    {
        cornGO = Instantiate(Resources.Load("Plant_Corn"),this.gameObject.transform.position,Quaternion.identity) as GameObject;
        cornGO.transform.SetParent(this.transform);
    }

}
