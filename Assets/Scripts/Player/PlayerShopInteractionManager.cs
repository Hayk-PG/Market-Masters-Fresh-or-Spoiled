using Photon.Pun;
using UnityEngine;

public class PlayerShopInteractionManager : MonoBehaviourPun
{
    [Header("Entity Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;




    private void OnEnable()
    {      
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        CheckPurchaseRequirements(gameEventType, data);
        BuyItem(gameEventType, data);       
    }

    private void CheckPurchaseRequirements(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.MeetsPurchaseRequirements)
        {
            return;
        }

        RetrievePurchaseRequirementsData(data, out float selectedItemsTotalCost);
        SetBuyButtonInteractability((ItemShopManager)data[1], selectedItemsTotalCost);
    }

    private void BuyItem(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.TryBuySelectedShopItem)
        {
            return;
        }

        float totalCost = 0f;

        foreach (var itemButton in (ShopItemButton[])data)
        {
            bool haveEnoughInventorySpace = _entityInventoryManager.InventoryItems.Count < _entityInventoryManager.InventorySize;

            if (!haveEnoughInventorySpace)
            {
                itemButton.Deselect();
                continue;
            }

            AddItemToInventory(itemButton.AssosiatedItem);
            totalCost += itemButton.Price;          
        }

        UpdateStock(totalCost);
    }

    private void AddItemToInventory(Item item)
    {
        _entityInventoryManager.AddItem(item);
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(item);
    }

    private void UpdateStock(float totalCost)
    {
        GameSceneReferences.Manager.RemoteRPCWrapper.UpdateMoneyRegardlessOfSale((short)-totalCost, _entityIndexManager.TeamIndex);
    }

    private void RetrievePurchaseRequirementsData(object[] data, out float selectedItemsTotalCost)
    {
        selectedItemsTotalCost = (float)data[0];
    }

    private void SetBuyButtonInteractability(ItemShopManager itemShopManager, float selectedItemsTotalCost)
    {
        int teamStockAmount = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team1StockAmount : GameSceneReferences.Manager.TeamStockManager.Team2StockAmount;
        itemShopManager.SetBuyButtonInteractability(selectedItemsTotalCost <= teamStockAmount);
    }
}