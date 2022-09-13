using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : Seed,IRaycastAction
{
    private Tile m_tile;
    [SerializeField] private ItemMainActionChannel m_DecreseableEvent;
    [SerializeField] private Material plantedMaterial;

    public override void MainAction()
    {
        if (m_tile != null && !m_tile.IsUsed)
        {
            if (!m_tile.CompareTag(Utils.TILE_WET_TAG)) m_tile.gameObject.GetComponent<Renderer>().material = plantedMaterial;

            m_tile.crop = m_seedData.cropPrefab;
            m_tile.SpawnCrop();
            m_DecreseableEvent.RaiseEvent();
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Tile tile))
        {
            Debug.Log("tile");
            m_tile = tile;
        }
    }
}
