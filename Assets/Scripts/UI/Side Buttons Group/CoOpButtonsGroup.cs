using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class CoOpButtonsGroup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool LastActiveState { get; set; } = true;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEven;
    }

    private void OnGameEven(GameEventType gameEventType, object[] data)
    {
        HideCoopButtonsWhenPopupActive(gameEventType);
        ShowCoopButtonsWhenPopupInactive(gameEventType);
    }

    private void HideCoopButtonsWhenPopupActive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.DisplayPopupNotification)
        {
            return;
        }
       
        SetCanvasGroupActive(false);
    }

    private void ShowCoopButtonsWhenPopupInactive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.OnPopupNotificationClosed)
        {
            return;
        }

        SetCanvasGroupActive(LastActiveState);
    }

    private void SetCanvasGroupActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);       
    }

    private void SetLastActiveState(bool isActive)
    {
        LastActiveState = isActive;
    }
}