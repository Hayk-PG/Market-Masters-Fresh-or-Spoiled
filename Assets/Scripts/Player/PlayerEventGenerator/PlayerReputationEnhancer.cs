
public class PlayerReputationEnhancer : PlayerBaseEventGenerator
{
    private bool _isCharitableGivingNotificationDisplayed;
    private int _charitableGivingNotificationTurn;
    private int _nextCharitableGivingNotificationTurn;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleGameTurnUpdateEvent(gameEventType, data);
    }

    /// <summary>
    /// Handles the actions on the game turn update event.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void HandleGameTurnUpdateEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        DisplayCharitableGivingNotification(data);
        ResetCharitableGivingNotificationDisplayState(data);
    }

    /// <summary>
    /// Displays the charitable giving notification if the conditions are met.
    /// </summary>
    /// <param name="data">Data associated with the game turn update event.</param>
    private void DisplayCharitableGivingNotification(object[] data)
    {
        if (_entityIndexManager.TeamIndex == (TeamIndex)data[2])
        {
            return;
        }

        if ((int)_playerReputationManager.ReputationState > (int)ReputationState.Poor)
        {
            return;
        }

        if (_isCharitableGivingNotificationDisplayed)
        {
            return;
        }
      
        DisplayNotification();
        RegisterCharitableGivingNotificationTime(turnCount: (int)data[3]);
    }

    /// <summary>
    /// Registers the turn count for displaying the charitable giving notification.
    /// </summary>
    /// <param name="turnCount">The current turn count.</param>
    private void RegisterCharitableGivingNotificationTime(int turnCount)
    {
        _charitableGivingNotificationTurn = turnCount;
        _nextCharitableGivingNotificationTurn = _charitableGivingNotificationTurn + 25;
        _isCharitableGivingNotificationDisplayed = true;
    }

    /// <summary>
    /// Resets the charitable giving notification display state if necessary.
    /// </summary>
    /// <param name="data">Data associated with the game turn update event.</param>
    private void ResetCharitableGivingNotificationDisplayState(object[] data)
    {
        if((int)data[3] > _nextCharitableGivingNotificationTurn && _isCharitableGivingNotificationDisplayed)
        {
            _isCharitableGivingNotificationDisplayed = false;
        }
    }

    /// <summary>
    /// Displays the charitable giving notification.
    /// </summary>
    private void DisplayNotification()
    {
        new Notification(NotificationType.DisplayNotificationWithCallback, ReputationBoostMessage.CharitableGivingTitle, ReputationBoostMessage.CharitableGivingMessage, UpdateReputationForItemExchangeCallback);
    }

    /// <summary>
    /// Callback function for updating reputation when the charitable giving notification is accepted.
    /// </summary>
    private void UpdateReputationForItemExchangeCallback()
    {
        int reputationPoints = 0;

        foreach (var inventoryItemButton in GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons)
        {
            if(inventoryItemButton.AssociatedItem == null)
            {
                continue;
            }

            inventoryItemButton.DestroySpoiledItemOnSeparateSale();
            reputationPoints += 2;
        }

        if(reputationPoints == 0)
        {
            return;
        }

        new Reputation(reputationPoints);
    }
}
