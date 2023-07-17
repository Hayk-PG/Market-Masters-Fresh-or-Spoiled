using System;
using System.Collections.Generic;

public struct SubmittedStorageItemData
{
    public SelectedStorageItemButtonData SelectedStorageItemButtonData { get; private set; }
    public List<StorageItem> StorageItemsList { get; private set; }
    public Action UpdateItemsCountTextCallback { get; private set; }




    public SubmittedStorageItemData(SelectedStorageItemButtonData selectedStorageItemButtonData, List<StorageItem> storageItemsList, Action updateItemsCountTextCallback)
    {
        SelectedStorageItemButtonData = selectedStorageItemButtonData;
        StorageItemsList = storageItemsList;
        UpdateItemsCountTextCallback = updateItemsCountTextCallback;
    }
}