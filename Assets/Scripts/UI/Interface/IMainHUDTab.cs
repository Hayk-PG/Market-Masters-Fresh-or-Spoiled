/// <summary>
/// Interface for a main HUD tab.
/// </summary>
public interface IMainHUDTab 
{
    /// <summary>
    /// Opens the tab.
    /// </summary>
    void Open();

    /// <summary>
    /// Closes the tab.
    /// </summary>
    void Close();

    /// <summary>
    /// Sets the activity state of the canvas group.
    /// </summary>
    /// <param name="isActive">The desired activity state. Default is <c>true</c>.</param>
    void SetCanvasGroupActive(bool isActive = true);
}