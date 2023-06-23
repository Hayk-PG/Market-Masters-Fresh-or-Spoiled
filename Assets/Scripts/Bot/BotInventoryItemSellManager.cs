using System.Collections;
using UnityEngine;

public class BotInventoryItemSellManager : EntityInventoryItemSellManager
{
    private bool _hasAlreadyPublished;

    public string BotName => _entityManager.EntityName;
    public int BotActorNumber => _entityManager.EntityActorNumber;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnGameTurnUpdate(gameEventType, data);
    }

    /// <summary>
    /// Handles the game turn update event and checks if the confirmed sale item can be published.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void OnGameTurnUpdate(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        SetPublishedStatus(false);
        StartCoroutine(WaitAndExecute(currentTurnTeamIndex: (TeamIndex)data[2]));
    }

    /// <summary>
    /// Waits for a random duration and executes the item sale process if the conditions are met.
    /// </summary>
    /// <param name="currentTurnTeamIndex">The current team index.</param>
    /// <returns>An IEnumerator used for coroutine execution.</returns>
    private IEnumerator WaitAndExecute(TeamIndex currentTurnTeamIndex)
    {
        bool canPublishConfirmedItemForSale = currentTurnTeamIndex == _entityIndexManager.TeamIndex && !_hasAlreadyPublished;

        if (!canPublishConfirmedItemForSale)
        {
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(0f, 7f));

        if (currentTurnTeamIndex != _entityIndexManager.TeamIndex)
        {
            yield break;
        }

        CountSelectedItemQuantity(out int selectedItemQuantity, out int targetItemId, out int targetItemSpoilPercentage);
        CheckItemQuantityAndProcess((byte)selectedItemQuantity, (byte)targetItemId, (byte)targetItemSpoilPercentage);
    }

    /// <summary>
    /// Counts the quantity of the selected item in the inventory.
    /// </summary>
    /// <param name="selectedItemQuantity">The quantity of the selected item.</param>
    /// <param name="targetItemId">The ID of the target item.</param>
    /// <param name="targetItemSpoilPercentage">The spoil percentage of the target item.</param>
    private void CountSelectedItemQuantity(out int selectedItemQuantity, out int targetItemId, out int targetItemSpoilPercentage)
    {
        selectedItemQuantity = 0;
        targetItemId = GameSceneReferences.Manager.ItemsBuyerManager.BuyingItem.ID;
        targetItemSpoilPercentage = 0;

        foreach (var InventoryItem in _entityInventoryManager.InventoryItems)
        {
            bool isInventoryItemBuyingItem = InventoryItem.ID == targetItemId;

            if (isInventoryItemBuyingItem)
            {
                selectedItemQuantity++;
            }
        }
    }

    /// <summary>
    /// Checks the item quantity and processes the confirmed sale item for publishing.
    /// </summary>
    /// <param name="selectedItemQuantity">The quantity of the selected item.</param>
    /// <param name="targetItemId">The ID of the target item.</param>
    /// <param name="targetItemSpoilPercentage">The spoil percentage of the target item.</param>
    private void CheckItemQuantityAndProcess(byte selectedItemQuantity, byte targetItemId, byte targetItemSpoilPercentage)
    {
        if(selectedItemQuantity < 1)
        {
            return;
        }

        GameSceneReferences.Manager.RemoteRPCWrapper.PublishBotConfirmedItemForSale(BotName, BotActorNumber, selectedItemQuantity, targetItemId, targetItemSpoilPercentage);
        SetPublishedStatus(true);
    }

    /// <summary>
    /// Sets the published status of the confirmed sale item.
    /// </summary>
    /// <param name="hasAlreadyPublished">A flag indicating if the item has already been published.</param>
    private void SetPublishedStatus(bool hasAlreadyPublished)
    {
        _hasAlreadyPublished = hasAlreadyPublished;
    }

    public override void PublishConfirmedItemForSale(byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage)
    {
        _sellingItemData[0] = BotName;
        _sellingItemData[1] = BotActorNumber;
        _sellingItemData[2] = sellingItemQuantity;
        _sellingItemData[3] = sellingItemId;
        _sellingItemData[4] = sellingItemSpoilPercentage;

        GameEventHandler.RaiseEvent(GameEventType.PublishSellingItemQuantity, _sellingItemData);
    }
}