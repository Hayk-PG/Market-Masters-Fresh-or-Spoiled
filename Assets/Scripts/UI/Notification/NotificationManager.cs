using UnityEngine;
using TMPro;
using Pautik;
using System;

public class NotificationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _denyAcceptButtonsCanvasGroup;
    [SerializeField] private CanvasGroup _closeButtonCanvasGroup;
    [SerializeField] private Btn _denyButton;
    [SerializeField] private Btn _acceptButton;
    [SerializeField] private Btn _closeButton;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;

    private NotificationType _notificationType;
    private Action AcceptAction;
    private string _notificationTitle;
    private string _notificationMessage;
    private object[] _data = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _denyButton.OnSelect += OnDeny;
        _closeButton.OnSelect += Close;
        _acceptButton.OnSelect += OnAccept;
    }

    /// <summary>
    /// Handles the GameEventType.DisplayNotification event.
    /// Retrieves data from the event parameters and updates the notification.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.DisplayNotification)
        {
            return;
        }

        RetrieveData(data);
        Open();
        SetTitle();
        UpdateMessage();
        OnReadonlyNotification();
    }

    /// <summary>
    /// Retrieves data from the event parameters and stores them in local variables.
    /// </summary>
    /// <param name="data">The data associated with the event.</param>
    private void RetrieveData(object[] data)
    {
        _notificationType = ((Notification)data[0]).NotificationType;
        _notificationTitle = ((Notification)data[0]).NotificationTitle;
        _notificationMessage = ((Notification)data[0]).NotificationMessage;
        AcceptAction = ((Notification)data[0]).AcceptAction;
    }

    /// <summary>
    /// Handles the logic for readonly notifications.
    /// Shows or hides the buttons subtab based on the notification type.
    /// </summary>
    private void OnReadonlyNotification()
    {
        if(_notificationType != NotificationType.DisplayReadNotification)
        {
            return;
        }

        ToggleButtonsSubTab(true);
    }

    /// <summary>
    /// Opens the notification and plays a sound effect.
    /// </summary>
    /// <param name="notificationSoundIndex">The index of the sound effect to play.</param>
    private void Open()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        _denyButton.Deselect();
        _acceptButton.Deselect();
        _closeButton.Deselect();
    }

    /// <summary>
    /// Handles the action when the accept button is clicked.
    /// Raises the SellSpoiledItems event and plays a sound effect.
    /// </summary>
    private void OnAccept()
    {
        _data[0] = 40;
        GameEventHandler.RaiseEvent(GameEventType.SellSpoiledItems, _data);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        PlaySoundEffect(0, 11);
        RaiseHideNotificationEvent();
    }

    /// <summary>
    /// Handles the action when the deny button is clicked.
    /// Closes the notification and plays a sound effect.
    /// </summary>
    private void OnDeny()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        PlaySoundEffect(4, 10);
        RaiseHideNotificationEvent();
    }

    /// <summary>
    /// Closes the notification.
    /// </summary>
    private void Close()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        PlaySoundEffect(0, 5);
        RaiseHideNotificationEvent();
    }

    /// <summary>
    /// Sets the title text of the notification.
    /// </summary>
    private void SetTitle()
    {
        _titleText.text = _notificationTitle;
    }

    /// <summary>
    /// Updates the message text of the notification.
    /// </summary>
    private void UpdateMessage()
    {
        _messageText.text = _notificationMessage;
    }

    /// <summary>
    /// Toggles the visibility of buttons subtab based on the provided flag.
    /// </summary>
    /// <param name="setCloseButtonSubTabActive">True to show the close button, false to show the deny and accept buttons.</param>
    private void ToggleButtonsSubTab(bool setCloseButtonSubTabActive)
    {
        GlobalFunctions.CanvasGroupActivity(_denyAcceptButtonsCanvasGroup, !setCloseButtonSubTabActive);
        GlobalFunctions.CanvasGroupActivity(_closeButtonCanvasGroup, setCloseButtonSubTabActive);
    }

    private void RaiseHideNotificationEvent()
    {
        GameEventHandler.RaiseEvent(GameEventType.DisplayNextNotification);
    }

    /// <summary>
    /// Plays a sound effect specified by the list and clip index.
    /// </summary>
    /// <param name="listIndex">The index of the sound effect list.</param>
    /// <param name="clipIndex">The index of the sound effect clip.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}