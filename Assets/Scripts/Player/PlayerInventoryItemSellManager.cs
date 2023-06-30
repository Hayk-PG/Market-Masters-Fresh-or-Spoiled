using Photon.Pun;

public class PlayerInventoryItemSellManager : EntityInventoryItemSellManager
{
    private object[] _stockData = new object[2];
    private object[] _reputationOnSpoiledSaleData = new object[2];




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!_entityManager.PlayerPhotonview.IsMine)
        {
            return;
        }

        RetrieveConfirmedSaleItemData(gameEventType, data);
        SellSpoiledItems(gameEventType, data);
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
        UpdateReputationOnSpoiledSale(sellingItemQuantity, sellingItemSpoilPercentage);
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

    private void UpdateReputationOnSpoiledSale(int sellingItemQuantity, int sellingItemSpoilPercentage)
    {
        _reputationOnSpoiledSaleData[0] = sellingItemQuantity;
        _reputationOnSpoiledSaleData[1] = sellingItemSpoilPercentage;
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationOnSpoiledSale, _reputationOnSpoiledSaleData);
    }

    private void SellSpoiledItems(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.SellSpoiledItems)
        {
            return;
        }

        UISoundController.PlaySound(5, 1);
        photonView.RPC("SellSpoiledItemsRPC", RpcTarget.AllViaServer, (short)((int)data[0]), (byte)_entityIndexManager.TeamIndex);            
    }

    [PunRPC]
    private void SellSpoiledItemsRPC(short moneyAmount, byte targetTeam)
    {
        _stockData[0] = moneyAmount;
        _stockData[1] = (TeamIndex)targetTeam;
        GameEventHandler.RaiseEvent(GameEventType.UpdateMoneyRegardlessOfSale, _stockData);
    }
}