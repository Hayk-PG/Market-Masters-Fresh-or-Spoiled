using Pautik;

public class ItemShopTab : BaseMainHUDTab
{
    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnGameTurnUpdate(gameEventType);
        OnSellingBuyingTabActivity(gameEventType);
    }

    private void OnGameTurnUpdate(GameEventType gameEventType)
    {
        bool canTriggerEvent = gameEventType == GameEventType.UpdateGameTurn && IsActive;

        if (!canTriggerEvent)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.SellingBuyingTabActivity);
    }

    private void OnSellingBuyingTabActivity(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
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