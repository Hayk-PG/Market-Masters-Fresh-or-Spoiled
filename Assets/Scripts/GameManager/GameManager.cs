using Photon.Pun;
using Pautik;

public class GameManager : MonoBehaviourPun
{
    private object[] _data = new object[1];

    /// <summary>
    /// Indicates whether the game has started.
    /// </summary>
    public bool IsGameStarted { get; private set; }
    public bool IsGameEnded { get; private set; }

    


    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles game events.
    /// </summary>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        CheckStartGame(gameEventType, data);
        CheckGameEnd(gameEventType, data);
    }

    /// <summary>
    /// Checks if the game can start based on the game event and data received.
    /// </summary>
    private void CheckStartGame(GameEventType gameEventType, object[] data)
    {
        if(!IsGameStarted && gameEventType == GameEventType.UpdateGameTime)
        {
            AnnounceGameStart(time: (float)data[0]);            
        }
    }

    private void CheckGameEnd(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        if (!IsGameStarted)
        {
            return;
        }

        float remainingTime = (float)data[2];

        if(remainingTime <= 0)
        {
            IsGameEnded = true;
            GameEventHandler.RaiseEvent(GameEventType.EndGame);
        }
    }

    /// <summary>
    /// Announces the start of the game if conditions are met.
    /// </summary>
    private void AnnounceGameStart(float time)
    {
        bool canAnnounceGameStart = !IsGameStarted && MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) && time <= 0f;

        if (canAnnounceGameStart)
        {
            StartGameLocally();
            photonView.RPC("PublishGameStartEvent", RpcTarget.AllViaServer, true);
        }
    }

    /// <summary>
    /// Sets the game to start immediately on the local side.
    /// </summary>
    private void StartGameLocally()
    {
        IsGameStarted = true;
    }

    [PunRPC]
    private void PublishGameStartEvent(bool isGameStarted)
    {
        _data[0] = IsGameStarted = isGameStarted;
        GameEventHandler.RaiseEvent(GameEventType.StartGame, _data);
    }
}