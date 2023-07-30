/// <summary>
/// Represents a notification with various properties and callbacks.
/// </summary>
public class Notification 
{
    /// <summary>
    /// The type of notification.
    /// </summary>
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// The title of the notification.
    /// </summary>
    public string NotificationTitle { get; set; }

    /// <summary>
    /// The main message of the notification.
    /// </summary>
    public string NotificationMessage { get; set; }

    /// <summary>
    /// An array of images associated with the notification.
    /// </summary>
    public UnityEngine.Sprite[] Images { get; set; }

    /// <summary>
    /// Callback method invoked when the notification is accepted or closed.
    /// </summary>
    public System.Action OnAcceptCallback { get; set; }




    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    public Notification()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with the specified notification type, title, and message.
    /// </summary>
    /// <param name="notificationType">The type of notification.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The main message of the notification.</param>
    public Notification(NotificationType notificationType, string title, string message)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with the specified notification type, title, message, and images.
    /// </summary>
    /// <param name="notificationType">The type of notification.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The main message of the notification.</param>
    /// <param name="images">An array of images associated with the notification.</param>
    public Notification(NotificationType notificationType, string title, string message, UnityEngine.Sprite[] images)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
        Images = images;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with the specified notification type, title, message, and callback.
    /// </summary>
    /// <param name="notificationType">The type of notification.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The main message of the notification.</param>
    /// <param name="onAcceptCallback">Callback method invoked when the notification is accepted or closed.</param>
    public Notification(NotificationType notificationType, string title, string message, System.Action OnAcceptCallback)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
        this.OnAcceptCallback = OnAcceptCallback;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with the specified notification type, title, message, images, and callback.
    /// </summary>
    /// <param name="notificationType">The type of notification.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The main message of the notification.</param>
    /// <param name="images">An array of images associated with the notification.</param>
    /// <param name="onAcceptCallback">Callback method invoked when the notification is accepted or closed.</param>
    public Notification(NotificationType notificationType, string title, string message, UnityEngine.Sprite[] images, System.Action OnAcceptCallback)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
        Images = images;
        this.OnAcceptCallback = OnAcceptCallback;
    }
}