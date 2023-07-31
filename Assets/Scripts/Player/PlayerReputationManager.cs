using UnityEngine;

/// <summary>
/// Manages the reputation of a player.
/// </summary>
public class PlayerReputationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;

    [Header("Reputation Status")]
    [SerializeField] private ReputationState _previousReputationState;
    [SerializeField] private ReputationState _currentReputationState;
    [SerializeField] private int _reputationPoints;

    private int _lastSuccessfulSellTurn = 0;
    private object[] _reputationPointsData = new object[1];

    public bool HasRecentlySoldSpoiledItem { get; private set; }
    public bool HasReputationBeenChanged { get; private set; }

    /// <summary>
    /// Gets the current reputation state.
    /// </summary>
    public ReputationState ReputationState
    {
        get => _currentReputationState;
        private set => _currentReputationState = value;
    }

    /// <summary>
    /// Gets the reputation points.
    /// </summary>
    public int ReputationPoints
    {
        get => _reputationPoints;
        private set => _reputationPoints = value;
    }




    private void Awake()
    {
        UpdateReputationPoints(50);
        UpdateReputationState();
        SetPreviousReputationState();
    }

    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!_entityManager.PlayerPhotonview.IsMine)
        {
            return;
        }

        UpdateReputationOnSale(gameEventType, data);
        UpdateReputationOnNoSale(gameEventType, data);
        UpdateReputation(gameEventType, data);
    }

    /// <summary>
    /// Updates the reputation when a sale is made.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Additional data related to the event.</param>
    private void UpdateReputationOnSale(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateReputationOnSale)
        {
            return;
        }

        int sellingItemQuantity = (int)data[0];
        int sellingItemSpoilPercentage = (int)data[1];
        int pointsForSellingFreshItems = sellingItemQuantity * 2;

        float penaltyPointsForSellingSpoiledItems = sellingItemQuantity * 4;
        float penaltyPointsBasedOnSpoilPercentage = penaltyPointsForSellingSpoiledItems / 100f * sellingItemSpoilPercentage;
        float finalReputationPenaltyPoints = penaltyPointsForSellingSpoiledItems + penaltyPointsBasedOnSpoilPercentage;
        float itemsFullHealthCombined = sellingItemQuantity * 100f;

        bool isNoticablySpoiled = (itemsFullHealthCombined / sellingItemSpoilPercentage) < 10f;

        if (isNoticablySpoiled)
        {
            UpdateReputationPoints(-Mathf.RoundToInt(finalReputationPenaltyPoints));
            UpdateReputationState();
            NotifyReputationChange();
            SetSpoiledItemSoldStatus(true);
        }
        else
        {
            UpdateReputationPoints(pointsForSellingFreshItems);
            UpdateReputationState();
            NotifyReputationChange();
            UpdateLastSuccessfulSellTurn(GameSceneReferences.Manager.GameTurnManager.TurnCount);
        }
    }

    /// <summary>
    /// Updates the reputation when no sale is made during a turn.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Additional data related to the event.</param>
    private void UpdateReputationOnNoSale(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        TeamIndex currentTeamTurn = (TeamIndex)data[2];
        int turnCount = (int)data[3];

        if(turnCount == 0 || currentTeamTurn == _entityIndexManager.TeamIndex)
        {
            return;
        }

        int noSellPenaltyPoint = (turnCount - 1) - _lastSuccessfulSellTurn;
        UpdateReputationPoints(-noSellPenaltyPoint);
        UpdateReputationState();
        NotifyReputationChange();
        UpdateLastSuccessfulSellTurn(turnCount);
    }

    private void UpdateReputation(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateReputation)
        {
            return;
        }

        UpdateReputationPoints( reputationPoints: (int)data[0]);
        UpdateReputationState();
        NotifyReputationChange();
    }

    /// <summary>
    /// Updates the reputation points with the given value, ensuring it stays within the range of 0 to 100.
    /// </summary>
    /// <param name="reputationPoints">The reputation points to add.</param>
    private void UpdateReputationPoints(int reputationPoints)
    {
        ReputationPoints = (ReputationPoints + reputationPoints) > 100 ? 100 : (ReputationPoints + reputationPoints) < 0 ? 0 : ReputationPoints + reputationPoints;
        _reputationPointsData[0] = ReputationPoints;
        GameEventHandler.RaiseEvent(GameEventType.SendReputationPoints, _reputationPointsData);
    }


    /// <summary>
    /// Updates the current reputation state based on the reputation points.
    /// </summary>
    private void UpdateReputationState()
    {
        ReputationState = ReputationFormula.ReputationState(ReputationPoints);
    }

    /// <summary>
    /// Notifies the reputation change by displaying a popup notification if the reputation state has changed.
    /// </summary>
    private void NotifyReputationChange()
    {
        if(_previousReputationState == ReputationState)
        {
            ToggleReputationChangeFlag(false);
            return;
        }

        SetRightMessageForPopupNotificationAndDisplay();
        SetPreviousReputationState();
        ToggleReputationChangeFlag(true);
    }

    /// <summary>
    /// Updates the last successful sell turn count.
    /// </summary>
    /// <param name="turnCount">The turn count to set.</param>
    private void UpdateLastSuccessfulSellTurn(int turnCount)
    {
        _lastSuccessfulSellTurn = turnCount;        
    }

    /// <summary>
    /// Sets the status indicating whether the player has recently sold a spoiled item.
    /// </summary>
    /// <param name="hasRecentlySoldSpoiledItem">Boolean value indicating if the player has recently sold a spoiled item.</param>
    public void SetSpoiledItemSoldStatus(bool hasRecentlySoldSpoiledItem)
    {
        HasRecentlySoldSpoiledItem = hasRecentlySoldSpoiledItem;
    }

    /// <summary>
    /// Sets the previous reputation state to the current reputation state.
    /// </summary>
    private void SetPreviousReputationState()
    {
        _previousReputationState = ReputationState;
    }

    /// <summary>
    /// Toggles the flag indicating whether the player's reputation has been changed.
    /// </summary>
    /// <param name="hasReputationBeenChanged">Boolean value indicating if the player's reputation has been changed.</param>
    private void ToggleReputationChangeFlag(bool hasReputationBeenChanged)
    {
        HasReputationBeenChanged = hasReputationBeenChanged;
    }

    /// <summary>
    /// Sets the right message for the popup notification based on the reputation state and displays it.
    /// </summary>
    private void SetRightMessageForPopupNotificationAndDisplay()
    {
        switch (ReputationState)
        {
            case ReputationState.Terrible: InitializePopupNotification(ReputationMessages.TerribleReputationMessage); break;
            case ReputationState.Poor: InitializePopupNotification(ReputationMessages.PoorReputationMessage); break;

            case ReputationState.Neutral:
                bool isAdvancing = (int)_previousReputationState < (int)ReputationState;

                if (isAdvancing)
                {
                    InitializePopupNotification(ReputationMessages.AdvanceNeutralReputationMessage);
                }
                else
                {
                    InitializePopupNotification(ReputationMessages.FallNeutralReputationMessage);
                }
                break;

            case ReputationState.Good: InitializePopupNotification(ReputationMessages.GoodReputationMessage); break;
            case ReputationState.Excellent: InitializePopupNotification(ReputationMessages.ExcellentReputationMessage); break;
        }
    }

    /// <summary>
    /// Initializes a popup notification with the given message.
    /// </summary>
    /// <param name="message">The message for the popup notification.</param>
    private void InitializePopupNotification(string message)
    {
        new PopupNotification(message);
    }
}