using Pautik;
using Photon.Pun;

public class PlayerShopInteractionManager : EntityShopInteractionManager
{
    private object[] _stockData = new object[2];

    protected override bool HavePermission => _entityManager.PlayerPhotonview.IsMine;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!HavePermission)
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

        Conditions<bool>.Compare(totalCost > 0, () => PlaySoundEffect(5, 1), () => PlaySoundEffect(4, 1));
        UpdateStock(totalCost);
    }

    protected override void UpdateStock(float totalCost)
    {
        photonView.RPC("BuyItemRPC", RpcTarget.AllViaServer, (short)-totalCost, (byte)_entityIndexManager.TeamIndex);
    }

    protected override void AddItemToInventory(Item item)
    {
        _entityInventoryManager.AddItem(item);
        GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(item);
    }

    [PunRPC]
    private void BuyItemRPC(short moneyAmount, byte targetTeam)
    {
        _stockData[0] = moneyAmount;
        _stockData[1] = (TeamIndex)targetTeam;
        GameEventHandler.RaiseEvent(GameEventType.UpdateMoneyRegardlessOfSale, _stockData);
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

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}