using System.Collections;
using UnityEngine;

public class Sickle : Item, IRaycastAction
{
    public GameObject selectedCrop;
    public ItemData harvestCropData;

    private ItemDatabase m_ItemDatabase;
    private SeedData seedData;
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

            int randSeedQuantity = Mathf.RoundToInt(Random.Range(seedData.minSeedDropQuantity, seedData.maxSeedDropQuanitty + 1));
            int randIngredientQuantity = Mathf.RoundToInt(Random.Range(seedData.minIngredientDropQuantity, seedData.maxIngredientDropQuantity + 1));
            AddSeedToInventory(seedData, randSeedQuantity);
            AddIngredientToInventory(seedData.harvestedIngredientData, randIngredientQuantity);
            PlantTile plantTile = selectedCrop.GetComponentInParent<PlantTile>();
            plantTile.IsUsed = false;
            if (plantTile.CompareTag(Utils.TILE_WET_TAG)) plantTile.SwitchStatus(PlantTile.TileStatus.WATERED);
            else plantTile.SwitchStatus(PlantTile.TileStatus.HOED);
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
            UIManager.Instance.ShowActionHelperPrimary("Left", "Ambil hasil panen");
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
        if (quantity <= 0) return;

        if (!m_ItemDatabase.Store(seed, quantity))
        {
            Item itemDrop = Instantiate(seed.prefab, selectedCrop.transform.position + RandomOffsetCircle() * .5f, Quaternion.identity).GetComponent<Item>();
            itemDrop.DropState(quantity);
        }
        else
        {
            m_UIManager.NotificationQueue.Enqueue($"<color=yellow>+{quantity}</color> {seed.ID}");
            PlayerAction.Instance.PlayAudio("bubble_sfx");
        }


    }

    private Vector3 RandomOffsetCircle()
    {
        Vector2 randomPos = Random.insideUnitCircle * .5f;
        Vector3 offset = new Vector3(randomPos.x, .25f, randomPos.y);
        return offset;
    }

    private void AddIngredientToInventory(ItemData item, int quantity)
    {
        if (quantity <= 0) return;


        if (!m_ItemDatabase.Store(item, quantity))
        {
            Item itemDrop = Instantiate(item.prefab, selectedCrop.transform.position + RandomOffsetCircle(), Quaternion.identity).GetComponent<Item>();
            itemDrop.DropState(quantity);
        }
        else
        {
            m_UIManager.NotificationQueue.Enqueue($"<color=yellow>+{quantity}</color> {item.ID}");
            PlayerAction.Instance.PlayAudio("bubble_sfx");
        }
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
