using Photon.Pun;
using UnityEngine;

public abstract class EntityShopInteractionManager : MonoBehaviourPun
{
    [Header("Entity Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;
    [SerializeField] protected EntityInventoryManager _entityInventoryManager;

    protected virtual bool HavePermission { get;}




    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected abstract void OnGameEvent(GameEventType gameEventType, object[] data);

    protected virtual void AddItemToInventory(Item item)
    {
        _entityInventoryManager.AddItem(item);
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(item);
    }

    protected virtual void UpdateStock(float totalCost)
    {
        GameSceneReferences.Manager.RemoteRPCWrapper.UpdateMoneyRegardlessOfSale((short)-totalCost, _entityIndexManager.TeamIndex);
    }
}