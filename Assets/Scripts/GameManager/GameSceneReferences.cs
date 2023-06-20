using UnityEngine;

public class GameSceneReferences : MonoBehaviour
{
    public static GameSceneReferences Manager { get; private set; }

    [Header("Transforms")]
    [SerializeField] private Transform _playersContainer;

    [Header("HUD")]
    [SerializeField] private PlayerUIGroupManager[] _playerUIGroups;
    [SerializeField] private TeamGroupPanelManager[] _teamGroupPanels;

    [Header("Game Manager")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameTimeManager _gameTimeManager;
    [SerializeField] private GameTurnManager _gameTurnManager;
    [SerializeField] private RemoteRPCWrapper _remoteRPCWrapper;

    // Transforms
    public Transform PlayersContainer => Manager._playersContainer;

    // HUD
    public PlayerUIGroupManager[] PlayerUIGroups => Manager._playerUIGroups;
    public TeamGroupPanelManager[] TeamGroupPanels => Manager._teamGroupPanels;

    // Game Manager
    public GameManager GameManager => Manager._gameManager;
    public GameTimeManager GameTimeManager => Manager._gameTimeManager;
    public GameTurnManager GameTurnManager => Manager._gameTurnManager;
    public RemoteRPCWrapper RemoteRPCWrapper => Manager._remoteRPCWrapper;




    private void Awake()
    {
        Manager = this;
    }
}