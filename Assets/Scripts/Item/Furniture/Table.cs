using System.Collections.Generic;
using UnityEngine;

public class Table : Furniture
{
    public bool IsFull { get => m_Seats.Count >= 4; }
    public bool HasSeat { get => m_Seats.Count > 0; }
    public List<Seat> Seats => m_Seats;

    public bool IsOccupied { get => m_IsOccupied; set => m_IsOccupied =  value ; }
    public Citizen Citizen { get => m_Citizen; set => m_Citizen =  value ; }

    [SerializeField] private List<Seat> m_Seats = new();
    private Table m_InstantiatedTable;
    private Citizen m_Citizen;
    private bool m_IsOccupied;

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
            Debug.DrawRay( ray.origin, ray.direction );
            if ( Physics.Raycast( ray, out RaycastHit hitInfo, 2 ) )
            {
                if ( hitInfo.collider != null && hitInfo.collider.TryGetComponent( out Seat seat ) )
                {
                    seat.CheckTable_Instantiated();
                }
            }
        }
    }
    private void OnDestroy()
    {
        m_Restaurant.Tables.Remove( this );
    }

}
