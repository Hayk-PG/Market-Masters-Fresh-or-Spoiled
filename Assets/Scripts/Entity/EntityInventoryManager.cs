using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventoryManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;

    [Header("Items List")]
    [SerializeField] protected List<Item> _inventoryItems;

    protected int _inventorySize = 8;

    public List<Item> InventoryItems => _inventoryItems;
    public int InventorySize => _inventorySize;
    public bool HaveEnoughInventorySpace => _inventoryItems.Count < _inventorySize;




    protected virtual void Start()
    {
        InitializeInventory();
    }

    protected virtual void InitializeInventory()
    {
        if (!HavePermission())
        {
            return;
        }

        for (int i = 0; i < _inventorySize; i++)
        {
            AddRandomItemFromCollection(out Item item);
        }
    }

    protected virtual bool HavePermission()
    {
        return true;
    }

    protected virtual void AddRandomItemFromCollection(out Item item)
    {
        int randomItemIndexFromCollection = Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count);
        item = GameSceneReferences.Manager.Items.Collection[randomItemIndexFromCollection];
        AddItem(item);
    }

    public virtual void AddItem(Item item = null, int itemId = 0)
    {
        if(item != null)
        {
            _inventoryItems.Add(item);
        }
        else
        {
            _inventoryItems.Add(GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == itemId));
        }
    }

    public virtual void RemoveItem(Item item = null, int itemId = 0)
    {
        if (item != null)
        {
            _inventoryItems.Remove(item);
        }
        else
        {
            _inventoryItems.Remove(GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == itemId));
        }
    }
}