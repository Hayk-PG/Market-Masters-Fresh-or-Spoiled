using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class TeamStockManager : MonoBehaviour
{
    public TeamIndex AddressedTeam { get; private set; }
    public int Team1StockAmount { get; private set; }
    public int Team2StockAmount { get; private set; }

    private object[] _teamStockData = new object[3];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        ReceiveMoneyFromSell(gameEventType, data);
        UpdateMoneyRegardlessOfSale(gameEventType, data);
    }

    /// <summary>
    /// Retrieves data from a game event and executes the necessary actions.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void ReceiveMoneyFromSell(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.PublishTeamCombinedSellingItemQuantity)
        {
            return;
        }

        // Extract the quantity of sold items from the data array
        int soldItemsQuantity = (byte)data[0];
        // Extract the spoil percentage from the data array
        int spoilPercentage = (byte)data[1];
        // Extract the team index from the data array
        int teamIndex = (byte)data[2];
        // Calculate the total revenue based on the quantity of sold items
        // and convert it to float for further calculations
        float totalRevenue  = soldItemsQuantity * 100f;
        // Assign the spoilage percentage
        float spoilagePercentage  = spoilPercentage;
        // Calculate the spoilage ratio, which represents the ratio of the spoilage
        // amount to the total revenue
        float spoilageRatio  = (spoilagePercentage / totalRevenue ) * 100f;
        // Calculate the initial payment, which is the previous paying amount multiplied
        // by the quantity of sold items
        float initialPayment  = GameSceneReferences.Manager.ItemsBuyerManager.PreviousPayingAmount * soldItemsQuantity;
        // Calculate the final payment by subtracting the spoilage amount from the initial payment
        float finalPayment  = initialPayment  - (initialPayment  / 100 * spoilageRatio );

        UpdateTeamStockAmount((TeamIndex)teamIndex, (int)finalPayment);
        UpdateStockUI();
    }

    /// <summary>
    /// Updates the player's money regardless of a sale transaction.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the game event.</param>
    private void UpdateMoneyRegardlessOfSale(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateMoneyRegardlessOfSale)
        {
            return;
        }

        short amount = (short)data[0];
        TeamIndex targetTeam = (TeamIndex)data[1];
        UpdateTeamStockAmount(targetTeam, amount);
        UpdateStockUI();
    }

    /// <summary>
    /// Updates the stock amount for a specific team.
    /// </summary>
    /// <param name="teamIndex">The index of the team.</param>
    /// <param name="amount">The amount to be added to the team's stock.</param>
    private void UpdateTeamStockAmount(TeamIndex teamIndex, int amount)
    {
        AddressedTeam = teamIndex;
        Conditions<bool>.Compare(AddressedTeam == TeamIndex.Team1, () => Team1StockAmount += amount, () => Team2StockAmount += amount);
    }

    /// <summary>
    /// Updates the stock UI and raises an event with the updated team stock data.
    /// </summary>
    private void UpdateStockUI()
    {
        _teamStockData[0] = AddressedTeam;
        _teamStockData[1] = Team1StockAmount;
        _teamStockData[2] = Team2StockAmount;
        GameEventHandler.RaiseEvent(GameEventType.UpdateStockUI, _teamStockData);
    }
}