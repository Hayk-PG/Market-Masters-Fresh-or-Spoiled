using UnityEngine;
using Pautik;

public class PlayerReputationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;

    [Header("Reputation Status")]
    [SerializeField] private ReputationState _previousReputationState;
    [SerializeField] private ReputationState _currentReputationState;
    [SerializeField] private int _reputationPoints;

    private object[] _data = new object[1];

    private string ExcellentReputationMessage
    {
        get => $"{WhiteText("Congratulations!")} {PartiallyTransparentText("You have reached an")} {WhiteText("Excellent Reputation!")}";
    }
    private string GoodReputationMessage
    {
        get => $"{PartiallyTransparentText("Your hard work has paid off! You are now at the")} {WhiteText("Good Reputation")} {PartiallyTransparentText("level.")}";
    }
    private string AdvanceNeutralReputationMessage
    {
        get => $"{WhiteText("Well done!")} {PartiallyTransparentText("You have advanced to the next reputation tier:")} {WhiteText("Neutral Reputation.")}";
    }
    private string PoorReputationMessage
    {
        get => $"{WhiteText("Warning!")} {PartiallyTransparentText("Your reputation has decreased to")} {WhiteText("Poor.")} {PartiallyTransparentText("Take action to improve it!")}";
    }
    private string TerribleReputationMessage
    {
        get => $"{WhiteText("Oops!")} {PartiallyTransparentText("Your reputation has dropped to")} {WhiteText("Terrible.")} {PartiallyTransparentText("Time to turn things around!")}";
    }
    private string FallNeutralReputationMessage
    {
        get => $"{PartiallyTransparentText("Your reputation has fallen to")} {WhiteText("Neutral.")} {PartiallyTransparentText("Work on enhancing your image.")}";
    }
    public ReputationState ReputationState
    {
        get => _currentReputationState;
        private set => _currentReputationState = value;
    }
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

        UpdateReputationOnSpoiledSale(gameEventType, data);
    }

    private void UpdateReputationOnSpoiledSale(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateReputationOnSpoiledSale)
        {
            return;
        }

        int sellingItemQuantity = (int)data[0];
        int sellingItemSpoilPercentage = (int)data[1];
        float penaltyPointsOnItemQuantity = sellingItemQuantity * 4;
        float penaltyPointsOnItemSpoilPercentage = penaltyPointsOnItemQuantity / 100f * sellingItemSpoilPercentage;
        float finalReputationPenaltyPoints = penaltyPointsOnItemQuantity + penaltyPointsOnItemSpoilPercentage;
        float itemsFullHealthCombined = sellingItemQuantity * 100f;
        bool isNoticablySpoiled = (itemsFullHealthCombined / sellingItemSpoilPercentage) < 10f;

        if (isNoticablySpoiled) 
        {
            UpdateReputationPoints(-Mathf.RoundToInt(finalReputationPenaltyPoints));
            UpdateReputationState();
            NotifyReputationChange();
        }
    }

    private void UpdateReputationPoints(int reputationPoints)
    {
        ReputationPoints = (ReputationPoints + reputationPoints) > 100 ? 100 : (ReputationPoints + reputationPoints) < 0 ? 0 : ReputationPoints + reputationPoints;
    }

    private void UpdateReputationState()
    {
        ReputationState = ReputationFormula.ReputationState(ReputationPoints);
    }

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

    private void SetPreviousReputationState()
    {
        _previousReputationState = ReputationState;
    }

    private void WrapData()
    {
        switch (ReputationState)
        {
            case ReputationState.Terrible: _data[0] = TerribleReputationMessage; break;
            case ReputationState.Poor: _data[0] = PoorReputationMessage; break;

            case ReputationState.Neutral:
                bool isAdvancing = (int)_previousReputationState < (int)ReputationState;

                if (isAdvancing)
                {
                    _data[0] = AdvanceNeutralReputationMessage;
                }
                else
                {
                    _data[0] = FallNeutralReputationMessage;
                }
                break;

            case ReputationState.Good: _data[0] = GoodReputationMessage; break;
            case ReputationState.Excellent: _data[0] = ExcellentReputationMessage; break;
        }
    }

    private string PartiallyTransparentText(string text)
    {
        return GlobalFunctions.TextWithColorCode("#FFFFFFC8", text);
    }

    private string WhiteText(string text)
    {
        return GlobalFunctions.TextWithColorCode("#FFFFFF", text);
    }
}