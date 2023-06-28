using Pautik;

public class MainGameTab : BaseMainHUDTab
{
    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnSellingBuyingTabActivity(gameEventType);
    }

    private void OnSellingBuyingTabActivity(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, !IsActive);
    }

    public override void Open()
    {
        
    }

    public override void Close()
    {
        
    }
}