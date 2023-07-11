
[System.Serializable]
public class Notification 
{
    public NotificationType NotificationType { get; private set; }
    public string NotificationTitle { get; private set; }
    public string NotificationMessage { get; private set; }    
    public UnityEngine.Sprite[] Images { get; private set; }
    public System.Action AcceptAction { get; private set; }




    public Notification(NotificationType notificationType, string notificationTitle, string notificationMessage)
    {
        NotificationType = notificationType;
        NotificationTitle = notificationTitle;
        NotificationMessage = notificationMessage;
    }

    public Notification(NotificationType notificationType, string notificationTitle, string notificationMessage, UnityEngine.Sprite[] images)
    {
        NotificationType = notificationType;
        NotificationTitle = notificationTitle;
        NotificationMessage = notificationMessage;
        Images = images;
    }
}