using System.Collections.Generic;
using UnityEngine;

public class PlayerStorageInteractionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private List<StorageItemButton> _selectedStorageItemsList;
    private List<StorageItem> _storageItemsList;
    private System.Action UpdateSorageItemsCountTextCallback;




    private void OnEnable()
    {
        if (!_entityManager.PlayerPhotonview.IsMine)
        {
            return;
        }

        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles game events and performs appropriate actions based on the event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        TryOpenStorageUI(gameEventType, data);
        AddItemToInventoryFromStorage(gameEventType, data);
    }

    /// <summary>
    /// Tries to open the storage UI when the appropriate game event is triggered.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void TryOpenStorageUI(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.RequestStorageUIOpen)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.OpenStorageUI, data);
    }

    /// <summary>
    /// Adds items from storage to the player's inventory when the appropriate game event is triggered.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void AddItemToInventoryFromStorage(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SubmitStorageItem)
        {
            return;
        }

        SubmittedStorageItemData submittedStorageItemData = (SubmittedStorageItemData)data[0];
        _selectedStorageItemsList = submittedStorageItemData.SelectedStorageItemButtonData.SelectedStorageItemsList;
        _storageItemsList = submittedStorageItemData.StorageItemsList;
        UpdateSorageItemsCountTextCallback = submittedStorageItemData.UpdateItemsCountTextCallback;

        bool dontHaveInventorySpace = !_entityInventoryManager.HaveEnoughInventorySpace;

        if (dontHaveInventorySpace)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        foreach (var selectedStorageItemButton in _selectedStorageItemsList)
        {
            if (!_entityInventoryManager.HaveEnoughInventorySpace)
            {
                break;
            }

            AddItemToInventory(selectedStorageItemButton.AssociatedStorageItem);
            RemoveItemFromStorageList(_storageItemsList, selectedStorageItemButton.AssociatedStorageItem);
            RemoveItemFromSelectedCell(selectedStorageItemButton, submittedStorageItemData.SelectedStorageItemButtonData.EmptyCellSprite);
        }

        UpdateSorageItemsCountTextCallback();
        PlaySoundEffect(0, 11);
    }

    /// <summary>
    /// Adds an item from storage to the player's inventory.
    /// </summary>
    /// <param name="storageItem">The storage item to add.</param>
    private void AddItemToInventory(StorageItem storageItem)
    {
        _entityInventoryManager.AddItem(storageItem.AssociatedItem);
        AddItemToInventoryUI(storageItem);
    }

    /// <summary>
    /// Adds an item from storage to the player's inventory UI.
    /// </summary>
    /// <param name="storageItem">The storage item to add.</param>
    private void AddItemToInventoryUI(StorageItem storageItem)
    {
        int itemNewLifetime = (GameSceneReferences.Manager.GameTurnManager.TurnCount - storageItem.InitialTurnCount) + storageItem.ItemSavedLifeTime;
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(storageItem.AssociatedItem, itemNewLifetime);
    }

    /// <summary>
    /// Removes the item from the selected storage cell and updates its UI with an empty cell sprite.
    /// </summary>
    /// <param name="storageItemButton">The storage item button.</param>
    /// <param name="emptyCellSprite">The empty cell sprite.</param>
    private void RemoveItemFromSelectedCell(StorageItemButton storageItemButton, Sprite emptyCellSprite)
    {
        storageItemButton.RemoveItem(emptyCellSprite);
    }

    /// <summary>
    /// Removes the selected storage item from the storage item list.
    /// </summary>
    /// <param name="storageItemsList">The storage item list.</param>
    /// <param name="selectedStorageItem">The selected storage item.</param>
    private void RemoveItemFromStorageList(List<StorageItem> storageItemsList, StorageItem selectedStorageItem)
    {
        storageItemsList.Remove(selectedStorageItem);
    }

    /// <summary>
    /// Plays a sound effect with the specified list index and clip index.
    /// </summary>
    /// <param name="listIndex">The index of the sound list.</param>
    /// <param name="clipIndex">The index of the sound clip.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}