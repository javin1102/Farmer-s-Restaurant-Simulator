using NPC.Citizen;
using UnityEngine;

public class Seat : Furniture
{
    public bool IsOccupied { get => m_IsOccupied; set => m_IsOccupied = value; }
    public Table Table { get => m_Table; set => m_Table = value; }
    public Citizen Citizen { get => m_Citizen; set => m_Citizen = value; }
    public Transform SitTf { get => m_SitTf; }

    [SerializeField] private Transform m_SitTf;
    [SerializeField] private Table m_Table;
    private Seat m_InstantiatedSeat;
    [SerializeField] private bool m_IsOccupied;
    [SerializeField] private Citizen m_Citizen;
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
        RaycastHit[] hits = Physics.BoxCastAll( m_InstantiatedGO.transform.position, Vector3.one / 3, ray.direction, Quaternion.identity, 1 );
        foreach ( var hitInfo in hits )
        {
            if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Table table ) )
            {
                if ( !table.HasMaxSeats )
                {
                    table.Seats.Add( m_InstantiatedSeat );
                    m_InstantiatedSeat.m_Table = table;
                    m_Restaurant.UnoccupiedSeats.Add( m_InstantiatedSeat );
                }

            }
        }
    }

    //Seat has been placed before
    public void CheckTable_Instantiated()
    {
        Ray ray = new( transform.position, transform.forward );
        RaycastHit[] hits = Physics.BoxCastAll( transform.position, Vector3.one / 2, ray.direction, Quaternion.identity, 1 );
        foreach ( var hitInfo in hits )
        {
            if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Table table ) )
            {
                if ( !table.HasMaxSeats )
                {
                    table.Seats.Add( this );
                    m_Table = table;
                    m_Restaurant.UnoccupiedSeats.Add( this );
                }

            }
        }

    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        if ( !m_IsInstantiated ) return;
        if ( m_Table != null ) m_Table.Seats.Remove( this );
        m_Restaurant = RestaurantManager.Instance;
        m_Restaurant.UnoccupiedSeats.Remove( this );
    }
}
