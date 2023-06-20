using Photon.Pun;
using UnityEngine;
using Pautik;

public class PlayerNumericChoiceManager : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;

    protected object[] _data = new object[3];

    protected bool _isConfirmedNumberPublished;




    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected virtual void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        ResetConfirmedNumberPublished(gameEventType, data);
        PublishConfirmedNumber(gameEventType, data);
    }

    protected virtual void ResetConfirmedNumberPublished(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        Conditions<bool>.Compare((TeamIndex)data[2] != _entityIndexManager.TeamIndex, () => ToggleConfirmedNumberPublished(), () => ToggleConfirmedNumberPublished(false));
    }

    protected virtual void PublishConfirmedNumber(GameEventType gameEventType, object[] data)
    {
        if (!CanPublishConfirmedNumber(gameEventType))
        {
            return;
        }

        ToggleConfirmedNumberPublished();
        photonView.RPC("PublishConfirmedNumberRPC", RpcTarget.AllViaServer, _entityManager.EntityName, _entityManager.EntityActorNumber, (int)data[0]);
    }

    protected virtual bool CanPublishConfirmedNumber(GameEventType currentGameEventType)
    {
        return currentGameEventType == GameEventType.ConfirmSelectedNumber && photonView.IsMine && !_isConfirmedNumberPublished;
    }

    protected virtual void ToggleConfirmedNumberPublished(bool isConfirmedNumberPublished = true)
    {
        _isConfirmedNumberPublished = isConfirmedNumberPublished;
    }

    [PunRPC]
    private void PublishConfirmedNumberRPC(string entityName, int entityActorNumber, int confirmedNumber)
    {
        _data[0] = entityName;
        _data[1] = entityActorNumber;
        _data[2] = confirmedNumber;

        GameEventHandler.RaiseEvent(GameEventType.PublishConfirmedNumber, _data);
    }
}