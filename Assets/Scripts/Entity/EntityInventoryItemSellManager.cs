using Photon.Pun;
using UnityEngine;

public abstract class EntityInventoryItemSellManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;
    [SerializeField] protected EntityInventoryManager _entityInventoryManager;

    protected object[] _sellingItemData = new object[4];




    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected abstract void OnGameEvent(GameEventType gameEventType, object[] data);

    public abstract void PublishConfirmedItemForSale(int sellingItemQuantity, int sellingItemId);

    public virtual void RemoveSoldItems(int sellingItemQuantity, int sellingItemId)
    {
        for (int i = 0; i < sellingItemQuantity; i++)
        {
            _entityInventoryManager.RemoveItem(null, sellingItemId);
        }
    }
}