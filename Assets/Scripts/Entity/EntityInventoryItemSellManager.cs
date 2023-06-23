using Photon.Pun;
using UnityEngine;

public abstract class EntityInventoryItemSellManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;
    [SerializeField] protected EntityInventoryManager _entityInventoryManager;

    protected object[] _sellingItemData = new object[5];




    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles the game event and triggers the appropriate actions on game turn update.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    protected abstract void OnGameEvent(GameEventType gameEventType, object[] data);

    /// <summary>
    /// Publishes the confirmed item for sale.
    /// </summary>
    /// <param name="sellingItemQuantity">The quantity of the selling item.</param>
    /// <param name="sellingItemId">The ID of the selling item.</param>
    /// <param name="sellingItemSpoilPercentage">The spoil percentage of the selling item.</param>
    public abstract void PublishConfirmedItemForSale(byte sellingItemQuantity, byte sellingItemId, byte sellingItemSpoilPercentage);

    /// <summary>
    /// Removes the sold items from the inventory.
    /// </summary>
    /// <param name="sellingItemQuantity">The quantity of the selling item.</param>
    /// <param name="sellingItemId">The ID of the selling item.</param>
    public virtual void RemoveSoldItems(byte sellingItemQuantity, byte sellingItemId)
    {
        for (int i = 0; i < sellingItemQuantity; i++)
        {
            _entityInventoryManager.RemoveItem(null, sellingItemId);
        }
    }
}