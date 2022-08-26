using UnityEngine;

public class RestaurantManager : MonoBehaviour
{

    public static RestaurantManager Instance { get => m_Instance; }
    private static RestaurantManager m_Instance;


    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
    }


}
