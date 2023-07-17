using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErrorMessageManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Btn _closeButton;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _errorMessageText;

    private const string _popUpAnimation = "ErrorPopUpAnim";
    private const string _closeAnimation = "ErrorClose";
    private string _errorMessage;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _closeButton.OnSelect += OnCloseButtonSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        DisplayErrorMessage(gameEventType, data);
        DisableOnGameEnd(gameEventType);
    }

    private void OnCloseButtonSelect()
    {
        GameEventHandler.RaiseEvent(GameEventType.CloseErrorMessage);
        PlayAnimation(_closeAnimation);
        PlaySoundEffect(6, 1);
    }

    private void DisplayErrorMessage(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.DisplayErrorMessage)
        {
            return;
        }

        RetrieveData(data);
        PrintMessage();
        PlayAnimation(_popUpAnimation);
        PlaySoundEffect(7, 1);
    }

    private void DisableOnGameEnd(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.EndGame)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private void RetrieveData(object[] data)
    {
        _errorMessage = (string)data[0];
        _icon.sprite = (Sprite)data[1];
    }

    private void PrintMessage()
    {
        _errorMessageText.text = _errorMessage;
    }

    private void PlayAnimation(string aniamtionName)
    {
        _animator.Play(aniamtionName, 0, 0);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}