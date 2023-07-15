using UnityEngine;
using Pautik;
using Photon.Pun;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class ItemsBuyerManager : MonoBehaviourPun
{
    private object[] _notificationData = new object[1];
    private bool _isItemBought = true;

    public byte[] DemandDrivenItemsIds { get; private set; }
    public Item BuyingItem { get; private set; }
    public int PreviousPayingAmount { get; private set; }
    public int CurrentPayingAmount { get; private set; }




    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonNetworkEvent;
        GameEventHandler.OnEvent += OnGameEvent;        
    }

    private void OnPhotonNetworkEvent(EventData eventData)
    {
        RetrieveDemandDrivenItemsData(eventData, eventData.CustomData);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateBuyingItem(gameEventType);
    }

    /// <summary>
    /// Retrieves the demand-driven items data from the Photon Network event.
    /// </summary>
    /// <param name="eventData">The Photon Network event data.</param>
    /// <param name="data">The custom data of the event.</param>
    private void RetrieveDemandDrivenItemsData(EventData eventData, object data)
    {
        if (eventData.Code != EventInfo.Code_DemandDrivenItemsId)
        {
            return;
        }

        DemandDrivenItemsIds = (byte[])data;
        DisplayDemandDrivenItemsNotification();
        print($"Demand drivent items count: {DemandDrivenItemsIds.Length}");
    }

    /// <summary>
    /// Updates the buying item based on the game event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    private void UpdateBuyingItem(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        bool canAssignBuyingItem = MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) && _isItemBought;

        if (!canAssignBuyingItem)
        {
            return;
        }

        AssignBuyingItem();
    }

    /// <summary>
    /// Displays a notification for the demand-driven items.
    /// </summary>
    private void DisplayDemandDrivenItemsNotification()
    {
        List<Sprite> itemsIcons = new List<Sprite>();
        GlobalFunctions.Loop<byte>.Foreach(DemandDrivenItemsIds, id => itemsIcons.Add(GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == id).Icon));
        _notificationData[0] = new Notification(NotificationType.DisplayReadNotificationWithImages, HighDemandItemsNotificationMessage.HighDemandItemsAlertTitle(10), HighDemandItemsNotificationMessage.HighDemandItemsAlertMessage(10), itemsIcons.ToArray());
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }

    /// <summary>
    /// Assigns the buying item based on the demand-driven items or the item collection.
    /// </summary>
    private void AssignBuyingItem()
    {
        byte itemId = 0;
        short randomCostPercentage = (short)Random.Range(25, 400);
        bool haveItemsInDemand = DemandDrivenItemsIds != null && DemandDrivenItemsIds.Length > 0;

        if (haveItemsInDemand)
        {
            GetRandomItemIdFromDemandDrivenItems(out itemId);
        }
        else
        {
            GetRandomItemIdFromItemCollection(out itemId);
        }

        photonView.RPC("AssignBuyingItemRPC", RpcTarget.AllViaServer, itemId, randomCostPercentage);
    }

    /// <summary>
    /// Gets a random item ID from the demand-driven items array.
    /// </summary>
    /// <param name="itemId">The randomly selected item ID.</param>
    private void GetRandomItemIdFromDemandDrivenItems(out byte itemId)
    {
        itemId = DemandDrivenItemsIds[Random.Range(0, DemandDrivenItemsIds.Length)];
    }

    /// <summary>
    /// Gets a random item ID from the item collection.
    /// </summary>
    /// <param name="itemId">The randomly selected item ID.</param>
    private void GetRandomItemIdFromItemCollection(out byte itemId)
    {
        itemId = (byte)GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)].ID;
    }

    [PunRPC]
    private void AssignBuyingItemRPC(byte itemId, short randomCostPercentage)
    {
        BuyingItem = GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == itemId);
        SetPayingAmount(BuyingItem, randomCostPercentage);
        UpdateUI(BuyingItem, randomCostPercentage);
    }

    /// <summary>
    /// Sets the paying amount for the buying item based on the random cost percentage.
    /// </summary>
    /// <param name="item">The buying item.</param>
    /// <param name="randomCostPercentage">The randomly generated cost percentage.</param>
    private void SetPayingAmount(Item item, short randomCostPercentage)
    {
        float itemPrice = item.Price;
        int currentPayingAmount = Mathf.RoundToInt(itemPrice / 100f * randomCostPercentage);
        PreviousPayingAmount = CurrentPayingAmount <= 0 ? currentPayingAmount : CurrentPayingAmount;
        CurrentPayingAmount = currentPayingAmount;
    }

    /// <summary>
    /// Updates the UI with the buying item information.
    /// </summary>
    /// <param name="item">The buying item.</param>
    /// <param name="randomCostPercentage">The randomly generated cost percentage.</param>
    private void UpdateUI(Item item, short randomCostPercentage)
    {
        GameSceneReferences.Manager.ItemsBuyerUIManager.UpdateUI(item.Icon, randomCostPercentage, CurrentPayingAmount);
    }   
}