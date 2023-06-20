using Pautik;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPun
{
    public static Network Manager { get; private set; }




    private void Awake()
    {
        Conditions<bool>.Compare(Manager == null, SetInstance, DestroyGameobject);
    }

    private void SetInstance()
    {
        Manager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void DestroyGameobject()
    {
        Destroy(gameObject);
    }

    public void Connect(string nickName, string userId)
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.AuthValues = new AuthenticationValues { UserId = userId };
        }
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public void JoinLobby(TypedLobby typedLobby = null)
    {
        PhotonNetwork.JoinLobby(typedLobby);
    }

    public void CreateRoom(RoomData roomData)
    {
        PhotonNetwork.CreateRoom(roomData.RoomName, roomData.RoomOptions, roomData.TypedLobby, roomData.ExpectedUsers);
    }

    public void JoinRoom(RoomData roomData)
    {
        PhotonNetwork.JoinRoom(roomData.RoomName, roomData.ExpectedUsers);
    }

    public void JoinOrCreateRoom(RoomData roomData)
    {
        PhotonNetwork.JoinOrCreateRoom(roomData.RoomName, roomData.RoomOptions, roomData.TypedLobby, roomData.ExpectedUsers);
    }

    public void JoinRandomRoom(RoomData roomData)
    {
        PhotonNetwork.JoinRandomRoom(roomData.ExpectedCustomRoomProperties, roomData.ExpectedMaxPlayers, roomData.MatchmakingType, roomData.TypedLobby, roomData.SqlLobbyFilter, roomData.ExpectedUsers);
    }

    public void JoinRandomOrCreateRoom(RoomData roomData)
    {
        PhotonNetwork.JoinRandomOrCreateRoom(roomData.ExpectedCustomRoomProperties, roomData.ExpectedMaxPlayers, roomData.MatchmakingType, roomData.TypedLobby, roomData.SqlLobbyFilter, roomData.RoomName,
                                             roomData.RoomOptions, roomData.ExpectedUsers);
    }

    public void LoadLevel(int levelIndex)
    {
        PhotonNetwork.LoadLevel(levelIndex);
    }
}