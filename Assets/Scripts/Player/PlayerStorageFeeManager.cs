using Photon.Pun;
using UnityEngine;

public class PlayerStorageFeeManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;

    private int _totalFee;
    private object[] _data = new object[1];
    private object[] _storageSpaceUsageData = new object[2];
         
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
        SubtractMoneyForStorageSpaceUsage(isMyTeamTurn);
        RaiseFeeCalculationEvent();        
    }

    private bool IsCurrentTurnTargetTurn(TeamIndex currentTurn, TeamIndex targetTurn)
    {
        return currentTurn == targetTurn;
    }

    private void SubtractMoneyForStorageSpaceUsage(bool isMyTeamTurn)
    {
        bool canProcess = !isMyTeamTurn && _totalFee > 0;

        if (!canProcess)
        {
            return;
        }

        photonView.RPC("SubtractMoneyForStorageSpaceUsageRPC", RpcTarget.AllViaServer, (short)-_totalFee, (byte)_entityIndexManager.TeamIndex);
        ResetTotalFee();
    }

    [PunRPC]
    private void SubtractMoneyForStorageSpaceUsageRPC(short moneyAmount, byte teamIndex)
    {
        _storageSpaceUsageData[0] = moneyAmount;
        _storageSpaceUsageData[1] = (TeamIndex)teamIndex;
        GameEventHandler.RaiseEvent(GameEventType.UpdateMoneyRegardlessOfSale, _storageSpaceUsageData);

        if((TeamIndex)teamIndex == _entityIndexManager.TeamIndex)
        {
            PlaySoundEffect(5, 0);
        }
    }

    private void RaiseFeeCalculationEvent()
    {
        _data[0] = CalculateTotalFeeDelegate;
        GameEventHandler.RaiseEvent(GameEventType.CalculateStorageSpaceFee, _data);
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

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);

    }
}