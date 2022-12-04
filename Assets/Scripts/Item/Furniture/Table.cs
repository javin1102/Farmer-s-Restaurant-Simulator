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
        m_FoodPlaces = transform.GetChild(1);
    }

    public Transform GetFoodPlace(Seat seat)
    {
        foreach (Transform child in m_FoodPlaces)
        {
            if (child.forward == seat.transform.forward)
            {
                return child;
            }
        }

        return null;
    }


    protected override void OnInstantiate()
    {

        m_InstantiatedTable = m_InstantiatedGO.GetComponent<Table>();
        m_Restaurant.Tables.Add(m_InstantiatedGO.GetComponent<Table>());
        CheckSeat(m_InstantiatedTable);
    }

    public void CheckSeat(Table table)
    {
        //Ray in 4 dir
        Ray[] rays = new Ray[4]
        {
            new Ray(table.transform.position, -table.transform.forward),
            new Ray(table.transform.position, table.transform.forward),
            new Ray(table.transform.position, table.transform.right),
            new Ray(table.transform.position, -table.transform.right),
        };

        foreach (Ray ray in rays)
        {
            RaycastHit[] hits = Physics.BoxCastAll(table.transform.position, Vector3.one / 2, ray.direction, Quaternion.identity, 1);
            foreach (var hitInfo in hits)
            {
                if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Seat seat))
                {
                    seat.Table = table;
                    m_Seats.Add(seat);
                }
            }
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
        if (!m_IsInstantiated) return;
        m_Restaurant = RestaurantManager.Instance;
        m_Restaurant.Tables.Remove(this);
    }


}
