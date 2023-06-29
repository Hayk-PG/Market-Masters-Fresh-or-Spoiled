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
        int randomCostPercentage = Random.Range(25, 400);
        photonView.RPC("AssignBuyingItemRPC", RpcTarget.AllViaServer, (byte)randomItemIndexFromCollection, (byte)randomCostPercentage);
    }

    [PunRPC]
    private void AssignBuyingItemRPC(byte randomItemIndexFromCollection, byte randomCostPercentage)
    {
        BuyingItem = GameSceneReferences.Manager.Items.Collection[randomItemIndexFromCollection];
        SetPayingAmount(BuyingItem, randomCostPercentage);
        UpdateUI(BuyingItem, randomCostPercentage);
    }

    private void UpdateUI(Item item, int randomCostPercentage)
    {
        GameSceneReferences.Manager.ItemsBuyerUIManager.UpdateUI(item.Icon, randomCostPercentage, CurrentPayingAmount);
    }

    private void SetPayingAmount(Item item, int randomCostPercentage)
    {
        float itemPrice = item.Price;
        int currentPayingAmount = Mathf.RoundToInt(itemPrice / 100 * randomCostPercentage);
        PreviousPayingAmount = CurrentPayingAmount <= 0 ? currentPayingAmount : CurrentPayingAmount;
        CurrentPayingAmount = currentPayingAmount;
    }
}