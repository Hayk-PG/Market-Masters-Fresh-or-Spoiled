using System.Collections;
using UnityEngine;

public class PlayerSpoiledItemsSeller : PlayerBaseEventGenerator
{
    protected int _executionTurnCount;
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

        if (!IsRandomNumberTriggerNumber(13))
        {
            return;
        }

        SetExecutionTurnCount(turnCount: (int)data[3]);
        QueueNotification();
        StartCoroutine(CheckRemoveNotificationCallback());
    }

    /// <summary>
    /// Checks if the random number generated matches the trigger number.
    /// </summary>
    /// <param name="triggerNumber">The trigger number to match the random number against.</param>
    /// <returns>True if the random number matches the trigger number; otherwise, false.</returns>
    protected virtual bool IsRandomNumberTriggerNumber(int triggerNumber)
    {
        return Random.Range(0, 21) == triggerNumber;
    }

    /// <summary>
    /// Queues a notification to sell spoiled items with a callback to handle the selling process.
    /// Handles the callback when the player accepts the notification to sell spoiled items.
    /// </summary>
    protected virtual void QueueNotification()
    {
        _notification = new Notification(NotificationType.DisplayNotificationWithCallback, SpoiledItemsSellOfferMessage.Title, SpoiledItemsSellOfferMessage.Message, delegate { GameEventHandler.RaiseEvent(GameEventType.SellSpoiledItems); _notification = null; });
    }

    /// <summary>
    /// Sets the execution turn count for the selling process.
    /// </summary>
    /// <param name="turnCount">The current turn count.</param>
    protected virtual void SetExecutionTurnCount(int turnCount)
    {
        _executionTurnCount = turnCount;
    }

    /// <summary>
    /// Coroutine to check if the notification callback should be removed.
    /// </summary>
    /// <returns>Enumerator to handle the coroutine behavior.</returns>
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

        _notification.RemoveCallbackFromNotificationManager();
        _notification = null;       
    }
}