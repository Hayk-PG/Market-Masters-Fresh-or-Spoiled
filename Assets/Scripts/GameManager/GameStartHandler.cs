using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Pautik;

public class GameStartHandler : MonoBehaviourPun
{
    private void OnEnable()
    {
        ManageGameStartEventSubscription(true);
    }

    private void ManageGameStartEventSubscription(bool isSubscribing)
    {
        Conditions<bool>.Compare(isSubscribing, () => GameEventHandler.OnEvent += OnGameEvent, () => GameEventHandler.OnEvent -= OnGameEvent);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.StartGame)
        {
            return;
        }

        IteratePlayerUIGroups();
        ManageGameStartEventSubscription(false);
    }

    private void IteratePlayerUIGroups()
    {
        if (!MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            return;
        }

        int playerUiGroupIndex = 0;

        for (int i = 0; i < GameSceneReferences.Manager.PlayerUIGroups.Length; i++)
        {
            PublishPlayerIndexes(i, playerUiGroupIndex);
            playerUiGroupIndex++;
        }
    }

    private void PublishPlayerIndexes(int i, int playerUiGroupIndex)
    {
        int actorNumber = i < PhotonNetwork.PlayerList.Length ? PhotonNetwork.PlayerList[i].ActorNumber : -1;
        int botName = i < PhotonNetwork.PlayerList.Length ? 0 : i - PhotonNetwork.PlayerList.Length;
        int teamIndex = (playerUiGroupIndex + 1) % 2;

        EventInfo.Content_SetPlayerIndex = new object[]
        {
                actorNumber,              
                playerUiGroupIndex,
                teamIndex,
                botName
        };

        PhotonNetwork.RaiseEvent(EventInfo.Code_SetPlayerIndex, EventInfo.Content_SetPlayerIndex, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }
}