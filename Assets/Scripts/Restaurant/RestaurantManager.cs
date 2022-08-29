using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public List<Table> Tables { get => m_Tables; }
    public List<Seat> UnoccupiedSeats { get => m_Seats; }

    private static RestaurantManager m_Instance;

    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private  List<Seat> m_Seats = new();

    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
    }
    private int RandomIndex { get => Random.Range( 0, m_Seats.Count ); }
    public bool FindUnoccupiedSeat( out Seat seat )
    {
        if (m_Seats.Count == 0 )
        {
            seat = null;
            return false;
        }

        seat = m_Seats[RandomIndex];
        return true;
    }
}
