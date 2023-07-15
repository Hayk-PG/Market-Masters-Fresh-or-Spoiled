using UnityEngine;

public class TitleScreenManager : BaseMainHUDTab
{
    [Header("UI Elemenets")]
    [SerializeField] private Btn _offlineGameButton;
    [SerializeField] private Btn _onlineGameButton;




    private void OnEnable()
    {
        _offlineGameButton.OnSelect += OnOfflineGameSelect;
        _onlineGameButton.OnSelect += OnOnlineGameSelect;
    }

    private void OnOfflineGameSelect()
    {
        GameEventHandler.RaiseEvent(GameEventType.SelectOfflineGame);
        SetButtonInteractability(false);
    }

    private void OnOnlineGameSelect()
    {
        GameEventHandler.RaiseEvent(GameEventType.SelectOnlineGame);
        SetButtonInteractability(false);
    }

    private void SetButtonInteractability(bool isInteractable)
    {
        _offlineGameButton.IsInteractable = isInteractable;
        _onlineGameButton.IsInteractable = isInteractable;
    }

    public override void Open()
    {
        
    }

    public override void Close()
    {
        
    }
}