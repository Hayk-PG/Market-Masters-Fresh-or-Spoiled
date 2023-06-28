using Photon.Pun;

public class PlayerInventoryItemSellManager : EntityInventoryItemSellManager
{
    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        RetrieveConfirmedSaleItemData(gameEventType, data);
        GetMoneyFromSellingSpoiledItems(gameEventType, data);
    }

    /// <summary>
    /// Retrieves the data of the confirmed sale item and triggers the appropriate actions.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void RetrieveConfirmedSaleItemData(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.ConfirmInventoryItemForSale)
        {
            return;
        }

        int sellingItemQuantity = (int)data[0];
        int sellingItemId = (int)data[1];
        int sellingItemSpoilPercentage = (int)data[2];

        if (sellingItemQuantity == 0)
        {
            return;
        }

        PublishConfirmedItemForSale((byte)sellingItemQuantity, (byte)sellingItemId, (byte)sellingItemSpoilPercentage);
        RemoveSoldItems((byte)sellingItemQuantity, (byte)sellingItemId);
    }

    private void GetMoneyFromSellingSpoiledItems(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.SellSpoiledItems)
        {
            return;
        }

        int moneyAmount = (int)data[0];
        GameSceneReferences.Manager.RemoteRPCWrapper.UpdateMoneyRegardlessOfSale((short)moneyAmount, _entityIndexManager.TeamIndex);
        UISoundController.PlaySound(5, 1);
    }

    public override void PublishConfirmedItemForSale(byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage)
    {
        photonView.RPC("PublishConfirmedItemForSaleRPC", RpcTarget.AllViaServer, _entityManager.EntityName, _entityManager.EntityActorNumber, sellingItemQuantity, sellingItemId, sellingItemSpoilPercentage);
    }

    [PunRPC]
    private void PublishConfirmedItemForSaleRPC(string entityName, int entityActorNumber, byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage)
    {
        _sellingItemData[0] = entityName;
        _sellingItemData[1] = entityActorNumber;
        _sellingItemData[2] = sellingItemQuantity;
        _sellingItemData[3] = sellingItemId;
        _sellingItemData[4] = sellingItemSpoilPercentage;
        GameEventHandler.RaiseEvent(GameEventType.PublishSellingItemQuantity, _sellingItemData);
    }
}