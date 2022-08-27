using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public List<Table> Tables { get => m_Tables; }

    private static RestaurantManager m_Instance;

    [SerializeField] private List<Table> m_Tables = new();

    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
    }

    public bool FindUnoccupiedTable( out Table table )
    {
        foreach ( Table t in m_Tables )
        {
            if ( !t.IsOccupied )
            {
                Debug.Log( "YES" );
                table = t;
                return true;
            }
        }

        table = null;
        return false;
    }
}
