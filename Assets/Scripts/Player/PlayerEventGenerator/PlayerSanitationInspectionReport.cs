using UnityEngine;

public class PlayerSanitationInspectionReport : PlayerBaseEventGenerator
{
    private int _sanitationInspectionTurnCount;
    private bool _isReportTriggered;
    private object[] _reputationData = new object[1];
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
            WrapData(-5, SanitationInspectionReport.IssueTitle, SanitationInspectionReport.IssueMessage);
        }
        else
        {
            WrapData(8, SanitationInspectionReport.NoIssueTitle, SanitationInspectionReport.NoIssueMessage);
        }

        RaiseEvents();
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

    private void WrapData(int reputationPoint, string notificationTitle, string notificationMessage)
    {
        _reputationData[0] = reputationPoint;
        _notificationData[0] = new Notification(NotificationType.DisplayReadNotification, notificationTitle, notificationMessage);
    }

    private void RaiseEvents()
    {
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationOnSanitationInspection, _reputationData);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}