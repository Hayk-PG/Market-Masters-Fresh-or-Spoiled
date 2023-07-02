using System.Collections;
using UnityEngine;

public class PlayerCustomerComplaintsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private PlayerReputationManager _playerReputationManager;

    private object[] _notificationData = new object[5];
    private object[] _saleRestrictionData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Event handler for game events.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        bool canReceiveGameEvents = _entityManager.PlayerPhotonview.IsMine;

        if (!canReceiveGameEvents)
        {
            return;
        }

        StartCoroutine(CheckForCustomerComplaintsAfterDelay(gameEventType, data));
    }

    /// <summary>
    /// Coroutine to check for customer complaints after a delay.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private IEnumerator CheckForCustomerComplaintsAfterDelay(GameEventType gameEventType, object[] data)
    {
        yield return new WaitForSeconds(0.1f);

        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            yield break;
        }

        TeamIndex currentTeamTurn = (TeamIndex)data[2];
        bool hasComplainingCustomer = currentTeamTurn != _entityIndexManager.TeamIndex && _playerReputationManager.HasRecentlySoldSpoiledItem;

        if (!hasComplainingCustomer)
        {
            yield break;
        }

        ProcessCustomerComplaints();      
    }

    /// <summary>
    /// Processes customer complaints based on the player's reputation state.
    /// </summary>
    private void ProcessCustomerComplaints()
    {
        switch (_playerReputationManager.ReputationState)
        {
            case ReputationState.Terrible:

                DisplayNotification(SaleRestrictionMessages.FiveTurns);
                ApplySaleRestriction(5);
                break;

            case ReputationState.Poor:

                DisplayNotification(SaleRestrictionMessages.FourTurns);
                ApplySaleRestriction(4);
                break;
            case ReputationState.Neutral:

                DisplayNotification(SaleRestrictionMessages.TwoTurns);
                ApplySaleRestriction(2);
                break;

            case ReputationState.Good:

                DisplayNotification();
                break;

            case ReputationState.Excellent:

                DisplayNotification();
                break;
        }

        _playerReputationManager.SetSpoiledItemSoldStatus(false);
    }

    /// <summary>
    /// Displays a notification with an optional additional message.
    /// </summary>
    /// <param name="additionalMessage">Additional message to be displayed in the notification.</param>
    private void DisplayNotification(string additionalMessage = "")
    {
        int negativeReviewAndPublicityIndex = Random.Range(0, NegativeReviews.Texts.Length);
        _notificationData[0] = NotificationType.DisplayReadNotification;
        _notificationData[1] = NegativeReviews.Texts[negativeReviewAndPublicityIndex].Item1;
        _notificationData[2] = NegativeReviews.Texts[negativeReviewAndPublicityIndex].Item2 + "\n\n" + additionalMessage;
        _notificationData[3] = null;
        _notificationData[4] = 6;
        GameEventHandler.RaiseEvent(GameEventType.DisplayNotification, _notificationData);
    }

    /// <summary>
    /// Applies a sale restriction for a given duration.
    /// </summary>
    /// <param name="saleRestrictionDuration">Duration of the sale restriction.</param>
    private void ApplySaleRestriction(int saleRestrictionDuration)
    {
        _saleRestrictionData[0] = saleRestrictionDuration;
        GameEventHandler.RaiseEvent(GameEventType.RestrictSaleAbility, _saleRestrictionData);
    }

    /// <summary>
    /// Handles incurring financial losses.
    /// </summary>
    private void IncurFinancialLosses()
    {
        // Implementation for incurring financial losses goes here
    }
}