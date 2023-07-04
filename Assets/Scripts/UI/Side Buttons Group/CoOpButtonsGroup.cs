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
        HideCoopButtonsWhenStorageActive(gameEventType, data);
    }

    private void HideCoopButtonsWhenPopupActive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.DisplayPopupNotification)
        {
            return;
        }

        EnqueueOpenStatus(_canvasGroup.interactable);
        IsActive = false;        
    }

    private void ShowCoopButtonsWhenPopupInactive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.OnPopupNotificationClosed)
        {
            return;
        }  
    }

    private void HideCoopButtonsWhenStorageActive(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.OpenStorageUI)
        {
            return;
        }

        bool isStorageActive = (bool)data[0];

        if (!isStorageActive)
        {
            IsActive = false;
        }
        else
        {
            IsActive = true;
        }
    }

    private void CloseStorageUI()
    {

    }

    private void EnqueueOpenStatus(bool isActive)
    {
        _openStatusQueue.Enqueue(isActive);
    }

    private void DequeueOpenStatus()
    {
        _openStatusQueue.Dequeue();
    }
}