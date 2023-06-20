using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class BotSpawnManager : MonoBehaviourPun
{
    [Header("Bot Prefab")]
    [SerializeField] private BotManager _botManagerPrefab;

    private BotManager _instantiatedBot;

    private int _botActorNumber;
    private int _botUIGroupIndex;
    private int _botTeamIndex;
    private int _botNameOrder;
    private string _botName;




    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonNetworkEvent;
    }

    private void OnPhotonNetworkEvent(EventData eventData)
    {
        ExecuteOnSetPlayerIndexEvent(eventData);
    }

    private void ExecuteOnSetPlayerIndexEvent(EventData eventData)
    {
        if (eventData.Code != EventInfo.Code_SetPlayerIndex)
        {
            return;
        }

        RetrieveData(data: (object[])eventData.CustomData);
        InstantiateBotAfterCheck();
    }

    private void RetrieveData(object[] data)
    {
        _botActorNumber = (int)data[0];
        _botUIGroupIndex = (int)data[1];
        _botTeamIndex = (int)data[2];
        _botNameOrder = (int)data[3];
        _botName = $"CPU-{_botNameOrder}";
    }

    private void InstantiateBotAfterCheck()
    {
        bool isOrderedToInstantiate = _botActorNumber < 0;

        if (!isOrderedToInstantiate)
        {
            return;
        }

        _instantiatedBot = Instantiate(_botManagerPrefab, GameSceneReferences.Manager.PlayersContainer);
        _instantiatedBot.Initialize(_botName, _botActorNumber, _botUIGroupIndex, _botTeamIndex);
        _instantiatedBot.name = _botName;
    }
}