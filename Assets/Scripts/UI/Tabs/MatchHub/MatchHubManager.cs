using UnityEngine;

public class MatchHubManager : BaseMainHUDTab
{
    [Header("Buttons")]
    [SerializeField] private Btn _roomCreationButton;
    [SerializeField] private Btn _roomBrowseButton;
    [SerializeField] private Btn _quickPlayButton;
 



    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;

        _roomCreationButton.OnSelect += OnRoomCreateButtonSelect;
        _quickPlayButton.OnSelect += OnQuickPlayButtonSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PerformOpenCheck(gameEventType, data);
    }

    private void PerformOpenCheck(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.OnConnectedToMaster)
        {
            return;
        }

        Open();
    }

    private void OnRoomCreateButtonSelect()
    {
        Network.Manager.CreateRoom(new RoomData { RoomName = "Room" });
    }

    private void OnQuickPlayButtonSelect()
    {
        Network.Manager.JoinRandomRoom(new RoomData());
    }
}