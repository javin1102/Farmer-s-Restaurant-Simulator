using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public class SerializableRecipeData : ISerializable
{
    public string ID;
    public bool isUnlock;
    public bool isSelling;

    public SerializableRecipeData(string ID, bool isUnlock, bool isSelling)
    {
        this.ID = ID;
        this.isUnlock = isUnlock;
        this.isSelling = isSelling;
    }

    public SerializableRecipeData(JSONNode jsonNode)
    {
        this.ID = jsonNode["ID"];
        this.isUnlock = jsonNode["isUnlock"];
        this.isSelling = jsonNode["isSelling"];
    }



    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("ID", ID);
        jsonObject.Add("isUnlock", isUnlock);
        jsonObject.Add("isSelling", isSelling);
        return jsonObject;
    }
}
