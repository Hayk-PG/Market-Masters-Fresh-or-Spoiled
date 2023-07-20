using UnityEngine;
using TMPro;

public class StorageSpaceFeeManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StorageManagerButton _storageManagerButton;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _storageRentalFeeText;

    private int _storageRentalFee;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateStorageRentalFeeAmount(gameEventType, data);
        CalculateStorageSpaceFee(gameEventType, data);      
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
}