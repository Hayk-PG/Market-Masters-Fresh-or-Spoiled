using Photon.Pun;
using Photon.Realtime;
using Pautik;
using System.Collections.Generic;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    private object[] _roomData = new object[1];



    public override void OnConnectedToMaster()
    {
        GameEventHandler.RaiseEvent(GameEventType.OnConnectedToMaster);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
    }

    public override void OnJoinedLobby()
    {
        GameEventHandler.RaiseEvent(GameEventType.OnJoinedLobby);
    }

    public override void OnJoinedRoom()
    {
        _roomData[0] = PhotonNetwork.CurrentRoom;
        GameEventHandler.RaiseEvent(GameEventType.OnJoinedRoom, _roomData);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        GlobalFunctions.DebugLog($"ReturnCode: {returnCode}/Message: {message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
    }
}