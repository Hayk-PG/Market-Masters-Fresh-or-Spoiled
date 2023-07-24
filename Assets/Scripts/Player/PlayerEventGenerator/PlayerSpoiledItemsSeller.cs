using System.Collections;
using UnityEngine;

public class PlayerSpoiledItemsSeller : PlayerBaseEventGenerator
{
    private int _executionTurnCount;
    private object[] _notificationData = new object[1];
    private Notification _notification;




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        ExecuteOnTurnUpdate(gameEventType, data);
    }

    private void ExecuteOnTurnUpdate(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (_notification != null)
        {
            return;
        }

        if(Random.Range(0, 11) != 8)
        {
            return;
        }

        SetExecutionTurnCount(turnCount: (int)data[3]);
        QueueNotification();
        StartCoroutine(CheckRemoveNotificationCallback());
    }

    private void QueueNotification()
    {
        _notificationData[0] = _notification = new Notification(NotificationType.DisplayNotificationWithCallback, SpoiledItemsSellOfferMessage.Title, SpoiledItemsSellOfferMessage.Message, delegate { GameEventHandler.RaiseEvent(GameEventType.SellSpoiledItems); _notification = null; });
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }

    private void SetExecutionTurnCount(int turnCount)
    {
        _executionTurnCount = turnCount;
    }

    private IEnumerator CheckRemoveNotificationCallback()
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