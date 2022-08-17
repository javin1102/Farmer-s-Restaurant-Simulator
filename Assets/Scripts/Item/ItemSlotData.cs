[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;
    public ItemSlotData( ItemData itemData, int quantity = 1 ) { this.itemData = itemData; this.quantity = quantity; }
    public void SwapValue(ItemSlotData targetData)
    {
        (itemData, targetData.itemData) = (targetData.itemData, itemData);
        (quantity, targetData.quantity) = (targetData.quantity, quantity);
    }
    public ItemSlotData() { }
}
