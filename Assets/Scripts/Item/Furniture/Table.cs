using System.Collections.Generic;
using UnityEngine;


public class Table : Furniture
{
    public bool HasMaxSeats { get => m_Seats.Count >= 4; }
    public bool HasSeat { get => m_Seats.Count > 0; }
    public List<Seat> Seats => m_Seats;

    [SerializeField] private List<Seat> m_Seats = new();
    private Table m_InstantiatedTable;
    private Transform m_FoodPlaces;

    private void Start()
    {
        m_FoodPlaces = transform.GetChild( 1 );
    }

    public Transform GetFoodPlace( Seat seat )
    {
        foreach ( Transform child in m_FoodPlaces )
        {
            if ( child.forward == seat.transform.forward )
            {
                return child;
            }
        }

        return null;
    }
    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        base.MainAction();
        m_InstantiatedTable = m_InstantiatedGO.GetComponent<Table>();
        m_Restaurant.Tables.Add( m_InstantiatedGO.GetComponent<Table>() );
        CheckSeat();
    }

    private void CheckSeat()
    {
        //Ray in 4 dir
        Ray[] rays = new Ray[4]
        {
            new Ray(m_InstantiatedGO.transform.position, -m_InstantiatedGO.transform.forward),
            new Ray(m_InstantiatedGO.transform.position, m_InstantiatedGO.transform.forward),
            new Ray(m_InstantiatedGO.transform.position, m_InstantiatedGO.transform.right),
            new Ray(m_InstantiatedGO.transform.position, -m_InstantiatedGO.transform.right),
        };

        foreach ( Ray ray in rays )
        {
            RaycastHit[] hits = Physics.BoxCastAll( m_InstantiatedGO.transform.position, Vector3.one / 2, ray.direction, Quaternion.identity, 1 );
            foreach ( var hitInfo in hits )
            {
                if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Seat seat ) )
                {
                    seat.Table = m_InstantiatedTable;
                    m_Seats.Add( seat );
                    m_Restaurant.UnoccupiedSeats.Add( seat );
                }
            }
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
        if ( !m_IsInstantiated ) return;
        m_Restaurant = RestaurantManager.Instance;
        m_Restaurant.Tables.Remove( this );
    }

}
