[System.Serializable]
public class ItemSlot
{
    public ItemData data;
    public int quantity;
    public ItemSlot( ItemData itemData, int quantity = 1 ) { this.data = itemData; this.quantity = quantity; }
    public void SwapValue(ItemSlot targetData)
    {
        (data, targetData.data) = (targetData.data, data);
        (quantity, targetData.quantity) = (targetData.quantity, quantity);
    }
    public ItemSlot() { }
}
