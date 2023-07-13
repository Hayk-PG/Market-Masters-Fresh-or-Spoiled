public class Notification 
{
    public NotificationType NotificationType { get; set; }
    public string NotificationTitle { get; set; }
    public string NotificationMessage { get; set; }    
    public UnityEngine.Sprite[] Images { get; set; }
    public System.Action AcceptAction { get; set; }
}