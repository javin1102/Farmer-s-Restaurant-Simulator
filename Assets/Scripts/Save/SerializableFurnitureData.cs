using System;
using SimpleJSON;

[Serializable]
public class SerializableFurnitureData : ISerializable
{
    public CustomTransform transform;
    public string ID; //resources item data id
    public int type;

    public SerializableFurnitureData(Furniture furniture)
    {
        this.transform = new(furniture.transform);
        this.ID = furniture.Data.ID;
        this.type = (int)((FurnitureData)furniture.Data).furnitureType;
    }

    public SerializableFurnitureData(JSONNode jsonNode)
    {
        this.transform = new(jsonNode["transform"]);
        this.ID = jsonNode["ID"];
        this.type = jsonNode["type"];
    }
    public SerializableFurnitureData(string id, int type, CustomTransform transform)
    {
        this.ID = id;
        this.type = type;
        this.transform = transform;
    }

    public virtual JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("ID", ID);
        jsonObject.Add("type", type);
        jsonObject.Add("transform", transform.Serialize());
        return jsonObject;
    }
}
