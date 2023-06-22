using Photon.Pun;
using Pautik;

public class RemoteRPCWrapper : MonoBehaviourPun
{
    private object[] _botNumericChoiceData = new object[3];
    private object[] _overrideGameTimeData = new object[1];

    private bool IsControllerMasterClient => MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);




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

    public void PublishBotConfirmedItemForSale(string entityName, int entityActorNumber, byte sellingItemQuantity, byte sellingItemId)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("PublishBotConfirmedItemForSaleRPC", RpcTarget.AllViaServer, entityName, entityActorNumber, sellingItemQuantity, sellingItemId);
    }

    [PunRPC]
    private void PublishBotConfirmedItemForSaleRPC(string entityName, int entityActorNumber, byte sellingItemQuantity, byte sellingItemId)
    {
        BotInventoryItemSellManager botInventoryItemSellManager = GlobalFunctions.ObjectsOfType<BotInventoryItemSellManager>.Find(bot => bot.BotName == entityName && bot.BotActorNumber == entityActorNumber);
        botInventoryItemSellManager?.PublishConfirmedItemForSale(sellingItemQuantity, sellingItemId);
        botInventoryItemSellManager?.RemoveSoldItems(sellingItemQuantity, sellingItemId);
    }

    public void PublishBotSelectedNumber(string entityName, int entityActorNumber, int confirmedNumber)
    {
        if (!IsControllerMasterClient)
        {
            return;
        }

        photonView.RPC("PublishBotSelectedNumberRPC", RpcTarget.AllViaServer, entityName, entityActorNumber, confirmedNumber);
    }

    [PunRPC]
    private void PublishBotSelectedNumberRPC(string entityName, int entityActorNumber, int confirmedNumber)
    {
        _botNumericChoiceData[0] = entityName;
        _botNumericChoiceData[1] = entityActorNumber;
        _botNumericChoiceData[2] = confirmedNumber;

        GameEventHandler.RaiseEvent(GameEventType.PublishSellingItemQuantity, _botNumericChoiceData);
    }

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
}