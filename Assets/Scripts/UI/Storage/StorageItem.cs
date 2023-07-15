public class StorageItem 
{
    public Item AssociatedItem { get; private set; }
    public int InitialTurnCount { get; private set; }
    public int ItemSavedLifeTime { get; private set; }
    public int StorageFeeProcessTurnCount { get; private set; }




    public StorageItem(Item associatedItem, int initialTurnCount, int itemRegisteredLifeTime)
    {
        AssociatedItem = associatedItem;
        InitialTurnCount = initialTurnCount;
        ItemSavedLifeTime = itemRegisteredLifeTime;
        StorageFeeProcessTurnCount = initialTurnCount;
    }

    public void UpdateStorageFeeProcessTurnCount(int storageFeeProcessTurnCount)
    {
        StorageFeeProcessTurnCount = storageFeeProcessTurnCount;
    }
}