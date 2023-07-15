using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class StorageManagerButton : InventoryItemDragDropUIResponder
{
    [Header("UI Elements")]
    [SerializeField] private BtnTxt _itemsCountText;

    [Header("Error Message")]
    [SerializeField] private ErrorMessageGroup _errorMessageGroup;

    private int _storageCapacity = 8;
    private object[] _storedItemsData = new object[1];
    private object[] _submittedStorageItemsData = new object[4];

    internal List<StorageItem> StorageItemsList { get; private set; } = new List<StorageItem>();
    private bool isItemStoringRestricted => _inventoryItemButton == null || _inventoryItemButton.AssociatedItem == null ||
                                            _inventoryItemButton.ItemSpoilPercentage > 20 || StorageItemsList.Count >= _storageCapacity;




    private void Awake()
    {
        UpdateItemsCountText();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _button.OnSelect += OnSelect;
    }

    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        base.HandleInventoryItemDragNDropEvent(gameEventType, data);
        SubmitStorageItem(gameEventType, data);
    }

    private void OnSelect()
    {
        if (IsButtonsGroupHidden)
        {
            return;
        }

        RequestStorageUIOpen();
    }

    private void RequestStorageUIOpen()
    {
        _storedItemsData[0] = StorageItemsList;
        GameEventHandler.RaiseEvent(GameEventType.RequestStorageUIOpen, _storedItemsData);
    }

    protected override void ExecuteOnDragRelease(object[] data)
    {
        StoreItem();
        base.ExecuteOnDragRelease(data);
    }

    protected override void ExecuteOnHover(object[] data)
    {
        base.ExecuteOnHover(data);
        ToggleItemStoringDisplay(sprite: isItemStoringRestricted ? _iconSprites[1] : _iconSprites[0]);
    }

    private void StoreItem()
    {
        if (isItemStoringRestricted)
        {
            DisplayError(errorIndex: _inventoryItemButton == null || _inventoryItemButton.AssociatedItem == null ? 0 : _inventoryItemButton.ItemSpoilPercentage > 20 ? 1 : 2);
            return;
        }

        int currentTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount;
        int itemSavedLifetime = _inventoryItemButton.ItemLifetime;
        AddItemToList(currentTurnCount, itemSavedLifetime);
        RemoveItemFromInventory();
        UpdateItemsCountText();
        PlaySoundEffect(7, 3);
    }

    private void AddItemToList(int currentTurnCount, int itemSavedLifetime)
    {
        StorageItemsList.Add(new StorageItem(_inventoryItemButton.AssociatedItem, currentTurnCount, itemSavedLifetime));
    }

    private void RemoveItemFromInventory()
    {
        _inventoryItemButton.DestroySpoiledItemOnSeparateSale();
    }

    private void DisplayError(int errorIndex)
    {
        _errorMessageGroup.ErrorMessages[errorIndex] = GlobalFunctions.PartiallyTransparentText(_errorMessageGroup.ErrorMessages[errorIndex]);
        _errorMessageGroup.DisplayErrorMessage(errorIndex, errorIndex);
    }

    private void SubmitStorageItem(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SendStorageItemForVerification)
        {
            return;
        }

        _submittedStorageItemsData[0] = (List<StorageItemButton>)data[0];
        _submittedStorageItemsData[1] = StorageItemsList;
        _submittedStorageItemsData[2] = (Sprite)data[1];
        _submittedStorageItemsData[3] = (System.Action)delegate { UpdateItemsCountText(); };
        GameEventHandler.RaiseEvent(GameEventType.SubmitStorageItem, _submittedStorageItemsData);
    }

    private void UpdateItemsCountText()
    {
        _itemsCountText.SetButtonTitle($"{GlobalFunctions.PartiallyTransparentText(StorageItemsList.Count.ToString())}/{GlobalFunctions.WhiteColorText(_storageCapacity.ToString())}");
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}