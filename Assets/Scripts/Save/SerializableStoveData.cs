using SimpleJSON;

public class SerializableStoveData : SerializableFurnitureData
{
    public bool hasChef;

    public SerializableStoveData(Stove furniture) : base(furniture)
    {
        this.hasChef = furniture.Chef;
    }
    public override JSONObject Serialize()
    {
        JSONObject jsonObject = base.Serialize();
        jsonObject.Add("hasChef", hasChef);
        return jsonObject;
    }
}