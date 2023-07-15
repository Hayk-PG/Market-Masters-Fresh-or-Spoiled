using UnityEngine;

public class PlayerStorageRentalFeeUpdater : PlayerBaseEventGenerator
{
    private int _defaultFee = 15;
    private int _randomFee;
    private int _duration = 30;
    private int _waitTurnCount;
    private object[] _rentalFeeData = new object[1];
    private object[] _notificationData = new object[1];

    private bool IsDiscountApplied => _waitTurnCount > GameSceneReferences.Manager.GameTurnManager.TurnCount;
    private bool CanApplyDiscount => IsRentalFeeUpdateTriggered() && !IsDiscountApplied;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        ExecuteOnGameTurnUpdate(gameEventType, data);
    }

    private void ExecuteOnGameTurnUpdate(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (CanApplyDiscount)
        {
            SetRandomFee();
            DetermineWaitTurnCount();
            UpdateRentalFee(_randomFee);
            DisplayNotification();
            print($"Applying discount: {_randomFee}");
            return;
        }

        if (!IsDiscountApplied)
        {
            UpdateRentalFee(_defaultFee);
            print($"Setting default fee amount: {_defaultFee}");
        }
    }

    private bool IsRentalFeeUpdateTriggered()
    {
        int minPossibilityRange = _playerReputationManager.ReputationState == ReputationState.Terrible ? 0 : _playerReputationManager.ReputationState == ReputationState.Poor ? 10 :
                             _playerReputationManager.ReputationState == ReputationState.Neutral ? 25 : _playerReputationManager.ReputationState == ReputationState.Good ? 40 : 45;
        int maxPossibilityRange = 51;
        int randomPossibilityNumber = Random.Range(minPossibilityRange, maxPossibilityRange);
        int targetNumber = Random.Range(minPossibilityRange, maxPossibilityRange);
        return randomPossibilityNumber == targetNumber;
    }

    private void SetRandomFee()
    {
        int minFeeRange = 0;
        int maxFeeRange = _playerReputationManager.ReputationState == ReputationState.Terrible ? 13 : _playerReputationManager.ReputationState == ReputationState.Poor ? 10 :
                           _playerReputationManager.ReputationState == ReputationState.Neutral ? 7 : _playerReputationManager.ReputationState == ReputationState.Good ? 4 : 2;
        _randomFee = Random.Range(minFeeRange, maxFeeRange);
    }

    private void DetermineWaitTurnCount()
    {
        _waitTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount + _duration;
    }

    private void UpdateRentalFee(int fee)
    {       
        _rentalFeeData[0] = fee;
        GameEventHandler.RaiseEvent(GameEventType.UpdateStorageRentalFee, _rentalFeeData);
    }

    private void DisplayNotification()
    {
        _notificationData[0] = new Notification(NotificationType.DisplayReadNotification, StorageRentalNotificationMessage.StorageRentDiscountTitle(_duration), StorageRentalNotificationMessage.StorageRentDiscountMessage(_duration));
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}