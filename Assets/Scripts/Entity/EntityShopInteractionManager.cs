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

    protected abstract void AddItemToInventory(Item item);

    protected abstract void UpdateStock(float totalCost);
}