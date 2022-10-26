using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFurnitureBuy : MonoBehaviour
{
    [SerializeField] private GameObject m_UIItemPrefab;
    [SerializeField] private GameObject m_Content;
    [SerializeField] private Toggle m_TableToggle, m_ChairToggle, m_StoveToggle;
    private readonly List<UIFurnitureItem> m_TableItems = new(), m_ChairItems = new(), m_StoveItems = new();
    private UIFurnitureStoreController m_StoreController;
    private void Start()
    {
        m_StoreController = UIFurnitureStoreController.Instance as UIFurnitureStoreController;
        m_StoreController.FurnituresData.ForEach( InstantiateUIItem );
        m_TableToggle.onValueChanged.AddListener( TableFilter );
        m_ChairToggle.onValueChanged.AddListener( ChairFilter );
        m_StoveToggle.onValueChanged.AddListener( StoveFilter );

        m_TableItems.ForEach( EnableUIItem );
    }

    private void StoveFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
   
        m_ChairItems.ForEach( DisableUIItem );
        m_TableItems.ForEach( DisableUIItem );
        m_StoveItems.ForEach( EnableUIItem );
    }



    private void ChairFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_ChairItems.ForEach( EnableUIItem );
        m_TableItems.ForEach( DisableUIItem );
        m_StoveItems.ForEach( DisableUIItem );
    }

    private void TableFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_TableItems.ForEach( EnableUIItem );
        m_ChairItems.ForEach( DisableUIItem );
        m_StoveItems.ForEach( DisableUIItem );
    }
    private void DisableUIItem( UIFurnitureItem obj )
    {
        obj.gameObject.SetActive( false );
    }

    private void EnableUIItem( UIFurnitureItem obj )
    {
        obj.gameObject.SetActive( true );
    }

    void InstantiateUIItem( FurnitureData furnitureData )
    {
        UIFurnitureItem uiItem = Instantiate( m_UIItemPrefab, m_Content.transform ).GetComponent<UIFurnitureItem>();
        uiItem.ItemData = furnitureData;
        switch ( furnitureData.type )
        {
            case FurnitureType.TABLE:
                m_TableItems.Add( uiItem );
                break;
            case FurnitureType.CHAIR:
                m_ChairItems.Add( uiItem );
                break;
            case FurnitureType.STOVE:
                m_StoveItems.Add( uiItem );
                break;
        }
        uiItem.gameObject.SetActive( false );
    }
}
