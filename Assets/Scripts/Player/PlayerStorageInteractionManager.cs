using System.Collections.Generic;
using UnityEngine;

public class PlayerStorageInteractionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private List<StorageItemButton> _storageItemsList;




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

        _storageItemsList = (List<StorageItemButton>)data[0];
        bool dontHaveInventorySpace = !_entityInventoryManager.HaveEnoughInventorySpace;

        if (dontHaveInventorySpace)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        if (_entityInventoryManager.HaveEnoughInventorySpace)

        foreach (var selectedStorageItem in _storageItemsList)
        {
            if (!_entityInventoryManager.HaveEnoughInventorySpace)
            {
                return;
            }

            AddItemToInventory(selectedStorageItem.AssosiatedItem);
            RemoveStoredItem(allStorageItemsList: (List<Item>)data[1], selectedStorageItem.AssosiatedItem);
            RemoveSelectedStorageItem(selectedStorageItem, emptyCellSprite: (Sprite)data[2]);
        }

        PlaySoundEffect(0, 11);
    }

    private void AddItemToInventory(Item item)
    {
        _entityInventoryManager.AddItem(item);
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(item);
    }

    private void RemoveSelectedStorageItem(StorageItemButton selectedStorageItemButton, Sprite emptyCellSprite)
    {
        selectedStorageItemButton.RemoveItem(emptyCellSprite);
    }

    private void RemoveStoredItem(List<Item> allStorageItemsList, Item item)
    {
        allStorageItemsList.Remove(item);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}