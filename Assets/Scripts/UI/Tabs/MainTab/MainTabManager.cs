
public class MainTabManager : BaseMainHUDTab
{
    private bool IsGameEnded => GameSceneReferences.Manager.GameManager.IsGameEnded;



    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        Open(gameEventType);
        Close(gameEventType);
        CloseOnGameEnd(gameEventType);
    }

    private void Open(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.CloseErrorMessage)
        {
            return;
        }

        if(IsGameEnded)
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

        if (IsGameEnded)
        {
            return;
        }

        SetCanvasGroupActive(0.3f);
    }

    private void CloseOnGameEnd(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.EndGame)
        {
            return;
        }

        SetCanvasGroupActive(0.1f);
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