using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class StorageManagerButton : InventoryItemDragDropUIResponder
{
    [Header("UI Elements")]
    [SerializeField] private BtnTxt _itemsCountText;

    private PlayerInventoryItemButton _inventoryItemButton;
    private int _storageCapacity = 8;
    private object[] _storedItemsData = new object[1];
    private object[] _submittedStorageItemsData = new object[4];

    internal List<StorageItem> StorageItemsList { get; private set; } = new List<StorageItem>();




    private void Awake()
    {
        UpdateItemsCountText();
    }

    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _button.OnSelect += OnSelect;
    }

    /// <summary>
    /// Handles game events and performs appropriate actions based on the event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleInventoryItemDragNDropEvent(gameEventType, data);
        SubmitStorageItem(gameEventType, data);
    }

    /// <summary>
    /// Handles the button selection event.
    /// </summary>
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
        GetInventoryItemButton((PlayerInventoryItemButton)data[1]);
        base.ExecuteOnHover(data);
    }

    protected override void CalculateIconAlpha(Vector2 mousePosition, out float alpha)
    {
        alpha = 0.5f;
    }

    /// <summary>
    /// Stores the inventory item in the storage based on the current conditions.
    /// </summary>
    private void StoreItem()
    {
        if (_inventoryItemButton == null || _inventoryItemButton.AssociatedItem == null || _inventoryItemButton.ItemSpoilPercentage > 20 || StorageItemsList.Count >= _storageCapacity)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        int currentTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount;
        int itemSavedLifetime = _inventoryItemButton.ItemLifetime;
        StorageItemsList.Add(new StorageItem(_inventoryItemButton.AssociatedItem, currentTurnCount, itemSavedLifetime));
        _inventoryItemButton.DestroySpoiledItemOnSeparateSale();
        UpdateItemsCountText();
        PlaySoundEffect(7, 3);
    }

    /// <summary>
    /// Retrieves the inventory item button associated with the storage manager button.
    /// </summary>
    /// <param name="playerInventoryItemButton">The inventory item button to retrieve.</param>
    private void GetInventoryItemButton(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _inventoryItemButton = playerInventoryItemButton;
    }

    /// <summary>
    /// Submits the storage item for verification when the appropriate game event is triggered.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
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

    /// <summary>
    /// Plays a sound effect based on the provided list index and clip index.
    /// </summary>
    /// <param name="listIndex">The index of the sound effect list.</param>
    /// <param name="clipIndex">The index of the sound effect clip.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}