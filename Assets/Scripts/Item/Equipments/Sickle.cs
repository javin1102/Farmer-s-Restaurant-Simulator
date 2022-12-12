using System.Collections;
using UnityEngine;

public class Sickle : Item, IRaycastAction
{
    public GameObject selectedCrop;
    private ItemDatabase m_ItemDatabase;

    public ItemData harvestCropData;

    private SeedData m_SeedData;

    private float dropChance;
    private bool IsDrop = false;

    private Matrix4x4 m_TileMatrix;
    private Mesh m_PreviewTileMesh;
    private MaterialChanger previewTileMaterialChanger;
    private AudioSource m_HarvestAudioSource;
    private new void Awake()
    {
        base.Awake();
        m_ItemDatabase = transform.root.GetComponent<ItemDatabase>();
        m_HarvestAudioSource = GetComponent<AudioSource>();
    }

    public override void MainAction()
    {
        if (selectedCrop != null)
        {
            m_SeedData = selectedCrop.GetComponentInParent<PlantGrowHandler>().SeedData;

            float rand = Random.Range(1f, 10f);
            if (rand <= m_SeedData.dropChance) AddSeedToInventory(m_SeedData);

            AddCropToInventory(m_SeedData.harverstedCropData);
            selectedCrop.GetComponentInParent<PlantTile>().IsUsed = false;
            Destroy(selectedCrop.transform.parent.gameObject);

            // play sickle sound effect
            StartCoroutine(PlaySFX());
        }
    }

    public void PerformRaycastAction(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag(Utils.CROP_TAG))
        {
            if (selectedCrop != null && selectedCrop != hitInfo.transform.gameObject)
            {
                selectedCrop.layer = 9;
            }
            selectedCrop = hitInfo.transform.gameObject;
            selectedCrop.layer = 13;
            UIManager.Instance.ShowActionHelperPrimary("Left", "To Use Sickle...");
            return;
        }
        if (selectedCrop != null)
        {
            selectedCrop.layer = 9;
        }
        selectedCrop = null;
        UIManager.Instance.HideActionHelper();
        return;
    }

    private void AddSeedToInventory(SeedData seed)
    {
        m_ItemDatabase.Store(seed);
    }

    private void AddCropToInventory(ItemData item)
    {
        m_ItemDatabase.Store(item);
    }

    IEnumerator PlaySFX()
    {
        StopAllCoroutines();
        m_HarvestAudioSource.Play();
        yield return new WaitForSeconds(0.4f);
        m_HarvestAudioSource.Stop();
    }
}
