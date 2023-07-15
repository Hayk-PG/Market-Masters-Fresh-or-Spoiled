using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Pautik;

public class GameStatusUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _timeRemainingText;
    [SerializeField] private TMP_Text _targetMoneyText;
    [SerializeField] private Slider _timeRemainingSlider;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateTimeRemainingTextAndSlider(gameEventType, data);
        UpdateTargetMoneyText(gameEventType, data);
    }

    private void UpdateTimeRemainingTextAndSlider(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        float remainingTime = (float)data[2];
        float sliderValue = (900f - remainingTime);
        _timeRemainingText.text = $"Time Remains: {Converter.MmSs(remainingTime)}";
        _timeRemainingSlider.minValue = 0f;
        _timeRemainingSlider.maxValue = 900f;
        _timeRemainingSlider.value = sliderValue;
    }

    private void UpdateTargetMoneyText(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateStockUI)
        {
            return;
        }

        int team1StockAmount = (int)data[1];
        int team2StockAmount = (int)data[2];
        int highestStockAmount = team1StockAmount < team2StockAmount ? team2StockAmount : team1StockAmount;
        _targetMoneyText.text = $"{highestStockAmount}/1000";
    }
}