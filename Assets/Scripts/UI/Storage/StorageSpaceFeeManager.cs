using UnityEngine;

public class StorageSpaceFeeManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StorageItemButton _storageItemButton;

    private int _feeProcessTurn;




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
        if(gameEventType != GameEventType.CalculateStorageSpaceFee)
        {
            return;
        }

        bool canCalculateStorageSpaceFee = GameSceneReferences.Manager.GameManager.IsGameStarted && _storageItemButton.HasStorageItem;

        print($"Is game started: {GameSceneReferences.Manager.GameManager.IsGameStarted}/Has storage item: {_storageItemButton.HasStorageItem}");

        if (!canCalculateStorageSpaceFee)
        {
            return;
        }

        bool hasTurnChangedSinceFeeProcessStart = _storageItemButton.AssociatedStorageItem.Value.InitialTurnCount > _feeProcessTurn;

        if (hasTurnChangedSinceFeeProcessStart)
        {
            _feeProcessTurn = _storageItemButton.AssociatedStorageItem.Value.InitialTurnCount;
        }

        int turnCountSinceFeeProcessStart = GameSceneReferences.Manager.GameTurnManager.TurnCount - _feeProcessTurn;
        int storageSpaceFeeAmount = turnCountSinceFeeProcessStart * 15;

        print($"Initial turn count: {_storageItemButton.AssociatedStorageItem.Value.InitialTurnCount}/Fee process turn: {_feeProcessTurn}/Turn count since fee process start: {turnCountSinceFeeProcessStart}/Storage space fee amount: {storageSpaceFeeAmount}/Has item: {_storageItemButton.AssociatedStorageItem.Value}");

        CalculatePlayerStorageTotalFee((System.Action<int>)data[0], storageSpaceFeeAmount);
        _feeProcessTurn = GameSceneReferences.Manager.GameTurnManager.TurnCount;
    }

    private void CalculatePlayerStorageTotalFee(System.Action<int> CalculateTotalFee, int storageSpaceFeeAmount)
    {
        if(storageSpaceFeeAmount < 1)
        {
            return;
        }

        CalculateTotalFee?.Invoke(storageSpaceFeeAmount);
    }
}