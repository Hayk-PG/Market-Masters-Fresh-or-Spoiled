
public class MainTabManager : BaseMainHUDTab
{
    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        Open(gameEventType);
        Close(gameEventType);
    }

    private void Open(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.CloseErrorMessage)
        {
            return;
        }

        SetCanvasGroupActive(1f);
    }

    private void Close(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.DisplayErrorMessage)
        {
            return;
        }

        SetCanvasGroupActive(0.3f);
    }

    private void SetCanvasGroupActive(float alpha)
    {
        _canvasGroup.alpha = alpha;
        _canvasGroup.interactable = alpha > 0.5f;
        _canvasGroup.blocksRaycasts = _canvasGroup.interactable;
    }

    public override void Open()
    {
        
    }

    public override void Close()
    {

    }
}