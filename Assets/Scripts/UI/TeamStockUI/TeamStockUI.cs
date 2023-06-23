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
        bool canUpdateMoneyText = gameEventType == GameEventType.UpdateTeamStockAmount && _controllerTeam == (TeamIndex)data[0];

        if (!canUpdateMoneyText)
        {
            return;
        }

        _moneyText.text = _controllerTeam == TeamIndex.Team1 ? ((int)data[1]).ToString() : ((int)data[2]).ToString();
    }
}