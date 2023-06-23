using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
    [Header("Turns")]
    [SerializeField] private TeamIndex _previousTeamTurn;
    [SerializeField] private TeamIndex _currentTeamTurn;

    private object[] _data = new object[4];

    public TeamIndex PreviousTeamTurn
    {
        get => _previousTeamTurn;
        private set => _previousTeamTurn = value;
    }
    public TeamIndex CurrentTeamTurn
    {
        get => _currentTeamTurn;
        private set => _currentTeamTurn = value;
    }
    public int TurnCount { get; private set; }
    public bool IsFirstTeamTurn { get; private set; }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateTurns(gameEventType, data);
    }

    private void UpdateTurns(GameEventType gameEventType, object[] data)
    {
        bool canToggleTurns = gameEventType == GameEventType.UpdateGameTime && GameSceneReferences.Manager.GameManager.IsGameStarted;

        if (!canToggleTurns)
        {
            return;
        }

        float seconds = (float)data[0];
        float roundDuration = (float)data[1] - 1;

        if(seconds >= roundDuration)
        {
            ToggleTurns();
            CounTurns();
            PublishTurns();
        }
    }

    public void ToggleTurns()
    {
        IsFirstTeamTurn = !IsFirstTeamTurn;
        PreviousTeamTurn = CurrentTeamTurn;
        CurrentTeamTurn = IsFirstTeamTurn ? TeamIndex.Team1 : TeamIndex.Team2;
    }

    public void CounTurns()
    {
        TurnCount++;
    }

    private void PublishTurns()
    {
        _data[0] = IsFirstTeamTurn;
        _data[1] = PreviousTeamTurn;
        _data[2] = CurrentTeamTurn;
        _data[3] = TurnCount;

        GameEventHandler.RaiseEvent(GameEventType.UpdateGameTurn, _data);
    }
}