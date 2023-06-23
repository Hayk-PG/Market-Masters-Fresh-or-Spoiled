using UnityEngine;
using Pautik;
using Photon.Pun;
using System.Collections.Generic;

public class ItemsBuyerManager : MonoBehaviourPun
{
    private bool _isItemBought = true;

    public Item BuyingItem { get; private set; }
    public int PreviousPayingAmount { get; private set; }
    public int CurrentPayingAmount { get; private set; }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateBuyingItem(gameEventType);
    }

    private void UpdateBuyingItem(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        bool canAssignBuyingItem = MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) && _isItemBought;

        if (canAssignBuyingItem)
        {
            AssignBuyingItem();
        }
    }

    private void AssignBuyingItem()
    {
        List<Item> entitiesInventoryItems = new List<Item>();
        GlobalFunctions.Loop<EntityInventoryManager>.Foreach(FindObjectsOfType<EntityInventoryManager>(), entityInventorManager => entitiesInventoryItems.AddRange(entityInventorManager.InventoryItems));

        if(entitiesInventoryItems.Count == 0)
        {
            return;
        }

        int randomItemIndexFromCollection = GameSceneReferences.Manager.Items.Collection.IndexOf(entitiesInventoryItems[Random.Range(0, entitiesInventoryItems.Count)]);
        photonView.RPC("AssignBuyingItemRPC", RpcTarget.AllViaServer, randomItemIndexFromCollection);
    }

    [PunRPC]
    private void AssignBuyingItemRPC(int randomItemIndexFromCollection)
    {
        BuyingItem = GameSceneReferences.Manager.Items.Collection[randomItemIndexFromCollection];
        UpdateBuyingItemData(BuyingItem);
    }

    private void UpdateBuyingItemData(Item item)
    {
        int percentage = Random.Range(25, 400);
        float itemPrice = item.Price;
        SetPayingAmount(Mathf.RoundToInt(itemPrice / 100 * percentage));
        GameSceneReferences.Manager.ItemsBuyerUIManager.UpdateBuyingItemData(item.Icon, percentage, CurrentPayingAmount);
    }

    private void SetPayingAmount(int currentPayingAmount)
    {
        PreviousPayingAmount = CurrentPayingAmount <= 0 ? currentPayingAmount : CurrentPayingAmount;
        CurrentPayingAmount = currentPayingAmount;
    }
}