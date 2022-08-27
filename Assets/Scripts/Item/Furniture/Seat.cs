using UnityEngine;

public class Seat : Furniture
{
    [SerializeField] private Table m_Table;
    private Seat m_InstantiatedSeat;

    public Table Table { get => m_Table; set => m_Table =  value ; }

    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        base.MainAction();
        m_InstantiatedSeat = m_InstantiatedGO.GetComponent<Seat>();
        CheckForTable();
    }

    //Seat just get instantiated
    private void CheckForTable()
    {
        Ray ray = new( m_InstantiatedGO.transform.position, m_InstantiatedGO.transform.forward );
      
        if ( Physics.Raycast( ray, out RaycastHit hitInfo, 2 ) )
        {
            if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Table table ) )
            {
                if ( !table.IsFull ) {
                    table.Seats.Add( m_InstantiatedSeat );
                    m_InstantiatedSeat.m_Table = table;
                } 

            }
        }
    }

    //Seat has been placed before
    public void CheckTable_Instantiated()
    {
        Ray ray = new( transform.position, transform.forward );

        if ( Physics.Raycast( ray, out RaycastHit hitInfo, 2 ) )
        {
            if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Table table ) )
            {
                if ( !table.IsFull )
                {
                    table.Seats.Add( this );
                    m_Table = table;
                }

            }
        }
    }

    private void OnDestroy()
    {
        if ( m_Table != null ) m_Table.Seats.Remove( this );
    }
}
