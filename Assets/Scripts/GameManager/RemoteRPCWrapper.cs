using Photon.Pun;
using Pautik;

public class RemoteRPCWrapper : MonoBehaviourPun
{
    private object[] _botNumericChoiceData = new object[3];
    private object[] _overrideGameTimeData = new object[1];
    private object[] _combinedSellingItemData = new object[3];
    private object[] _stockData = new object[2];

    private bool IsControllerMasterClient => MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);




    /// <summary>
    /// Initializes the bot inventory for a specific entity.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="entityActorNumber">The actor number of the entity.</param>
    /// <param name="itemsIndexes">The indexes of the items to be added to the bot inventory.</param>
    public void InitializeBotInventory(string entityName, int entityActorNumber, byte[] itemsIndexes)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("InitializeBotInventoryRPC", RpcTarget.AllViaServer, entityName, entityActorNumber, itemsIndexes);
    }

    [PunRPC]
    private void InitializeBotInventoryRPC(string entityName, int entityActorNumber, byte[] itemsIndexes)
    {
        BotInventoryManager botInventoryManager = GlobalFunctions.ObjectsOfType<BotInventoryManager>.Find(bot => bot.BotName == entityName && bot.BotActorNumber == entityActorNumber);

        if (botInventoryManager == null)
        {
            return;
        }

        for (int i = 0; i < itemsIndexes.Length; i++)
        {
            int itemIndex = itemsIndexes[i];
            botInventoryManager.AddItem(item: GameSceneReferences.Manager.Items.Collection[itemIndex]);
        }
    }

    /// <summary>
    /// Publishes a confirmed item for sale by a bot.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="entityActorNumber">The actor number of the entity.</param>
    /// <param name="sellingItemQuantity">The quantity of the selling item.</param>
    /// <param name="sellingItemId">The ID of the selling item.</param>
    /// <param name="sellingItemSpoilPercentage">The spoil percentage of the selling item.</param>
    public void PublishBotConfirmedItemForSale(string entityName, int entityActorNumber, byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("PublishBotConfirmedItemForSaleRPC", RpcTarget.AllViaServer, entityName, entityActorNumber, sellingItemQuantity, sellingItemId, sellingItemSpoilPercentage);
    }

    [PunRPC]
    private void PublishBotConfirmedItemForSaleRPC(string entityName, int entityActorNumber, byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage)
    {
        BotInventoryItemSellManager botInventoryItemSellManager = GlobalFunctions.ObjectsOfType<BotInventoryItemSellManager>.Find(bot => bot.BotName == entityName && bot.BotActorNumber == entityActorNumber);
        botInventoryItemSellManager?.PublishConfirmedItemForSale(sellingItemQuantity, sellingItemId, sellingItemSpoilPercentage);
        botInventoryItemSellManager?.RemoveSoldItems(sellingItemQuantity, sellingItemId);
    }

    /// <summary>
    /// Publishes the combined selling item quantity for a team.
    /// </summary>
    /// <param name="data">The data containing the selling item quantities.</param>
    public void PublishTeamCombinedSellingItemQuantity(object[] data)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        byte[] receivedData = new byte[] { ((byte)(int)data[0]), ((byte)(int)data[1]), ((byte)(int)data[2]), };
        photonView.RPC("PublishTeamCombinedSellingItemQuantityRPC", RpcTarget.AllViaServer, receivedData);
    }

    [PunRPC]
    private void PublishTeamCombinedSellingItemQuantityRPC(byte[] data)
    {
        _combinedSellingItemData[0] = data[0];
        _combinedSellingItemData[1] = data[1];
        _combinedSellingItemData[2] = data[2];
        GameEventHandler.RaiseEvent(GameEventType.PublishTeamCombinedSellingItemQuantity, _combinedSellingItemData);
    }

    /// <summary>
    /// Overrides the game time with a target game time.
    /// </summary>
    /// <param name="targetGameTime">The target game time.</param>
    public void OverrideGameTime(float targetGameTime)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("OverrideGameTimeRPC", RpcTarget.AllViaServer, targetGameTime);
    }

    [PunRPC]
    private void OverrideGameTimeRPC(float targetGameTime)
    {
        _overrideGameTimeData[0] = targetGameTime;
        GameEventHandler.RaiseEvent(GameEventType.OverrideGameTime, _overrideGameTimeData);
    }

    public void UpdateMoneyRegardlessOfSale(short moneyAmount, TeamIndex targetGameTime)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("UpdateMoneyRegardlessOfSaleRPC", RpcTarget.AllViaServer, moneyAmount, (byte)targetGameTime);
    }

    [PunRPC]
    private void UpdateMoneyRegardlessOfSaleRPC(short moneyAmount, byte targetGameTime)
    {
        _stockData[0] = moneyAmount;
        _stockData[1] = (TeamIndex)targetGameTime;
        GameEventHandler.RaiseEvent(GameEventType.UpdateMoneyRegardlessOfSale, _stockData);
    }
}