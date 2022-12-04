using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SerializableStockIngredientData : ISerializable
{
    public int quantity;
    public string ID;
    public SerializableStockIngredientData(string ID, int quantity)
    {
        this.ID = ID;
        this.quantity = quantity;
    }

    public SerializableStockIngredientData(JSONNode jsonNode)
    {
        this.ID = jsonNode["ID"];
        this.quantity = jsonNode["quantity"];
    }

    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("ID", ID);
        jsonObject.Add("quantity", quantity);
        return jsonObject;
    }
}
