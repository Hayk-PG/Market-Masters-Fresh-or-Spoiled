using UnityEngine;
using Pautik;

public class ShopTabButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Btn _toggleSellingBuyingTabButton;
    [SerializeField] private CanvasGroup _buttonCanvasGroup;

    [Header("Error Message")]
    [SerializeField] private ErrorMessageGroup _errorMessage;

    private const string _disableButtonAnimation = "DisableButton";
    private const string _enableButtonAnimation = "EnableButton";
    private bool _isSelected = false;




    private void OnEnable()
    {
        _toggleSellingBuyingTabButton.OnPointerDownHandler += OnPointerDownHandler;
        GameEventHandler.OnEvent += OnGameEvent;

    }

    private void OnPointerDownHandler()
    {
        if (_isSelected)
        {
            return;
        }

        bool canInteractWithButton = GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn != GameSceneReferences.Manager.TeamGroupPanels[0].TeamIndex 
                                  && GameSceneReferences.Manager.GameManager.IsGameStarted;

        if (!canInteractWithButton)
        {
            DisplayError();
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

        ToggleSelectionState();
        ToggleButtonCanvasGroup();
        PlaySoundEffect();
    }

    private void DisplayError()
    {
        _errorMessage.ErrorMessages[0] = GlobalFunctions.PartiallyTransparentText(_errorMessage.ErrorMessages[0]);
        _errorMessage.DisplayErrorMessage(0, 0);
    }

    private void ToggleSelectionState()
    {
        _isSelected = !_isSelected;      
    }

    private void ToggleButtonCanvasGroup()
    {
        PlayClickAnimation(_isSelected ? _disableButtonAnimation : _enableButtonAnimation);
    }

    private void PlaySoundEffect()
    {
        UISoundController.PlaySound(9, _isSelected ? 0 : 1);
    }

    private void PlayClickAnimation(string animationClipName)
    {
        _animator.Play(animationClipName, 0, 0);
    }
}