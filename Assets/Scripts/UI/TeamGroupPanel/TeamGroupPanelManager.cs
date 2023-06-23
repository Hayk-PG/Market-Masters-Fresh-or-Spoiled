using UnityEngine;
using Pautik;

public class TeamGroupPanelManager : MonoBehaviour
{
    [Header("Owner Team")]
    [SerializeField] private TeamIndex _teamIndex;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _frameCanvasGroup;
    [SerializeField] private TeamStockUI _teamStockUI;

    [Header("Player UI Groups")]
    [SerializeField] private PlayerUIGroupManager[] _playerUIGroups;

    private object[] _teamCombinedNumberData = new object[2];

    public TeamIndex TeamIndex => _teamIndex;
    public PlayerUIGroupManager[] PlayerUIGroups => _playerUIGroups;
    public int TeamCombinedSellingItemQuantity { get; private set; }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        TrackGameTurn(gameEventType, data);
        PublishTeamCombinedNumber(gameEventType);
    }

    public void ChangeTeamIndex(TeamIndex teamIndex)
    {
        _teamIndex = teamIndex;
        _teamStockUI.SetControllerTeam(teamIndex);
    }

    public void UpdateTeamCombinedNumber(int number)
    {
        TeamCombinedSellingItemQuantity = (TeamCombinedSellingItemQuantity + number) > 0 ? TeamCombinedSellingItemQuantity + number : 0;
    }

    public void SetPlayerUIGroupOwnership(int playerUIGroupIndex, string ownerName, int actorNumber, bool isLocalPlayerOwner = false)
    {
        PlayerUIGroups[playerUIGroupIndex].SetOwnership(ownerName, actorNumber, isLocalPlayerOwner);
    }

    private void TrackGameTurn(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        SetTurnIndicatorActive(isTeamTurn: (TeamIndex)data[2] == _teamIndex);
    }

    private void PublishTeamCombinedNumber(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if(TeamCombinedSellingItemQuantity > 0)
        {
            _teamCombinedNumberData[0] = TeamCombinedSellingItemQuantity;
            _teamCombinedNumberData[1] = TeamIndex;
            GameSceneReferences.Manager.RemoteRPCWrapper.PublishTeamCombinedSellingItemQuantity((byte)TeamCombinedSellingItemQuantity, (byte)TeamIndex);
            TeamCombinedSellingItemQuantity = 0;
        }
    }

    private void SetTurnIndicatorActive(bool isTeamTurn)
    {
        GlobalFunctions.CanvasGroupActivity(_frameCanvasGroup, isTeamTurn);
    }
}