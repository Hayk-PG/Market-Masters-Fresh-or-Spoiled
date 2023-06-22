using Photon.Pun;
using UnityEngine;

public class PlayerInventoryItemSellManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private object[] _sellingItemData = new object[4];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
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

    private void PublishConfirmedItemForSale(int sellingItemQuantity, int sellingItemId)
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
        GameEventHandler.RaiseEvent(GameEventType.PublishConfirmedItemForSale, _sellingItemData);
    }

    private void RemoveSoldItems(int sellingItemQuantity, int sellingItemId)
    {
        for (int i = 0; i < sellingItemQuantity; i++)
        {
            _entityInventoryManager.RemoveItem(null, sellingItemId);
        }
    }
}