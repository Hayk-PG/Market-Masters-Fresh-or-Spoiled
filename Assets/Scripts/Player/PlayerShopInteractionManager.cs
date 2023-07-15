using Pautik;
using Photon.Pun;

public class PlayerShopInteractionManager : EntityShopInteractionManager
{
    private object[] _stockData = new object[2];

    private int _itemBoughTurn = 0;

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
            if (!_entityInventoryManager.HaveEnoughInventorySpace)
            {
                itemButton.Deselect(true);
                continue;
            }

            AddItemToInventory(itemButton.AssociatedItem);
            RemoveItemFromShopItemButton(itemButton);
            totalCost += itemButton.Price;          
        }

        Conditions<bool>.Compare(totalCost > 0, () => PlaySoundEffect(5, 1), () => PlaySoundEffect(4, 1));
        UpdateStock(totalCost);
        UpdateReputation(totalCost);
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

    private void RemoveItemFromShopItemButton(ShopItemButton shopItemButton)
    {
        shopItemButton.RemoveItem();
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

    private void UpdateReputation(float totalCost)
    {
        if(totalCost <= 0)
        {
            return;
        }

        bool hasItemBeenBoughtThisTurn = _itemBoughTurn == GameSceneReferences.Manager.GameTurnManager.TurnCount;

        if (hasItemBeenBoughtThisTurn)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationOnBuy);
        _itemBoughTurn = GameSceneReferences.Manager.GameTurnManager.TurnCount;
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}