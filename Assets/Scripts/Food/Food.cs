using UnityEngine;

public class Food : MonoBehaviour
{
    public Seat Seat { get => m_Seat; set => m_Seat =  value ; }
    public FoodData Data { get => m_Data; }

    [SerializeField] private FoodData m_Data;
    [SerializeField] private Seat m_Seat;
}
