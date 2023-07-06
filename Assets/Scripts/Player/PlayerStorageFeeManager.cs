using UnityEngine;

public class PlayerStorageFeeManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;

    private int _totalFee;
    private object[] _data = new object[1];

    private System.Action<int> CalculateTotalFeeDelegate => CalculateTotalFee;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        bool isPlayerController = _entityManager.PlayerPhotonview.IsMine;

        if (!isPlayerController)
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

        bool isMyTeamTurn = IsCurrentTurnTargetTurn((TeamIndex)data[2], _entityIndexManager.TeamIndex);

        RaiseFeeCalculationEvent();
        ResetTotalFee();
    }

    private void RaiseFeeCalculationEvent()
    {
        _data[0] = CalculateTotalFeeDelegate;
        GameEventHandler.RaiseEvent(GameEventType.CalculateStorageSpaceFee, _data);
    }

    private bool IsCurrentTurnTargetTurn(TeamIndex currentTurn, TeamIndex targetTurn)
    {
        return currentTurn == targetTurn;
    }

    private void CalculateTotalFee(int storageSpaceFeeAmount)
    {
        _totalFee = storageSpaceFeeAmount;
        print(_totalFee);
    }

    private void ResetTotalFee()
    {
        _totalFee = 0;
    }
}