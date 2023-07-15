using System.Collections.Generic;
using UnityEngine;
using Pautik;
using System.Linq;

public class EntityInventoryPublisher : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityInventoryManager _entityInventoryManager;

    protected object[] _inventoryData = new object[1];
    protected byte[] _inventoryItemsIdArray;
    protected HashSet<byte> _inventoryItemsIdHashSet;




    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected virtual void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PublishInventoryItemsId(gameEventType);
    }

    protected virtual void PublishInventoryItemsId(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (!IsAllowed())
        {
            return;
        }

        if (!IsTargetTurn())
        {
            return;
        }

        if (IsInventoryEmpty())
        {
            return;
        }

        GetInventoryItemsId();
        ConvertInventoryItemsIdToArray();
        PublishInventoryData(_inventoryItemsIdArray);
    }

    protected virtual bool IsAllowed()
    {
        return true;
    }

    protected bool IsTargetTurn()
    {
        return GameSceneReferences.Manager.GameTurnManager.TurnCount == 1 || (GameSceneReferences.Manager.GameTurnManager.TurnCount + 2) % 10 == 0;
    }

    protected bool IsInventoryEmpty()
    {
        return _entityInventoryManager.InventoryItems.Count == 0;
    }

    protected void GetInventoryItemsId()
    {
        _inventoryItemsIdHashSet = new HashSet<byte>();
        GlobalFunctions.Loop<Item>.Foreach(_entityInventoryManager.InventoryItems.ToArray(), inventoryItem => _inventoryItemsIdHashSet.Add((byte)inventoryItem.ID));
    }

    protected void ConvertInventoryItemsIdToArray()
    {
        _inventoryItemsIdArray = _inventoryItemsIdHashSet.ToArray();
    }

    protected virtual void PublishInventoryData(byte[] inventoryItemsIdArray)
    {
        _inventoryData[0] = inventoryItemsIdArray;
        GameEventHandler.RaiseEvent(GameEventType.PublishInventoryData, _inventoryData);
    }
}