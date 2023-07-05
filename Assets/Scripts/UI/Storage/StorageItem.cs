public struct StorageItem 
{
    public Item AssociatedItem { get; private set; }
    public int InitialTurnCount { get; private set; }
    public int ItemSavedLifeTime { get; private set; }




    public StorageItem(Item associatedItem, int initialTurnCount, int itemRegisteredLifeTime)
    {
        AssociatedItem = associatedItem;
        InitialTurnCount = initialTurnCount;
        ItemSavedLifeTime = itemRegisteredLifeTime;
    }
}