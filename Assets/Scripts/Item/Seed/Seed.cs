using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Seed : Item,IRaycastAction
{
    [SerializeField] private Tile m_tile;
    [SerializeField] private GameObject m_PrefabPlant;
    [SerializeField] private Material plantedMaterial;


    public override void MainAction()
    {
        if (m_tile != null && !m_tile.IsUsed)
        {
            if(!m_tile.CompareTag(Utils.TILE_WET_TAG)) m_tile.gameObject.GetComponent<Renderer>().material = plantedMaterial;

            m_tile.crop = m_PrefabPlant;
            m_tile.SpawnCrop();
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Tile tile))
        {
            m_tile = tile;
        }
    }

}
