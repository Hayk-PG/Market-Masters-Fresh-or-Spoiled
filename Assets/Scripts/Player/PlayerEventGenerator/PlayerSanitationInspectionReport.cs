using UnityEngine;

public class PlayerSanitationInspectionReport : PlayerBaseEventGenerator
{
    private int _sanitationInspectionTurnCount;
    private bool _isReportTriggered;
    private object[] _notificationData = new object[1];




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        ExecuteOnTurnUpdate(gameEventType, data);
    }

    private void ExecuteOnTurnUpdate(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        TriggerSanitationInspectionReport(data);
        ResetReportTriggeredState(data);
    }

    private void TriggerSanitationInspectionReport(object[] data)
    {
        if (_isReportTriggered)
        {
            return;
        }

        int randomTriggerValue = Random.Range(0, 11);
        int triggerValue = 6;
        bool hasFoundIssue = false;

        if (randomTriggerValue != triggerValue)
        {
            return;
        }

        _isReportTriggered = true;
        _sanitationInspectionTurnCount = (int)data[3];
        
        foreach (var inventoryItemButton in GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons)
        {
            if (inventoryItemButton.AssociatedItem == null)
            {
                continue;
            }

            if (inventoryItemButton.ItemSpoilPercentage > 20)
            {
                hasFoundIssue = true;
                break;
            }
        }

        if (hasFoundIssue)
        {
            UpdateReputation(-5);
            QueueNotification(SanitationInspectionReport.IssueTitle, SanitationInspectionReport.IssueMessage);
        }
        else
        {
            UpdateReputation(8);
            QueueNotification(SanitationInspectionReport.NoIssueTitle, SanitationInspectionReport.NoIssueMessage);
        }
    }

    private void ResetReportTriggeredState(object[] data)
    {
        if (!_isReportTriggered)
        {
            return;
        }

        int reportTriggeredStateResetThreshold = _sanitationInspectionTurnCount + 10;
        int currentTurnCount = (int)data[3];

        if(currentTurnCount <= reportTriggeredStateResetThreshold)
        {
            return;
        }

        _isReportTriggered = false;
    }

    private void UpdateReputation(int reputationPoints)
    {
        new Reputation(reputationPoints);
    }

    private void QueueNotification(string notificationTitle, string notificationMessage)
    {
        _notificationData[0] = new Notification(NotificationType.DisplayReadNotification, notificationTitle, notificationMessage);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}