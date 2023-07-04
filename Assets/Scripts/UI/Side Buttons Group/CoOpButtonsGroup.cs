using UnityEngine;
using Pautik;

public class CoOpButtonsGroup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool LastActiveState { get; set; } = true;
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
    }

    private void HideCoopButtonsWhenPopupActive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.DisplayPopupNotification)
        {
            return;
        }

        IsActive = false;
    }

    private void ShowCoopButtonsWhenPopupInactive(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.OnPopupNotificationClosed)
        {
            return;
        }

        IsActive = LastActiveState;
    }

    private void SetLastActiveState(bool isActive)
    {
        LastActiveState = isActive;
    }
}