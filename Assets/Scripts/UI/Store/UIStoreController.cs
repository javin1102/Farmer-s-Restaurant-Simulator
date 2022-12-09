using UnityEngine;
using UnityEngine.UI;

public abstract class UIStoreController : MonoBehaviour
{
    public Transform SpawnTf { get; set; }
    public BoxCollider SpawnCollider { get => m_SpawnCollider; set => m_SpawnCollider = value; }
    public static UIStoreController Instance { get => m_Instance; }

    protected static UIStoreController m_Instance;
    protected BoxCollider m_SpawnCollider;
    [SerializeField] protected GameObject m_MainMenuGO, m_BuyMenuGO, m_SellMenuGO, m_QuizMenuGO;
    [SerializeField] protected Button m_BuyMenuButton, m_SellMenuButton, m_QuizMenuButton, m_ExitMenuButton, m_BackButton;
    protected PlayerAction m_PlayerAction;
    protected ResourcesLoader m_ResourcesLoader;
    protected void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_BackButton.onClick.AddListener(OpenMainMenu);
        m_BuyMenuButton.onClick.AddListener(OpenBuyMenu);
        m_SellMenuButton.onClick.AddListener(OpenSellMenu);
        m_QuizMenuButton.onClick.AddListener(OpenQuizMenu);
        m_ExitMenuButton.onClick.AddListener(CloseStoreMenu);
    }

    public void ToggleUI(Transform spawnTf)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        m_PlayerAction.IsOtherUIOpen = gameObject.activeInHierarchy;
        if (gameObject.activeInHierarchy)
        {
            SpawnTf = spawnTf;
            m_SpawnCollider = SpawnTf.GetComponent<BoxCollider>();
            m_PlayerAction.OnEnableOtherUI?.Invoke();
        }
        else
        {
            SpawnTf = null;
            m_PlayerAction.OnDisableOtherUI?.Invoke();
        }


    }

    void OpenMenu(GameObject menu)
    {
        m_MainMenuGO.SetActive(false);
        m_BuyMenuGO.SetActive(false);
        m_QuizMenuGO.SetActive(false);
        m_SellMenuGO.SetActive(false);
        menu.SetActive(true);
        if (menu != m_MainMenuGO) m_BackButton.gameObject.SetActive(true);
        else m_BackButton.gameObject.SetActive(false);
    }

    void OpenBuyMenu() => OpenMenu(m_BuyMenuGO);
    void OpenSellMenu() => OpenMenu(m_SellMenuGO);
    void OpenMainMenu() => OpenMenu(m_MainMenuGO);
    void OpenQuizMenu() => OpenMenu(m_QuizMenuGO);
    void CloseStoreMenu()
    {
        gameObject.SetActive(false);
        m_PlayerAction.IsOtherUIOpen = false;
        m_PlayerAction.OnDisableOtherUI?.Invoke();
        SpawnTf = null;
    }

    public void SpawnItem(ItemData item)
    {
        float posX = Random.Range(SpawnCollider.bounds.min.x + .25f, SpawnCollider.bounds.max.x - .25f);
        float posY = SpawnCollider.bounds.max.y;
        float posZ = Random.Range(SpawnCollider.bounds.min.z + .25f, SpawnCollider.bounds.max.z - .25f);
        Vector3 randPos = new(posX, posY + .5f, posZ);
        Item boughtItem = Instantiate(item.prefab, randPos, Quaternion.identity).GetComponent<Item>();
        boughtItem.DropState();
    }

}
