using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class UIIngredientSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public IngredientData IngredientData { get => m_IngredientData; set => m_IngredientData = value; }
    public UnityAction<IngredientData, Vector3> OnHoverEnter { get => m_OnHoverEnter; set => m_OnHoverEnter =  value ; }
    public UnityAction OnHoverExit { get => m_OnHoverExit; set => m_OnHoverExit =  value ; }

    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_QuantityText, m_IngredientNameText;
    [SerializeField] private GameObject m_OverlayGO;
    private IngredientData m_IngredientData;
    private FoodsController m_FoodsController;
    private UnityAction<IngredientData, Vector3> m_OnHoverEnter;
    private UnityAction m_OnHoverExit;
    private RectTransform m_RectTf;
    private void Start()
    {
        m_FoodsController = FoodsController.Instance;
        m_RectTf = GetComponent<RectTransform>();
    }

    private void Update() => UpdateUI();

    public void UpdateUI()
    {
        m_IngredientNameText.text = m_IngredientData.id;
        m_Icon.sprite = m_IngredientData.icon;
        if ( m_FoodsController.StockIngredients.TryGetValue( m_IngredientData.id, out StockIngredient ingredient ) )
        {
            EnableOverlayUI( ingredient );
        }
        else
        {
            DisableOverlayUI();
        }
    }

    private void EnableOverlayUI( StockIngredient ingredient)
    {
        m_OverlayGO.SetActive( false );
        m_QuantityText.text = ingredient.quantity.ToString();
    }

    private void DisableOverlayUI()
    {
        m_OverlayGO.SetActive( true );
        m_QuantityText.text = "0";
    }

    public void OnPointerEnter( PointerEventData eventData )
    {
        OnHoverEnter?.Invoke( IngredientData, m_RectTf.anchoredPosition );
    }

    public void OnPointerExit( PointerEventData eventData )
    {
        OnHoverExit?.Invoke();
        //print( "exit" );
    }
}
