using System.Collections.Generic;
using UnityEngine;

public class PlayerStorageInteractionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private bool IsPlayerTeamTurn => GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn == _entityIndexManager.TeamIndex;




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
        CloseStorageUIOnTurnUpdate(gameEventType);
        AddItemToInventoryFromStorage(gameEventType, data);
    }

    private void TryOpenStorageUI(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.RequestStorageUIOpen)
        {
            return;
        }

        if (IsPlayerTeamTurn)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.OpenStorageUI, data);
    }

    private void CloseStorageUIOnTurnUpdate(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.CloseStorageUI);
    }

    private void AddItemToInventoryFromStorage(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SubmitStorageItem)
        {
            return;
        }

        if (IsPlayerTeamTurn)
        {
            return;
        }

        foreach (var selectedStorageItem in (List<StorageItemButton>)data[0])
        {
            if (!_entityInventoryManager.HaveEnoughInventorySpace)
            {
                return;
            }

            AddItemToInventory(selectedStorageItem.AssosiatedItem);
            RemoveStoredItem(allStorageItemsList: (List<Item>)data[1], selectedStorageItem.AssosiatedItem);
            RemoveSelectedStorageItem(selectedStorageItem, emptyCellSprite: (Sprite)data[2]);
        }
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
}