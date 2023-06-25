using UnityEngine;

public class SellingBuyingTabButtonController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Btn _toggleSellingBuyingTabButton;
    [SerializeField] private BtnTxt _text;

    private const string _sellTab = "sell";
    private const string _buyTab = "buy";

    private bool _isTextSwitched = false;




    private void OnEnable()
    {
        _toggleSellingBuyingTabButton.OnPointerDownHandler += OnPointerDownHandler;
        GameEventHandler.OnEvent += OnGameEvent;

    }

    private void OnPointerDownHandler()
    {
        bool canInteractWithButton = GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn != GameSceneReferences.Manager.TeamGroupPanels[0].TeamIndex 
                                  && GameSceneReferences.Manager.GameManager.IsGameStarted;

        if (!canInteractWithButton)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.SellingBuyingTabActivity);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        _isTextSwitched = !_isTextSwitched;

        switch (_isTextSwitched)
        {
            case true: _text.SetButtonTitle(_sellTab);break;
            case false: _text.SetButtonTitle(_buyTab); break;
        }
    }
}