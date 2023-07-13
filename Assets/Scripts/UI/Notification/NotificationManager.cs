using UnityEngine;
using TMPro;
using Pautik;

public class NotificationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _denyAcceptButtonsCanvasGroup;
    [SerializeField] private CanvasGroup _closeButtonCanvasGroup;
    [SerializeField] private CanvasGroup _messageSubTabCanvasGroup;
    [SerializeField] private CanvasGroup _messageWithImagesSubTabCanvasGroup;

    [Header("Buttons")]
    [SerializeField] private Btn _denyButton;
    [SerializeField] private Btn _acceptButton;
    [SerializeField] private Btn _closeButton;
    [SerializeField] private Btn _backButton;
    [SerializeField] private Btn _forwardButton;

    [Header("TMP Texts")]
    [SerializeField] private TMP_Text _notificationPageText;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private TMP_Text _messageWithImagesText;

    [Header("Notification Images Group")]
    [SerializeField] private NotificationImagesGroupManager _notificationImageGroupManager;

    private Notification _notification;
    private NotificationType _notificationType;
    private string _notificationTitle;
    private string _notificationMessage;
    private object[] _data = new object[1];
    private object[] _nextNotificationData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _denyButton.OnSelect += OnDeny;
        _closeButton.OnSelect += Close;
        _acceptButton.OnSelect += OnAccept;
        _backButton.OnPointerUpHandler += ()=> DisplayNotificationAtPosition(-1);
        _forwardButton.OnPointerUpHandler += () => DisplayNotificationAtPosition(1);
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

        bool onlyUpdateNotification = (bool)data[0];

        if (onlyUpdateNotification)
        {
            UpdatePageText(text: (string)data[2]);
            return;
        }

        RetrieveNotification((Notification)data[1]);
        RetrieveNotificationType(_notification.NotificationType);
        RetrieveTexts(_notification.NotificationTitle, _notification.NotificationMessage);
        UpdatePageText(text: (string)data[2]);
        SetTitle();
        UpdateMessage();
        Open();
        OnReadonlyNotification();
        OnReadonlyNotificationWithImages();
    }

    private void RetrieveNotification(Notification notification)
    {
        _notification = notification;
    }

    private void RetrieveNotificationType(NotificationType notificationType)
    {
        _notificationType = notificationType;
    }

    private void RetrieveTexts(string title, string message)
    {
        _notificationTitle = title;
        _notificationMessage = message;
    }

    private void OnReadonlyNotification()
    {
        if (_notificationType != NotificationType.DisplayReadNotification)
        {
            return;
        }

        ToggleButtonsSubTab(true);
        ToggleMessageSubTabs(false);
    }

    private void OnReadonlyNotificationWithImages()
    {
        if (_notificationType != NotificationType.DisplayReadNotificationWithImages)
        {
            return;
        }

        _messageWithImagesText.text = _notification.NotificationMessage;
        _notificationImageGroupManager.Setup(_notification.Images);
        ToggleButtonsSubTab(true);
        ToggleMessageSubTabs(true);
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
    }

    /// <summary>
    /// Handles the action when the deny button is clicked.
    /// Closes the notification and plays a sound effect.
    /// </summary>
    private void OnDeny()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        PlaySoundEffect(4, 10);
    }

    /// <summary>
    /// Closes the notification.
    /// </summary>
    private void Close()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        PlaySoundEffect(0, 5);
    }

    private void DisplayNotificationAtPosition(int notificationIndexPointer)
    {
        _nextNotificationData[0] = notificationIndexPointer;
        GameEventHandler.RaiseEvent(GameEventType.DisplayNextNotification, _nextNotificationData);
    }

    private void UpdatePageText(string text)
    {
        _notificationPageText.text = text;
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

    private void ToggleMessageSubTabs(bool isMessageWithGridSubTabActive)
    {
        GlobalFunctions.CanvasGroupActivity(_messageSubTabCanvasGroup, !isMessageWithGridSubTabActive);
        GlobalFunctions.CanvasGroupActivity(_messageWithImagesSubTabCanvasGroup, isMessageWithGridSubTabActive);
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