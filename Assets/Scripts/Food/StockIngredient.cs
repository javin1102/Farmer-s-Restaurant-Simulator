using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StockIngredient 
{
    public int quantity;
    public IngredientData data;
    public StockIngredient( IngredientData ingredient, int quantity = 1 )
    {
        this.quantity = quantity;
        this.data = ingredient;
    }
}
