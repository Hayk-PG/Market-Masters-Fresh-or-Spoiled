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

    private object[] _data = new object[1];

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
        UpdateLastSuccessfulSellTurn(turnCount);
    }

    /// <summary>
    /// Updates the reputation points with the given value, ensuring it stays within the range of 0 to 100.
    /// </summary>
    /// <param name="reputationPoints">The reputation points to add.</param>
    private void UpdateReputationPoints(int reputationPoints)
    {
        ReputationPoints = (ReputationPoints + reputationPoints) > 100 ? 100 : (ReputationPoints + reputationPoints) < 0 ? 0 : ReputationPoints + reputationPoints;
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
            return;
        }

        WrapData();
        SetPreviousReputationState();
        GameEventHandler.RaiseEvent(GameEventType.DisplayPopupNotification, _data);
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
    /// Sets the previous reputation state to the current reputation state.
    /// </summary>
    private void SetPreviousReputationState()
    {
        _previousReputationState = ReputationState;
    }

    /// <summary>
    /// Wraps the data based on the current reputation state.
    /// </summary>
    private void WrapData()
    {
        switch (ReputationState)
        {
            case ReputationState.Terrible: _data[0] = ReputationMessages.TerribleReputationMessage; break;
            case ReputationState.Poor: _data[0] = ReputationMessages.PoorReputationMessage; break;

            case ReputationState.Neutral:
                bool isAdvancing = (int)_previousReputationState < (int)ReputationState;

                if (isAdvancing)
                {
                    _data[0] = ReputationMessages.AdvanceNeutralReputationMessage;
                }
                else
                {
                    _data[0] = ReputationMessages.FallNeutralReputationMessage;
                }
                break;

            case ReputationState.Good: _data[0] = ReputationMessages.GoodReputationMessage; break;
            case ReputationState.Excellent: _data[0] = ReputationMessages.ExcellentReputationMessage; break;
        }
    }
}