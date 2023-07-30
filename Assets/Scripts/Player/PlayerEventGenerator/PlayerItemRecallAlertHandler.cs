using UnityEngine;

/// <summary>
/// Handles item recall alert events for the player.
/// </summary>
public class PlayerItemRecallAlertHandler : PlayerBaseEventGenerator
{
    private Notification _notification;
    private short _itemRecallAlertDuration;
    private short _recallLevel;   
    private Sprite[] _recallItemsIcons;
    private short[] _recallItemsIdsArray;
    private object[] _itemRecallAlerNotificationData = new object[1];
    private object[] _itemRecallAlertPopupNotificationData = new object[1];
    private object[] _dispatchRecalItemsData = new object[1];
    private object[] _refundData = new object[1];
    private object[] _reputationData = new object[2];




    /// <summary>
    /// Handles game events for item recall alert.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnTriggerItemRecallAlert(gameEventType, data);
        ExpireItemRecallAlert(gameEventType, data);
        CheckIfSellingItemIsRecallItem(gameEventType, data);
    }

    /// <summary>
    /// Triggers item recall alert based on game event.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void OnTriggerItemRecallAlert(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.TriggerItemRecallAlert)
        {
            return;
        }

        RetrieveItemRecallAlertData(data);
        QueueNotification();
        DisplayPopupNotification(ItemRecallAlertMessage.PopupMessage);
        DispatchRecallItems();
    }

    /// <summary>
    /// Expire the item recall alert based on game event.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
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
        DisplayPopupNotification(ItemRecallAlertMessage.ResolvedPopupMessage);
        DispatchRecallItems();
    }

    /// <summary>
    /// Checks if the selling item is a recall item based on game event.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
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

    /// <summary>
    /// Retrieves data for the item recall alert from the game event.
    /// </summary>
    /// <param name="data">Data associated with the game event.</param>
    private void RetrieveItemRecallAlertData(object[] data)
    {
        UpdateRecallItemsIdsArray((short[])data[0]);
        _itemRecallAlertDuration = (short)data[1];
        _recallLevel = (short)data[2];
    }

    /// <summary>
    /// Updates the recall item IDs array.
    /// </summary>
    /// <param name="recallItemsIdsArray">The array of recall item IDs.</param>
    private void UpdateRecallItemsIdsArray(short[] recallItemsIdsArray)
    {
        _recallItemsIdsArray = recallItemsIdsArray;
    }

    /// <summary>
    /// Queues a notification for the item recall alert.
    /// </summary>
    private void QueueNotification()
    {
        _recallItemsIcons = new Sprite[_recallItemsIdsArray.Length];

        for (int i = 0; i < _recallItemsIdsArray.Length; i++)
        {
            _recallItemsIcons[i] = GameSceneReferences.Manager.Items.Collection.Find(item => item.ID == _recallItemsIdsArray[i]).Icon;
        }

        _itemRecallAlerNotificationData[0] = _notification = new Notification(NotificationType.DisplayNotificationWithImagesAndCallback, ItemRecallAlertMessage.Title, ItemRecallAlertMessage.Message, _recallItemsIcons, RemoveRecallItemsCallback);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _itemRecallAlerNotificationData);
    }

    /// <summary>
    /// Displays a popup notification for the item recall alert.
    /// </summary>
    /// <param name="message">The message to display in the popup notification.</param>
    private void DisplayPopupNotification(string message)
    {
        _itemRecallAlertPopupNotificationData[0] = message;
        GameEventHandler.RaiseEvent(GameEventType.DisplayPopupNotification, _itemRecallAlertPopupNotificationData);
    }

    /// <summary>
    /// Dispatches the recall items based on the item recall alert.
    /// </summary>
    private void DispatchRecallItems()
    {
        _dispatchRecalItemsData[0] = _recallItemsIdsArray;
        GameEventHandler.RaiseEvent(GameEventType.DispatchRecallItems, _dispatchRecalItemsData);
    }

    /// <summary>
    /// Removes the recall items callback and processes the player's response.
    /// </summary>
    private void RemoveRecallItemsCallback()
    {       
        int refundAmount = 0;
        int reputationPoints = 0;

        foreach (var itemButton in GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons)
        {
            reCheckItemIfDestroyedLabel:
            if(itemButton.AssociatedItem == null)
            {
                continue;
            }

            for (int i = 0; i < _recallItemsIdsArray.Length; i++)
            {      
                if(itemButton.AssociatedItem == null)
                {
                    goto reCheckItemIfDestroyedLabel;
                }

                if(itemButton.AssociatedItem.ID != _recallItemsIdsArray[i])
                {
                    continue;
                }

                if (itemButton.ItemSpoilPercentage > 10)
                {
                    refundAmount += itemButton.AssociatedItem.Price;
                }
                else
                {
                    reputationPoints++;
                }

                itemButton.DestroySpoiledItemOnSeparateSale();
            }
        }

        RemoveNotificationCallback();
        GetRefund(isApplicableForRefund: refundAmount > 0, refundAmount: refundAmount);
        UpdateReputation(canIncreaseReputationPoints: reputationPoints > 0, reputationPoints: reputationPoints);
    }

    /// <summary>
    /// Gets the refund for the recalled items if applicable.
    /// </summary>
    /// <param name="isApplicableForRefund">Flag indicating if the player is eligible for a refund.</param>
    /// <param name="refundAmount">The amount to refund.</param>
    private void GetRefund(bool isApplicableForRefund, int refundAmount)
    {
        if (!isApplicableForRefund)
        {
            return;
        }

        _refundData[0] = refundAmount;
        GameEventHandler.RaiseEvent(GameEventType.GetMoneyFromSellingSpoiledItems, _refundData);
    }

    /// <summary>
    /// Updates the player's reputation points if applicable.
    /// </summary>
    /// <param name="canIncreaseReputationPoints">Flag indicating if the player's reputation points can be increased.</param>
    /// <param name="reputationPoints">The number of reputation points to add.</param>
    private void UpdateReputation(bool canIncreaseReputationPoints, int reputationPoints)
    {
        if (!canIncreaseReputationPoints)
        {
            return;
        }

        _reputationData[0] = reputationPoints;
        _reputationData[1] = 0;
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationOnSale, _reputationData);
    }

    /// <summary>
    /// Removes the notification callback.
    /// </summary>
    private void RemoveNotificationCallback()
    {
        GameEventHandler.RaiseEvent(GameEventType.RemoveNotificationCallback, new object[] { _notification });
        _notification = null;
    }
}