using UnityEngine;
using TMPro;
using Pautik;

public class TeamStockUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _moneyText;

    [Header("Controller Team")]
    [SerializeField] private TeamIndex _controllerTeam;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    public void SetControllerTeam(TeamIndex teamIndex)
    {
        _controllerTeam = teamIndex;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateMoneyText(gameEventType, data);
    }

    private void UpdateMoneyText(GameEventType gameEventType, object[] data)
    {
        bool canUpdateMoneyText = gameEventType == GameEventType.UpdateStockUI && _controllerTeam == (TeamIndex)data[0];

        if (!canUpdateMoneyText)
        {
            return;
        }

        int moneyAmount = _controllerTeam == TeamIndex.Team1 ? ((int)data[1]) : ((int)data[2]);
        _moneyText.text = $"${Converter.ThousandsSeparatorString(moneyAmount, true)}";
        UISoundController.PlaySound(6, 0);
    }
}