using UnityEngine;
using TMPro;
using Pautik;

public class TeamStockUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _effecText;

    [Header("Controller Team")]
    [SerializeField] private TeamIndex _controllerTeam;

    private const string _increment = "Increment";
    private const string _decrement = "Decrement";

    private int _previousAmount;
    private int _currentAmount;




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
        UpdateText(_moneyText, moneyAmount);
        PlayAnimation(moneyAmount);
        UISoundController.PlaySound(6, 0);
    }

    private void PlayAnimation(int moneyAmount)
    {
        _previousAmount = _currentAmount;
        _currentAmount = moneyAmount;
        int amountDifference = Mathf.Abs(_currentAmount - _previousAmount);

        if (_previousAmount == _currentAmount)
        {
            return;
        }

        _animator.Play(_previousAmount < _currentAmount ? _increment : _decrement, 0, 0);
        Conditions<bool>.Compare(_previousAmount < _currentAmount, () => UpdateText(_effecText, amountDifference, "+"), () => UpdateText(_effecText, amountDifference, "-"));
    }

    private void UpdateText(TMP_Text text, int amount, string symbol = "")
    {
        text.text = $"{symbol}${Converter.ThousandsSeparatorString(amount, true)}";
    }
}