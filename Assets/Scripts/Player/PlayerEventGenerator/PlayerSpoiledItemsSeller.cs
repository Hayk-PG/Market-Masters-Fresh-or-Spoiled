using System.Collections;
using UnityEngine;

public class PlayerSpoiledItemsSeller : PlayerBaseEventGenerator
{
    protected int _executionTurnCount;
    protected object[] _notificationData = new object[1];
    protected Notification _notification;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (_notification != null)
        {
            return;
        }

        if (!IsRandomNumberTriggerNumber(8))
        {
            return;
        }

        SetExecutionTurnCount(turnCount: (int)data[3]);
        QueueNotification();
        StartCoroutine(CheckRemoveNotificationCallback());
    }

    protected virtual bool IsRandomNumberTriggerNumber(int triggerNumber)
    {
        return Random.Range(0, 11) == triggerNumber;
    }

    protected virtual void QueueNotification()
    {
        _notificationData[0] = _notification = new Notification(NotificationType.DisplayNotificationWithCallback, SpoiledItemsSellOfferMessage.Title, SpoiledItemsSellOfferMessage.Message, delegate { GameEventHandler.RaiseEvent(GameEventType.SellSpoiledItems); _notification = null; });
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }

    protected virtual void SetExecutionTurnCount(int turnCount)
    {
        _executionTurnCount = turnCount;
    }

    protected virtual IEnumerator CheckRemoveNotificationCallback()
    {
        while(_executionTurnCount > GameSceneReferences.Manager.GameTurnManager.TurnCount - 3)
        {
            yield return null;
        }

        if(_notification == null)
        {
            yield break;
        }

        GameEventHandler.RaiseEvent(GameEventType.RemoveNotificationCallback, _notificationData);
        _notification = null;       
    }
}