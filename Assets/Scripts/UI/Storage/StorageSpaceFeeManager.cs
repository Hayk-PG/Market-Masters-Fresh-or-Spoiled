using UnityEngine;

public class StorageSpaceFeeManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StorageManagerButton _storageManagerButton;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        CalculateStorageSpaceFee(gameEventType, data);
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
            storageSpaceFeeAmount += (turnCountSinceFeeProcessStart * 15);          
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
}