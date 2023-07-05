public struct StorageItem 
{
    public Item AssosiatedItem { get; private set; }
    public int ItemSavedLifeTime { get; private set; }




    public StorageItem(Item assosiatedItem, int itemRegisteredLifeTime)
    {
        AssosiatedItem = assosiatedItem;
        ItemSavedLifeTime = itemRegisteredLifeTime;
    }
}