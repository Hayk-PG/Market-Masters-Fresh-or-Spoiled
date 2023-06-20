using Photon.Pun;
using Pautik;

public class RemoteRPCWrapper : MonoBehaviourPun
{
    private object[] _botNumericChoiceData = new object[3];
    private object[] _overrideGameTimeData = new object[1];

    private bool IsMasterClient => MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);




    public void PublishBotSelectedNumber(string entityName, int entityActorNumber, int confirmedNumber)
    {
        if (!IsMasterClient)
        {
            return;
        }

        photonView.RPC("PublishBotSelectedNumberRPC", RpcTarget.AllViaServer, entityName, entityActorNumber, confirmedNumber);
    }

    [PunRPC]
    private void PublishBotSelectedNumberRPC(string entityName, int entityActorNumber, int confirmedNumber)
    {
        _botNumericChoiceData[0] = entityName;
        _botNumericChoiceData[1] = entityActorNumber;
        _botNumericChoiceData[2] = confirmedNumber;

        GameEventHandler.RaiseEvent(GameEventType.PublishConfirmedNumber, _botNumericChoiceData);
    }

    public void OverrideGameTime(float targetGameTime)
    {
        if (!IsMasterClient)
        {
            return;
        }

        photonView.RPC("OverrideGameTimeRPC", RpcTarget.AllViaServer, targetGameTime);
    }

    [PunRPC]
    private void OverrideGameTimeRPC(float targetGameTime)
    {
        _overrideGameTimeData[0] = targetGameTime;
        GameEventHandler.RaiseEvent(GameEventType.OverrideGameTime, _overrideGameTimeData);
    }
}