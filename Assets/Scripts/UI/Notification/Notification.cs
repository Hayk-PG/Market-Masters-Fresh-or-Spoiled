public class Notification 
{
    public NotificationType NotificationType { get; set; }
    public string NotificationTitle { get; set; }
    public string NotificationMessage { get; set; }    
    public UnityEngine.Sprite[] Images { get; set; }
    public System.Action OnAcceptCallback { get; set; }




    public Notification()
    {

    }

    public Notification(NotificationType notificationType, string title, string message)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
    }

    public Notification(NotificationType notificationType, string title, string message, UnityEngine.Sprite[] images)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
        Images = images;
    }

    public Notification(NotificationType notificationType, string title, string message, System.Action OnAcceptCallback)
    {
        NotificationType = notificationType;
        NotificationTitle = title;
        NotificationMessage = message;
        this.OnAcceptCallback = OnAcceptCallback;
    }
}