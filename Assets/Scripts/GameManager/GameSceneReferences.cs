using UnityEngine;

public class GameSceneReferences : MonoBehaviour
{
    public static GameSceneReferences Manager { get; private set; }
    
    [Header("Game Manager")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameTimeManager _gameTimeManager;
    [SerializeField] private GameTurnManager _gameTurnManager;
    [SerializeField] private RemoteRPCWrapper _remoteRPCWrapper;

    [Header("HUD")]
    [SerializeField] private PlayerUIGroupManager[] _playerUIGroups;
    [SerializeField] private TeamGroupPanelManager[] _teamGroupPanels;
    [SerializeField] private PlayerInventoryUIManager _playerInventoryUIManager;

    [Header("Transforms")]
    [SerializeField] private Transform _playersContainer;

    [Header("Scriptable Objects")]
    [SerializeField] private Items _items;

    // Game Manager
    public GameManager GameManager => Manager._gameManager;
    public GameTimeManager GameTimeManager => Manager._gameTimeManager;
    public GameTurnManager GameTurnManager => Manager._gameTurnManager;
    public RemoteRPCWrapper RemoteRPCWrapper => Manager._remoteRPCWrapper;

    // HUD
    public PlayerUIGroupManager[] PlayerUIGroups => Manager._playerUIGroups;
    public TeamGroupPanelManager[] TeamGroupPanels => Manager._teamGroupPanels;
    public PlayerInventoryUIManager PlayerInventoryUIManager => Manager._playerInventoryUIManager;

    // Transforms
    public Transform PlayersContainer => Manager._playersContainer;

    // Scriptable Objects
    public Items Items => Manager._items;




    private void Awake()
    {
        Manager = this;
    }
}