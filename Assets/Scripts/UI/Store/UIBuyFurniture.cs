using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyFurniture : MonoBehaviour
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

        m_TableItems.ForEach( EnableItem );
    }

    private void StoveFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
   
        m_ChairItems.ForEach( DisableItem );
        m_TableItems.ForEach( DisableItem );
        m_StoveItems.ForEach( EnableItem );
    }



    private void ChairFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_ChairItems.ForEach( EnableItem );
        m_TableItems.ForEach( DisableItem );
        m_StoveItems.ForEach( DisableItem );
    }

    private void TableFilter( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_TableItems.ForEach( EnableItem );
        m_ChairItems.ForEach( DisableItem );
        m_StoveItems.ForEach( DisableItem );
    }
    private void DisableItem( UIFurnitureItem obj )
    {
        obj.gameObject.SetActive( false );

    }

    private void EnableItem( UIFurnitureItem obj )
    {
        obj.gameObject.SetActive( true );
    }

    void InstantiateUIItem( FurnitureData furnitureData )
    {
        UIFurnitureItem uiItem = Instantiate( m_UIItemPrefab, m_Content.transform ).GetComponent<UIFurnitureItem>();
        uiItem.FurnitureData = furnitureData;
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
