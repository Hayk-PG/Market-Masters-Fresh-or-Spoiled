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

    private object[] _teamCombinedSellingItemData = new object[3];

    public TeamIndex TeamIndex => _teamIndex;
    public PlayerUIGroupManager[] PlayerUIGroups => _playerUIGroups;
    public int TeamCombinedSellingItemQuantity { get; private set; }
    public int TeamCombinedSellingItemSpoilPercentage { get; private set; }




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

    public void GetTeamCombinedSellingItemData(int itemQuntity, int itemSpoilPercentage)
    {
        TeamCombinedSellingItemQuantity = (TeamCombinedSellingItemQuantity + itemQuntity) > 0 ? TeamCombinedSellingItemQuantity + itemQuntity : 0;
        TeamCombinedSellingItemSpoilPercentage = itemSpoilPercentage;
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
            _teamCombinedSellingItemData[0] = TeamCombinedSellingItemQuantity;
            _teamCombinedSellingItemData[1] = TeamCombinedSellingItemSpoilPercentage;
            _teamCombinedSellingItemData[2] = TeamIndex;

            GameSceneReferences.Manager.RemoteRPCWrapper.PublishTeamCombinedSellingItemQuantity(_teamCombinedSellingItemData);

            TeamCombinedSellingItemQuantity = 0;
        }
    }

    private void SetTurnIndicatorActive(bool isTeamTurn)
    {
        GlobalFunctions.CanvasGroupActivity(_frameCanvasGroup, isTeamTurn);
    }
}