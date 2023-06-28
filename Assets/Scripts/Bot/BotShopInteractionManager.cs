using UnityEngine;
using Pautik;
using System.Collections;

public class BotShopInteractionManager : EntityShopInteractionManager
{
    protected override bool HavePermission => MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);



    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!HavePermission)
        {
            return;
        }

        StartCoroutine(GenerateRandomShopPurchaseAfterDelay(gameEventType, data));
    }

    private IEnumerator GenerateRandomShopPurchaseAfterDelay(GameEventType gameEventType, object[] data)
    {
        bool canHaveAccessToShop = gameEventType == GameEventType.UpdateGameTurn && _entityIndexManager.TeamIndex != (TeamIndex)data[2];

        if (!canHaveAccessToShop)
        {
            yield break;
        }

        float randomDelay = Random.Range(1f, 3f);
        yield return new WaitForSeconds(randomDelay);
        GenerateRandomShopPurchase();
    }

    private void GenerateRandomShopPurchase()
    {
        float teamStock = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team1StockAmount : GameSceneReferences.Manager.TeamStockManager.Team2StockAmount;
        float purchaseLimit = teamStock / 100f * 30f;
        float totalMoneySpend = 0f;

        for (int i = 0; i < GameSceneReferences.Manager.Items.Collection.Count; i++)
        {
            int randomItemIndex = GameSceneReferences.Manager.Items.Collection.IndexOf(GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)]);
            int randomPercentage = (Random.Range(40, 400));
            float itemOriginalPrice = GameSceneReferences.Manager.Items.Collection[randomItemIndex].Price;
            float newPrice = itemOriginalPrice / 100 * randomPercentage;
            bool canBuyNewItem = (teamStock - newPrice + totalMoneySpend) > purchaseLimit;
            bool haveEnoughInventorySpace = _entityInventoryManager.InventoryItems.Count < _entityInventoryManager.InventorySize;

            if (!canBuyNewItem || !haveEnoughInventorySpace)
            {
                break;
            }

            totalMoneySpend += newPrice;
            AddItemToInventory(GameSceneReferences.Manager.Items.Collection[randomItemIndex]);
        }

        if(totalMoneySpend > 0)
        {
            UpdateStock(totalMoneySpend);
        }
    }

    protected override void AddItemToInventory(Item item)
    {
        _entityInventoryManager.AddItem(item);
    }
}