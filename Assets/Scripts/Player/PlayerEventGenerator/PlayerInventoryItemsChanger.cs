using UnityEngine;

public class PlayerInventoryItemsChanger : PlayerSpoiledItemsSeller
{
    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.RecordSaleAttempts)
        {
            return;
        }

        if(_notification != null)
        {
            return;
        }

        int successfulSaleAttempts = (int)data[0];
        int failedSaleAttempts = (int)data[1];

        if (failedSaleAttempts < 4)
        {
            return;
        }

        if (!IsRandomNumberTriggerNumber(1))
        {
            return;
        }

        SetExecutionTurnCount(GameSceneReferences.Manager.GameTurnManager.TurnCount);
        QueueNotification();
        StartCoroutine(CheckRemoveNotificationCallback());
    }

    protected override bool IsRandomNumberTriggerNumber(int triggerNumber)
    {
        return Random.Range(0, 3) == triggerNumber;
    }

    protected override void QueueNotification()
    {
        _notificationData[0] = _notification = new Notification(NotificationType.DisplayNotificationWithCallback, ReplaceUnsoldItemsOfferMessage.Title, ReplaceUnsoldItemsOfferMessage.Message, delegate { GameEventHandler.RaiseEvent(GameEventType.ActivateItemsDroppingHelicopter); _notification = null; });
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}