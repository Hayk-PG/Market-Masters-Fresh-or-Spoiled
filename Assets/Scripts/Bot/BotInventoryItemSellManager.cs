using System.Collections;
using UnityEngine;

public class BotInventoryItemSellManager : EntityInventoryItemSellManager
{
    private bool _hasAlreadyPublished;

    public string BotName => _entityManager.EntityName;
    public int BotActorNumber => _entityManager.EntityActorNumber;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        TryRetrieveData(gameEventType, data);
    }

    private void TryRetrieveData(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        ToggleHasAlreadyPublished(false);
        TeamIndex currentTurnTeamIndex = (TeamIndex)data[2];
        bool canPublishConfirmedItemForSale = currentTurnTeamIndex == _entityIndexManager.TeamIndex && !_hasAlreadyPublished;     

        if (!canPublishConfirmedItemForSale)
        {
            return;
        }

        StartCoroutine(DoubleCheckAndExecuteAfterDelay(currentTurnTeamIndex));
    }

    private void ToggleHasAlreadyPublished(bool hasAlreadyPublished)
    {
        _hasAlreadyPublished = hasAlreadyPublished;
    }

    private IEnumerator DoubleCheckAndExecuteAfterDelay(TeamIndex currentTurnTeamIndex)
    {
        yield return new WaitForSeconds(Random.Range(0f, 7f));

        if(currentTurnTeamIndex != _entityIndexManager.TeamIndex)
        {
            yield break;
        }

        CountSelectedItemQuantity(out int selectedItemQuantity, out int targetItemId);
        PublishConfirmedItemForSale((byte)selectedItemQuantity, (byte)targetItemId);
    }

    private void CountSelectedItemQuantity(out int selectedItemQuantity, out int targetItemId)
    {
        selectedItemQuantity = 0;
        targetItemId = GameSceneReferences.Manager.ItemsBuyerManager.BuyingItem.ID;

        foreach (var InventoryItem in _entityInventoryManager.InventoryItems)
        {
            bool isInventoryItemBuyingItem = InventoryItem.ID == targetItemId;

            if (isInventoryItemBuyingItem)
            {
                if(selectedItemQuantity == 0)
                {
                    selectedItemQuantity++;
                    continue;
                }

                selectedItemQuantity = Random.Range(0, 2) < 1 ? selectedItemQuantity + 1 : selectedItemQuantity + 0;
            }
        }
    }

    private void PublishConfirmedItemForSale(byte selectedItemQuantity, byte targetItemId)
    {
        if(selectedItemQuantity < 1)
        {
            return;
        }

        byte quantity = selectedItemQuantity;
        byte id = targetItemId;
        GameSceneReferences.Manager.RemoteRPCWrapper.PublishBotConfirmedItemForSale(BotName, BotActorNumber, quantity, id);
        ToggleHasAlreadyPublished(true);
    }

    public override void PublishConfirmedItemForSale(int sellingItemQuantity, int sellingItemId)
    {
        _sellingItemData[0] = BotName;
        _sellingItemData[1] = BotActorNumber;
        _sellingItemData[2] = sellingItemQuantity;
        _sellingItemData[3] = sellingItemId;
        
        GameEventHandler.RaiseEvent(GameEventType.PublishSellingItemQuantity, _sellingItemData);
    }
}