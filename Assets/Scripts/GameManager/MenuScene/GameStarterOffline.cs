using System.Collections;
using UnityEngine;
using Pautik;

public class GameStarterOffline : MonoBehaviour
{
    private WaitForSeconds _gameStartDelay = new WaitForSeconds(1f);




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        StartOfflineGame(gameEventType);
    }

    private void StartOfflineGame(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SelectOfflineGame)
        {
            return;
        }

        StartCoroutine(StartOfflineGameAfterDelay());
    }

    private IEnumerator StartOfflineGameAfterDelay()
    {
        yield return _gameStartDelay;
        MyPhotonNetwork.ManageOfflineMode(true);
        MyPhotonNetwork.Nickname = "Player";
        MyPhotonNetwork.SetAuthValues("Player");
        Network.Manager.CreateRoom(new RoomData { RoomName = "Room" });
    }
}