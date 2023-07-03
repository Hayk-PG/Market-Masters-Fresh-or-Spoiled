
[System.Serializable]
public struct Notification 
{
    public NotificationType NotificationType { get; private set; }
    public string NotificationTitle { get; private set; }
    public string NotificationMessage { get; private set; }
    public System.Action AcceptAction { get; private set; }




    public Notification(NotificationType notificationType, string notificationTitle, string notificationMessage, System.Action acceptAction)
    {
        NotificationType = notificationType;
        NotificationTitle = notificationTitle;
        NotificationMessage = notificationMessage;
        AcceptAction = acceptAction;
    }
}