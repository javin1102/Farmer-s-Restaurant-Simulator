using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInventoryController : MonoBehaviour, IPointerClickHandler
{
    public UIInventorySlot SelectedSlot { get => m_SelectedSlot; set => m_SelectedSlot = value; }
    public Sprite SelectedSprite { get => m_SelectedSprite; }
    public Sprite UnselectedSprite { get => m_UnselectedSprite; }

    [SerializeReference] private UIInventorySlot m_SelectedSlot;
    [SerializeField] private GameObject m_ExtraUI;
    [SerializeField] private Sprite m_SelectedSprite, m_UnselectedSprite;
    private void OnDisable()
    {
        if ( m_SelectedSlot != null )
            m_SelectedSlot.ResetSprite();
        m_SelectedSlot = null;
        DisableExtraUI();
    }

    public void EnableExtraUI() => m_ExtraUI.SetActive( true );
    public void DisableExtraUI() => m_ExtraUI.SetActive( false );
    public void OnPointerClick( PointerEventData eventData )
    {
        if ( m_SelectedSlot != null )
            m_SelectedSlot.ResetSprite();
        DisableExtraUI();
        m_SelectedSlot = null;
    }
}
