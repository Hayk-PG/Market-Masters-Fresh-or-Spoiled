using Photon.Realtime;

public class RoomHubManager : BaseMainHUDTab
{
    private Room _joinedRoom;


    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.OnJoinedRoom)
        {
            return;
        }

        OnJoinedRoom(data);
    }

    private void OnJoinedRoom(object[] data)
    {
        _joinedRoom = (Room)data[0];
        Network.Manager.LoadLevel(1);
    }
}