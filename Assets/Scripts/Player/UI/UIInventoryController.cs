//Must attach to active obj
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryController : MonoBehaviour,  IPointerClickHandler
{
    public ItemSlot SelectedItem { get => m_SelectedSlot; set => m_SelectedSlot =  value ; }

    [SerializeReference] private ItemSlot m_SelectedSlot;
    [SerializeField] private GameObject m_DropUI;

    private void OnDisable()
    {
        DisableDropUI();
    }

    public void EnableDropUI() => m_DropUI.SetActive( true );
    public void DisableDropUI() => m_DropUI.SetActive( false );

    public void OnPointerClick( PointerEventData eventData )
    {
        DisableDropUI();
    }
}
