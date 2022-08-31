using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StockIngredient 
{
    public int quantity;
    public ItemData ingredient;
    public StockIngredient( ItemData ingredient, int quantity = 1 )
    {
        this.quantity = quantity;
        this.ingredient = ingredient;
    }
}
