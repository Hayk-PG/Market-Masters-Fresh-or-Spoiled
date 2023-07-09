using UnityEngine;

public class PlayerStorageRentalFeeUpdater : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private PlayerReputationManager _playerReputationManager;

    private int _defaultFee = 15;
    private int _randomFee;
    private int _duration = 30;
    private int _waitTurnCount;
    private object[] _rentalFeeData = new object[1];
    private object[] _notificationData = new object[4];

    private bool CanReceiveGameEvent => _entityManager.PlayerPhotonview.IsMine;
    private bool IsDiscountApplied => _waitTurnCount > GameSceneReferences.Manager.GameTurnManager.TurnCount;
    private bool CanApplyDiscount => IsRentalFeeUpdateTriggered() && !IsDiscountApplied;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!CanReceiveGameEvent)
        {
            return;
        }

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
        _notificationData[0] = NotificationType.DisplayReadNotification;
        _notificationData[1] = StorageRentalNotificationMessage.StorageRentDiscountTitle(_duration);
        _notificationData[2] = StorageRentalNotificationMessage.StorageRentDiscountMessage(_duration);
        _notificationData[3] = null;
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}