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

    protected override void OnInstantiate()
    {
        m_InstantiatedSeat = m_InstantiatedGO.GetComponent<Seat>();
        m_Restaurant.Seats.Add(m_InstantiatedSeat);
        CheckForTable(m_InstantiatedSeat);
    }

    public void CheckForTable(Seat seat)
    {
        Ray ray = new(seat.transform.position, seat.transform.forward);
        RaycastHit[] hits = Physics.BoxCastAll(seat.transform.position, Vector3.one / 3, ray.direction, Quaternion.identity, 1);
        foreach (var hitInfo in hits)
        {
            if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Table table))
            {
                if (!table.HasMaxSeats)
                {
                    table.Seats.Add(seat);
                    seat.m_Table = table;
                }

            }
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
        if (!m_IsInstantiated) return;
        if (m_Table != null) m_Table.Seats.Remove(this);
        m_Restaurant = RestaurantManager.Instance;
        m_Restaurant.Seats.Remove(this);
    }


}
