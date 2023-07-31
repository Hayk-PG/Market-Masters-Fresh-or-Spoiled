using UnityEngine;

public class PlayerSanitationInspectionReport : PlayerBaseEventGenerator
{
    private int _sanitationInspectionTurnCount;
    private bool _isReportTriggered;




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

        bool isRandomNumberTriggerNumber = Random.Range(0, 21) == 14;
        bool hasFoundIssue = false;

        if (!isRandomNumberTriggerNumber)
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
        new Notification(NotificationType.DisplayReadNotification, notificationTitle, notificationMessage);
    }
}