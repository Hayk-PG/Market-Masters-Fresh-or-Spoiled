using UnityEngine;
using UnityEngine.UI;

public class SellingBuyingTabButtonController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SideButtonsPopUpEffectController _sideButtonsEffectController;

    [Header("UI Elements")]
    [SerializeField] private Btn _toggleSellingBuyingTabButton;
    [SerializeField] private BtnTxt _text;
    [SerializeField] private Image _leftArrowIcon;
    [SerializeField] private Image _rightArrowIcon;

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

        PlaySoundEffect();
        PlayClickAnimation();
        GameEventHandler.RaiseEvent(GameEventType.SellingBuyingTabActivity);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        UpdateText();
        SwitchArrowIcons();
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

    private void SwitchArrowIcons()
    {
        switch (_isTextSwitched)
        {
            case true:
                _leftArrowIcon.gameObject.SetActive(false);
                _rightArrowIcon.gameObject.SetActive(true);
                break;

            case false:
                _leftArrowIcon.gameObject.SetActive(true);
                _rightArrowIcon.gameObject.SetActive(false);
                break;
        }
    }

    private void PlaySoundEffect()
    {
        UISoundController.PlaySound(0, _isTextSwitched ? 4 : 5);
        UISoundController.PlaySound(6, 0);
    }

    private void PlayClickAnimation()
    {
        _sideButtonsEffectController.PlayAnimation(0);
    }
}