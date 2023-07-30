using UnityEngine;

public class PlayerItemRecallAlertHandler : PlayerBaseEventGenerator
{        
    private short _itemRecallAlertDuration;
    private short _recallLevel;
    private Notification _notification;
    private Sprite[] _recallItemsIcons;
    private short[] _recallItemsIdsArray;
    private object[] _itemRecallAlerNotificationData = new object[1];
    private object[] _itemRecallAlertPopupNotificationData = new object[1];
    private object[] _dispatchRecalItemsData = new object[1];




    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnTriggerItemRecallAlert(gameEventType, data);
        ExpireItemRecallAlert(gameEventType, data);
        CheckIfSellingItemIsRecallItem(gameEventType, data);
    }

    private void OnTriggerItemRecallAlert(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.TriggerItemRecallAlert)
        {
            return;
        }

        RetrieveItemRecallAlertData(data);
        QueueNotification();
        DisplayPopupNotification();
        DispatchRecallItems();
    }

    private void ExpireItemRecallAlert(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (_notification == null)
        {
            return;
        }

        if((int)data[3] <= _itemRecallAlertDuration)
        {
            return;
        }

        RemoveNotificationCallback();
        UpdateRecallItemsIdsArray(null);
        DispatchRecallItems();
    }

    private void CheckIfSellingItemIsRecallItem(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.PublishSellingItemQuantity)
        {
            return;
        }

        if(_recallItemsIdsArray == null)
        {
            return;
        }

        int publishedActorNumber = (int)data[1];

        if(publishedActorNumber != _entityManager.EntityActorNumber)
        {
            return;
        }

        //TODO: Apply penalties to the player
    }

    private void RetrieveItemRecallAlertData(object[] data)
    {
        UpdateRecallItemsIdsArray((short[])data[0]);
        _itemRecallAlertDuration = (short)data[1];
        _recallLevel = (short)data[2];
    }

    private void UpdateRecallItemsIdsArray(short[] recallItemsIdsArray)
    {
        _recallItemsIdsArray = recallItemsIdsArray;
    }

    private void QueueNotification()
    {
        _recallItemsIcons = new Sprite[_recallItemsIdsArray.Length];

        for (int i = 0; i < _recallItemsIdsArray.Length; i++)
        {
            _recallItemsIcons[i] = GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == _recallItemsIdsArray[i]).Icon;
        }

        _itemRecallAlerNotificationData[0] = _notification = new Notification(NotificationType.DisplayReadNotificationWithImages, "", "", _recallItemsIcons);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _itemRecallAlerNotificationData);
    }

    private void DisplayPopupNotification()
    {
        _itemRecallAlertPopupNotificationData[0] = "";
        GameEventHandler.RaiseEvent(GameEventType.DisplayPopupNotification, _itemRecallAlertPopupNotificationData);
    }

    private void DispatchRecallItems()
    {
        _dispatchRecalItemsData[0] = _recallItemsIdsArray;
        GameEventHandler.RaiseEvent(GameEventType.DispatchRecallItems, _dispatchRecalItemsData);
    }

    private void RemoveRecallItemsCallback()
    {
        RemoveNotificationCallback();
        int refundAmount = 0;

        foreach (var itemButton in GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons)
        {
            if(itemButton.AssociatedItem == null)
            {
                continue;
            }

            for (int i = 0; i < _recallItemsIdsArray.Length; i++)
            {
                if(itemButton.AssociatedItem.ID == _recallItemsIdsArray[i])
                {
                    itemButton.DestroySpoiledItemOnSeparateSale();
                    refundAmount += itemButton.AssociatedItem.Price;
                }
            }
        }
    }

    private void RemoveNotificationCallback()
    {
        GameEventHandler.RaiseEvent(GameEventType.RemoveNotificationCallback, new object[] { _notification });
        _notification = null;
    }
}