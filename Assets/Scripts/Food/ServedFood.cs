using UnityEngine;

public class ServedFood 
{
    public GameObject foodGO;
    public int price;
    public ServedFood( GameObject foodGO, int price )
    {
        this.foodGO = foodGO;
        this.price = price;
    }
}
