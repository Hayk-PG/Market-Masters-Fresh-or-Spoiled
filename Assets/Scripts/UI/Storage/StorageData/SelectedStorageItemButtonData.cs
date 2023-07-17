using System.Collections.Generic;
using UnityEngine;

public struct SelectedStorageItemButtonData 
{
    public List<StorageItemButton> SelectedStorageItemsList { get; private set; }
    public Sprite EmptyCellSprite { get; private set; }




    public SelectedStorageItemButtonData(List<StorageItemButton> selectedStorageItemsList, Sprite emptyCellSprite)
    {
        SelectedStorageItemsList = selectedStorageItemsList;
        EmptyCellSprite = emptyCellSprite;
    }
}