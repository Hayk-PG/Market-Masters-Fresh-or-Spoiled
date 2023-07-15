using Photon.Pun;
using Photon.Realtime;
using Pautik;

public class PlayerInventoryPublisher : EntityInventoryPublisher
{
    protected override bool IsAllowed()
    {
        return _entityManager.PlayerPhotonview.IsMine;
    }

    protected override void PublishInventoryData(byte[] inventoryItemsIdArray)
    {
        bool isMasterClient = MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);

        if (isMasterClient)
        {
            base.PublishInventoryData(inventoryItemsIdArray);
            return;
        }

        EventInfo.Content_InventoryPublisher = inventoryItemsIdArray;
        PhotonNetwork.RaiseEvent(EventInfo.Code_InventoryPublisher, EventInfo.Content_InventoryPublisher, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendUnreliable);
    }
}