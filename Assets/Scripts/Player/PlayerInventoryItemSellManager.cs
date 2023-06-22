using Photon.Pun;

public class PlayerInventoryItemSellManager : EntityInventoryItemSellManager
{
    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        TryRetrieveDataAndExecute(gameEventType, data);
    }

    private void TryRetrieveDataAndExecute(GameEventType gameEventType, object[] data)
    {
        bool canRetrieveData = gameEventType == GameEventType.ConfirmInventoryItemForSale && photonView.IsMine;

        if (!canRetrieveData)
        {
            return;
        }

        int sellingItemQuantity = (int)data[0];
        int sellingItemId = (int)data[1];

        if (sellingItemQuantity == 0)
        {
            return;
        }

        PublishConfirmedItemForSale(sellingItemQuantity, sellingItemId);
        RemoveSoldItems(sellingItemQuantity, sellingItemId);
    }

    public override void PublishConfirmedItemForSale(int sellingItemQuantity, int sellingItemId)
    {
        photonView.RPC("PublishConfirmedItemForSaleRPC", RpcTarget.AllViaServer, _entityManager.EntityName, _entityManager.EntityActorNumber, sellingItemQuantity, sellingItemId);
    }

    [PunRPC]
    private void PublishConfirmedItemForSaleRPC(string entityName, int entityActorNumber, int sellingItemQuantity, int sellingItemId)
    {
        _sellingItemData[0] = entityName;
        _sellingItemData[1] = entityActorNumber;
        _sellingItemData[2] = sellingItemQuantity;
        _sellingItemData[3] = sellingItemId;
        GameEventHandler.RaiseEvent(GameEventType. PublishSellingItemQuantity, _sellingItemData);
    }
}