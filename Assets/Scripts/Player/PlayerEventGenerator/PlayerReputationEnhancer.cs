
public class PlayerReputationEnhancer : PlayerBaseEventGenerator
{
    private object[] _notificationData = new object[1];
    private object[] _reputationUpdateData = new object[1];
    private bool _isCharitableGivingNotificationDisplayed;
    private int _charitableGivingNotificationTurn;
    private int _nextCharitableGivingNotificationTurn;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleGameTurnUpdateEvent(gameEventType, data);
    }

    private void HandleGameTurnUpdateEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        DisplayCharitableGivingNotification(data);
        ResetCharitableGivingNotificationDisplayState(data);
    }

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

    private void RegisterCharitableGivingNotificationTime(int turnCount)
    {
        _charitableGivingNotificationTurn = turnCount;
        _nextCharitableGivingNotificationTurn = _charitableGivingNotificationTurn + 25;
        _isCharitableGivingNotificationDisplayed = true;
    }

    private void ResetCharitableGivingNotificationDisplayState(object[] data)
    {
        if((int)data[3] > _nextCharitableGivingNotificationTurn && _isCharitableGivingNotificationDisplayed)
        {
            _isCharitableGivingNotificationDisplayed = false;
        }
    }

    private void DisplayNotification()
    {
        _notificationData[0] = new Notification(NotificationType.DisplayNotificationWithCallback, ReputationBoostMessage.CharitableGivingTitle, ReputationBoostMessage.CharitableGivingMessage, UpdateReputationForItemExchangeCallback);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }

    private void UpdateReputationForItemExchangeCallback()
    {
        int reputationPoint = 0;

        foreach (var inventoryItemButton in GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons)
        {
            if(inventoryItemButton.AssociatedItem == null)
            {
                continue;
            }

            inventoryItemButton.DestroySpoiledItemOnSeparateSale();
            reputationPoint += 2;
        }

        if(reputationPoint == 0)
        {
            return;
        }

        _reputationUpdateData[0] = reputationPoint;
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationForItemExchange, _reputationUpdateData);
    }
}
