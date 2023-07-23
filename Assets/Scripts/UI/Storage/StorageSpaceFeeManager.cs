using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageSpaceFeeManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StorageManagerButton _storageManagerButton;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _storageRentalFeeText;
    [SerializeField] private Image _discountRemainingTimeBackground;
    [SerializeField] private Image _discountRemainingTimeFill;

    private int _storageRentalFee;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateStorageRentalFeeAmount(gameEventType, data);
        CalculateStorageSpaceFee(gameEventType, data);
        UpdateDiscountRemainingTime(gameEventType, data);
    }

    private void UpdateStorageRentalFeeAmount(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateStorageRentalFee)
        {
            return;
        }

        _storageRentalFee = (int)data[0];
        UpdateStorageRentalFeeText();
    }

    private void CalculateStorageSpaceFee(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.CalculateStorageSpaceFee)
        {
            return;
        }

        bool canCalculateStorageSpaceFee = GameSceneReferences.Manager.GameManager.IsGameStarted && _storageManagerButton.StorageItemsList.Count > 0;

        if (!canCalculateStorageSpaceFee)
        {
            return;
        }

        int storageSpaceFeeAmount = 0;

        foreach (var storageItem in _storageManagerButton.StorageItemsList)
        {
            bool hasTurnChangedSinceFeeProcessStart = GameSceneReferences.Manager.GameTurnManager.TurnCount > storageItem.StorageFeeProcessTurnCount;

            if (!hasTurnChangedSinceFeeProcessStart)
            {
                continue;
            }

            int turnCountSinceFeeProcessStart = GameSceneReferences.Manager.GameTurnManager.TurnCount - storageItem.StorageFeeProcessTurnCount;
            storageSpaceFeeAmount += (turnCountSinceFeeProcessStart * _storageRentalFee);          
            storageItem.UpdateStorageFeeProcessTurnCount(GameSceneReferences.Manager.GameTurnManager.TurnCount);
        }

        CalculatePlayerStorageTotalFee((System.Action<int>)data[0], storageSpaceFeeAmount);
    }

    private void CalculatePlayerStorageTotalFee(System.Action<int> CalculateTotalFee, int storageSpaceFeeAmount)
    {
        if (storageSpaceFeeAmount < 1)
        {
            return;
        }

        CalculateTotalFee?.Invoke(storageSpaceFeeAmount);
    }

    private void UpdateStorageRentalFeeText()
    {
        _storageRentalFeeText.text = $"${_storageRentalFee}";
    }

    private void UpdateDiscountRemainingTime(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.PublishStorageDiscountRemainingTime)
        {
            return;
        }

        int remainingTime = (int)data[0];
        int timeFrame = (int)data[1];

        if(remainingTime < 1)
        {
            _discountRemainingTimeFill.fillAmount = 0;
            SetDiscountRemainingTimeIndicatorActive(false);
            return;
        }

        float fillAmount = Mathf.InverseLerp(0, timeFrame * 0.01f, remainingTime * 0.01f);
        _discountRemainingTimeFill.fillAmount = fillAmount;
        SetDiscountRemainingTimeIndicatorActive(true);
    }

    private void SetDiscountRemainingTimeIndicatorActive(bool isActive)
    {
        if(_discountRemainingTimeBackground.gameObject.activeInHierarchy == isActive)
        {
            return;
        }

        _discountRemainingTimeBackground.gameObject.SetActive(isActive);
    }
}