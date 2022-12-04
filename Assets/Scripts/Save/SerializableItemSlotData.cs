using SimpleJSON;

public class SerializableItemSlotData : ISerializable
{
    public string ID;
    public int type;
    public int slotIndex;
    public int quantity;

    public SerializableItemSlotData(ItemSlot itemSlot, int slotIndex)
    {
        this.ID = itemSlot.data.ID;
        this.type = (int)itemSlot.data.itemType;
        this.slotIndex = slotIndex;
        this.quantity = itemSlot.quantity;
    }

    public SerializableItemSlotData(JSONNode jsonNode)
    {
        this.ID = jsonNode["ID"];
        this.type = jsonNode["type"];
        this.slotIndex = jsonNode["slotIndex"];
        this.quantity = jsonNode["quantity"];
    }

    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("ID", ID);
        jsonObject.Add("type", type);
        jsonObject.Add("slotIndex", slotIndex);
        jsonObject.Add("quantity", quantity);
        return jsonObject;
    }
}
