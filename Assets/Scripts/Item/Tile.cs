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
    public bool IsUsed = false;
    public GameObject crop;

    public void SpawnCrop()
    {
        IsUsed = true;
        crop = Instantiate(this.crop, transform.position, Quaternion.identity);
        crop.transform.SetParent(this.transform);
    }
}
