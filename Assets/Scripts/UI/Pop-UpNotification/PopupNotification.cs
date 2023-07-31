/// <summary>
/// Represents a popup notification with a message that can be displayed.
/// </summary>
public class PopupNotification 
{
    private object[] _data = new object[1];




    /// <summary>
    /// Initializes a new instance of the PopupNotification class with the given message.
    /// </summary>
    /// <param name="message">The message to be displayed in the popup notification.</param>
    public PopupNotification(string message)
    {
        _data[0] = message;
        GameEventHandler.RaiseEvent(GameEventType.DisplayPopupNotification, _data);
    }
}