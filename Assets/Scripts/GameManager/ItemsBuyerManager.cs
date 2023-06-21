using UnityEngine;
using Pautik;
using Photon.Pun;

public class ItemsBuyerManager : MonoBehaviourPun
{
    private bool _isItemBought = true;
    private object[] _buyingItemData = new object[1];

    public Item BuyingItem { get; private set; }




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
        int randomItemIndexFromCollection = Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count);
        photonView.RPC("AssignBuyingItemRPC", RpcTarget.AllViaServer, randomItemIndexFromCollection);
    }

    [PunRPC]
    private void AssignBuyingItemRPC(int randomItemIndexFromCollection)
    {
        BuyingItem = GameSceneReferences.Manager.Items.Collection[randomItemIndexFromCollection];
        WrapBuyingItemData(BuyingItem);
        SendBuyingItemData();
    }

    private void WrapBuyingItemData(Item item)
    {
        _buyingItemData[0] = item;
    }

    private void SendBuyingItemData()
    {
        GameEventHandler.RaiseEvent(GameEventType.UpdateBuyingItem, _buyingItemData);
    }
}