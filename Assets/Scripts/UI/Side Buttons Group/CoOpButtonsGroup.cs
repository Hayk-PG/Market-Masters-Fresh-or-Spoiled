using UnityEngine;
using Pautik;

public class CoOpButtonsGroup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

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
        Hide(gameEventType);
        Unhide(gameEventType);
    }

    private void Hide(GameEventType gameEventType)
    {
        bool shouldHide = gameEventType == GameEventType.DisplayPopupNotification || gameEventType == GameEventType.OpenStorageUI;

        if (!shouldHide)
        {
            return;
        }

        IsActive = false;
    }

    private void Unhide(GameEventType gameEventType)
    {
        bool shouldUnhide = gameEventType == GameEventType.OnPopupNotificationClosed || gameEventType == GameEventType.CloseStorageUI;

        if (!shouldUnhide)
        {
            return;
        }

        IsActive = true;
    }
}