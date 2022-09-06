using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Seed : Item,IRaycastAction
{
    [SerializeField] private Tile m_tile;
    [SerializeField] private SeedData m_seedData;   


    public override void MainAction()
    {
        if (m_tile != null && !m_tile.IsUsed)
        {
            m_tile.crop = m_Data.prefab;
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
