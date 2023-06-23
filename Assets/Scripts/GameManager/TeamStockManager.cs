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
        RetrieveDataAndExecute(gameEventType, data);
    }

    private void RetrieveDataAndExecute(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.PublishTeamCombinedSellingItemQuantity)
        {
            return;
        }

        int soldItemsQuantity = (byte)data[0];
        int teamIndex = (byte)data[1];
        int money = GameSceneReferences.Manager.ItemsBuyerManager.PreviousPayingAmount * soldItemsQuantity;
        UpdateTeamStockAmount((TeamIndex)teamIndex, money);
        UpdateStockUI();
    }

    private void UpdateTeamStockAmount(TeamIndex teamIndex, int amount)
    {
        AddressedTeam = teamIndex;
        Conditions<bool>.Compare(AddressedTeam == TeamIndex.Team1, () => Team1StockAmount += amount, () => Team2StockAmount += amount);
    }

    private void UpdateStockUI()
    {
        _teamStockData[0] = AddressedTeam;
        _teamStockData[1] = Team1StockAmount;
        _teamStockData[2] = Team2StockAmount;
        GameEventHandler.RaiseEvent(GameEventType.UpdateTeamStockAmount, _teamStockData);
    }
}