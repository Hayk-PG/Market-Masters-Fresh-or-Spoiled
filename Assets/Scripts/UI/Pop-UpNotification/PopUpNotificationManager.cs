using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpNotificationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Btn _button;

    private string _message;
    private const string _popUpNotificationDisplayAnim = "Pop-UpNotificationAnim";
    private const string _closePopUpNotificationAnim = "ClosePop-UpNotificationAnim";




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _button.OnSelect += OnButtonSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        DisplayPopupNotification(gameEventType, data);
    }

    private void OnButtonSelect()
    {
        ClosePopupNotification();
    }

    private void DisplayPopupNotification(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.DisplayPopupNotification)
        {
            return;
        }

        RetrieveData(data);
        UpdateMessageText();
        PlayAnimation(_popUpNotificationDisplayAnim);
        DeselectButton();
        PlaySoundEffect(1, 1);
    }

    private void ClosePopupNotification()
    {
        PlayAnimation(_closePopUpNotificationAnim);
        PlaySoundEffect(0, 3);
        GameEventHandler.RaiseEvent(GameEventType.OnPopupNotificationClosed);
    }

    private void RetrieveData(object[] data)
    {
        _message = (string)data[0];
    }

    private void UpdateMessageText()
    {
        _messageText.text = _message;
    }

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName, 0, 0);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }

    private void DeselectButton()
    {
        _button.Deselect();
    }
}
