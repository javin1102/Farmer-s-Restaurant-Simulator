using System.Collections;
using UnityEngine;

public class Sickle : Item, IRaycastAction
{
    public GameObject selectedCrop;
    private ItemDatabase m_ItemDatabase;

    public ItemData harvestCropData;

    private SeedData seedData;

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
            seedData = selectedCrop.GetComponentInParent<PlantGrowHandler>().SeedData;

            float rand = Random.Range(1f, 10f);
            if (rand <= seedData.dropChance)
            {
                int randSeedQuantity = Mathf.RoundToInt(Random.Range(seedData.minSeedDropQuantity, seedData.maxSeedDropQuanitty + 1));
                AddSeedToInventory(seedData, randSeedQuantity);
            }

            int randIngredientQuantity = Mathf.RoundToInt(Random.Range(seedData.minIngredientDropQuantity, seedData.maxIngredientDropQuantity + 1));
            AddIngredientToInventory(seedData.harvestedIngredientData, randIngredientQuantity);
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

    private void AddSeedToInventory(SeedData seed, int quantity)
    {
        m_ItemDatabase.Store(seed, quantity);
    }

    private void AddIngredientToInventory(ItemData item, int quantity)
    {
        m_ItemDatabase.Store(item, quantity);
    }
    private void OnDestroy()
    {
        if (selectedCrop != null)
        {
            selectedCrop.layer = 9;
        }
        selectedCrop = null;
        UIManager.Instance.HideActionHelper();
    }
    IEnumerator PlaySFX()
    {
        StopAllCoroutines();
        m_HarvestAudioSource.Play();
        yield return new WaitForSeconds(0.4f);
        m_HarvestAudioSource.Stop();
    }
}
