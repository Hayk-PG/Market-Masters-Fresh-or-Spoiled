using UnityEngine;
using Pautik;
using System.Collections.Generic;

public class CoOpButtonsGroup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private Queue<bool> _openStatusQueue = new Queue<bool>();

    public bool IsActive
    {
        get => _canvasGroup.interactable;
        private set => GlobalFunctions.CanvasGroupActivity(_canvasGroup, value);
    }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEven;
    }

    private void OnGameEven(GameEventType gameEventType, object[] data)
    {
        HideCoopButtonsWhenPopupActive(gameEventType);
        ShowCoopButtonsWhenPopupInactive(gameEventType);
        HideCoopButtonsWhenStorageActive(gameEventType);
        ShowCoopButtonsWhenStoragetInactive(gameEventType);
    }

    private void HideCoopButtonsWhenPopupActive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.DisplayPopupNotification)
        {
            return;
        }

        CloseCoopButtonsGroup();
    }

    private void ShowCoopButtonsWhenPopupInactive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.OnPopupNotificationClosed)
        {
            return;
        }

        TryOpenCoopButonsGroup();
    }

    private void HideCoopButtonsWhenStorageActive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.OpenStorageUI)
        {
            return;
        }

        CloseCoopButtonsGroup();
    }

    private void ShowCoopButtonsWhenStoragetInactive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.CloseStorageUI)
        {
            return;
        }

        TryOpenCoopButonsGroup();
    }

    private void CloseCoopButtonsGroup()
    {
        _openStatusQueue.Enqueue(_canvasGroup.interactable);
        IsActive = false;
    }

    private void TryOpenCoopButonsGroup()
    {
        IsActive = _openStatusQueue.Count < 1 ? true : _openStatusQueue.Dequeue();
    }
}