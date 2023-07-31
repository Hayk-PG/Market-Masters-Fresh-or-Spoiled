using UnityEngine;

/// <summary>
/// Handles the player's inventory spoilage surge events.
/// </summary>
public class PlayerInventorySpoilageSurgeHandler : PlayerBaseEventGenerator
{
    private int _spoilageSurgeDuration;
    private bool _isItemSpoilageSurgeActive;
    private object[] _spoilageRateData = new object[1];



    /// <summary>
    /// Handles the game events related to inventory spoilage surge.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    protected override void OnGameEvent(GameEventType gameEventType, object[] data)
    {        
        IncreaseSpoilageRate(gameEventType, data);
        CheckReleaseSpoilageSurge(gameEventType, data);
    }

    /// <summary>
    /// Increases the spoilage rate when the inventory spoilage surge event is triggered.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void IncreaseSpoilageRate(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.ItemSpoilageSurge)
        {
            return;
        }

        SetItemSpoilageSurgeActiveState(true);
        RetrieveItemSpoilageSurgeData(data);
        RaiseUpdateSpoilageRateEvent(Random.Range(5, 16));
        QueueNotification(ItemsSpoilageSurgeMessage.Title, ItemsSpoilageSurgeMessage.Message);
    }

    /// <summary>
    /// Checks if the inventory spoilage surge can be released based on game time and duration.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void CheckReleaseSpoilageSurge(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        if (!_isItemSpoilageSurgeActive)
        {
            return;
        }

        if (_spoilageSurgeDuration < (float)data[2])
        {
            return;
        }

        RaiseUpdateSpoilageRateEvent(0);
        QueueNotification(ItemsSpoilageSurgeMessage.ResolvedTitle, ItemsSpoilageSurgeMessage.ResolvedMessage);
        SetItemSpoilageSurgeActiveState(false);
    }

    /// <summary>
    /// Retrieves the duration of the inventory spoilage surge from the game event data.
    /// </summary>
    /// <param name="data">Data associated with the game event.</param>
    private void RetrieveItemSpoilageSurgeData(object[] data)
    {
        _spoilageSurgeDuration = (short)data[0];
    }

    /// <summary>
    /// Sets the state of the inventory spoilage surge.
    /// </summary>
    /// <param name="isItemSpoilageSurgeActive">True if the inventory spoilage surge is active, otherwise false.</param>
    private void SetItemSpoilageSurgeActiveState(bool isItemSpoilageSurgeActive)
    {
        _isItemSpoilageSurgeActive = isItemSpoilageSurgeActive;
    }

    /// <summary>
    /// Raises the game event to update the spoilage rate.
    /// </summary>
    /// <param name="spoilageSurgeDuration">The updated spoilage surge duration.</param>
    private void RaiseUpdateSpoilageRateEvent(int spoilageSurgeDuration)
    {
        _spoilageRateData[0] = spoilageSurgeDuration;
        GameEventHandler.RaiseEvent(GameEventType.UpdateSpoilageRate, _spoilageRateData);
    }

    /// <summary>
    /// Queues a notification to be displayed.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The content of the notification.</param>
    private void QueueNotification(string title, string message)
    {
        new Notification(NotificationType.DisplayReadNotification, title, message);
    }
}
