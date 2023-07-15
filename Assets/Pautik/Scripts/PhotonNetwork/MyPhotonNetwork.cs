using Photon.Pun;
using Photon.Realtime;

namespace Pautik
{
    public class MyPhotonNetwork
    {
        public static string Nickname
        {
            get => PhotonNetwork.NickName;
            set => PhotonNetwork.NickName = value;
        }

        /// <summary>
        /// Gets a value indicating whether Photon is in offline mode.
        /// </summary>
        public static bool IsOfflineMode => PhotonNetwork.OfflineMode;

        /// <summary>
        /// Gets a value indicating whether the client is connected to the Photon server.
        /// </summary>
        public static bool IsConnected => PhotonNetwork.IsConnected;

        /// <summary>
        /// Gets a value indicating whether the client is connected and ready to send/receive messages.
        /// </summary>
        public static bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;

        /// <summary>
        /// Gets a value indicating whether the client is currently in a room.
        /// </summary>
        public static bool IsInRoom => PhotonNetwork.InRoom;

        /// <summary>
        /// Gets a value indicating whether the client is currently in a lobby.
        /// </summary>
        public static bool IsInLobby => PhotonNetwork.InLobby;

        /// <summary>
        /// Gets an array of all players in the current room.
        /// </summary>
        public static Player[] PlayersList => PhotonNetwork.PlayerList;

        /// <summary>
        /// Gets the local player.
        /// </summary>
        public static Player LocalPlayer => PhotonNetwork.LocalPlayer;

        /// <summary>
        /// Gets the master client of the current room.
        /// </summary>
        public static Player MasterClient => PhotonNetwork.MasterClient;

        /// <summary>
        /// Gets the current room.
        /// </summary>
        public static Room CurrentRoom => PhotonNetwork.CurrentRoom;

        /// <summary>
        /// Gets the current lobby.
        /// </summary>
        public static TypedLobby CurrentLobby => PhotonNetwork.CurrentLobby;

        /// <summary>
        /// Manages the offline mode of Photon.
        /// </summary>
        /// <param name="isOfflineMode">True to enable offline mode, false to disable it.</param>
        public static void ManageOfflineMode(bool isOfflineMode)
        {
            if (IsOfflineMode == isOfflineMode)
                return;

            PhotonNetwork.OfflineMode = isOfflineMode;
        }

        public static void SetAuthValues(string userId)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues { UserId = userId };
        }

        /// <summary>
        /// Determines whether the specified player is the master client.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>True if the player is the master client, false otherwise.</returns>
        public static bool IsMasterClient(Player player)
        {
            return player.IsMasterClient;
        }

        /// <summary>
        /// Determines whether the local client is the owner of the PhotonView.
        /// </summary>
        /// <param name="photonView">The PhotonView to check.</param>
        /// <returns>True if the local client is the owner of the PhotonView, false otherwise.</returns>
        public static bool AmPhotonViewOwner(PhotonView photonView)
        {
            return photonView.IsMine && photonView.AmOwner;
        }

        /// <summary>
        /// Sends all outgoing commands immediately.
        /// </summary>
        public static void SendAllOutgoingCommands()
        {
            PhotonNetwork.SendAllOutgoingCommands();
        }

        /// <summary>
        /// Determines whether the current lobby matches the desired lobby type and name.
        /// </summary>
        /// <param name="lobbyType">The desired lobby type.</param>
        /// <param name="lobbyName">The desired lobby name.</param>
        /// <returns>True if the current lobby matches the desired lobby type and name, false otherwise.</returns>
        public static bool IsDesiredLobby(LobbyType lobbyType, string lobbyName)
        {
            return CurrentLobby.Type == lobbyType && CurrentLobby.Name == lobbyName;
        }

        /// <summary>
        /// Loads the specified game scene.
        /// </summary>
        public static void LoadLevel()
        {
            PhotonNetwork.LoadLevel(MyScene.Manager.GameSceneName);
        }
    }
}