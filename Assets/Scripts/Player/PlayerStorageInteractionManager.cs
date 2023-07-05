using System.Collections.Generic;
using UnityEngine;

public class PlayerStorageInteractionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private List<StorageItemButton> _storageItemButtonsList;
    private List<StorageItem> _storageItemsList;




    private void OnEnable()
    {
        if (!_entityManager.PlayerPhotonview.IsMine)
        {
            return;
        }

        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        TryOpenStorageUI(gameEventType, data);
        AddItemToInventoryFromStorage(gameEventType, data);
    }

    private void TryOpenStorageUI(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.RequestStorageUIOpen)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.OpenStorageUI, data);
    }

    private void AddItemToInventoryFromStorage(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SubmitStorageItem)
        {
            return;
        }

        _storageItemButtonsList = (List<StorageItemButton>)data[0];
        _storageItemsList = (List<StorageItem>)data[1];

        bool dontHaveInventorySpace = !_entityInventoryManager.HaveEnoughInventorySpace;

        if (dontHaveInventorySpace)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        foreach (var selectedStorageItemButton in _storageItemButtonsList)
        {
            if (!_entityInventoryManager.HaveEnoughInventorySpace)
            {
                return;
            }

            AddItemToInventory(selectedStorageItemButton.AssosiatedStorageItem);
            RemoveItemFromSelectedCell(selectedStorageItemButton, (Sprite)data[2]);
            RemoveItemFromStorageList(_storageItemsList, selectedStorageItemButton.AssosiatedStorageItem);
        }

        PlaySoundEffect(0, 11);
    }

    private void AddItemToInventory(StorageItem storageItem)
    {
        _entityInventoryManager.AddItem(storageItem.AssosiatedItem);
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInventoryItemWithSavedLifetime(storageItem.AssosiatedItem, storageItem.ItemSavedLifeTime);
    }

    private void RemoveItemFromSelectedCell(StorageItemButton storageItemButton, Sprite emptyCellSprite)
    {
        storageItemButton.RemoveItem(emptyCellSprite);
    }

    private void RemoveItemFromStorageList(List<StorageItem> storageItemsList, StorageItem selectedStorageItem)
    {
        storageItemsList.Remove(selectedStorageItem);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}