
public struct RoomData 
{
    public ExitGames.Client.Photon.Hashtable ExpectedCustomRoomProperties { get; set; }
    public Photon.Realtime.MatchmakingMode MatchmakingType { get; set; }
    public Photon.Realtime.TypedLobby TypedLobby { get; set; }
    public Photon.Realtime.RoomOptions RoomOptions { get; set; }
    public string[] ExpectedUsers { get; set; }
    public string SqlLobbyFilter { get; set; }     
    public string RoomName { get; set; }
    public byte ExpectedMaxPlayers { get; set; }
}
