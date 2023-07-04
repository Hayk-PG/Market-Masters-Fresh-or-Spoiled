using UnityEngine;
using Pautik;

public class StorageUIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private StorageItemButton[] _itemButtons;
    [SerializeField] private Btn[] _commandButtons;

    [Header("Sprites")]
    [SerializeField] private Sprite _emptyCellSprite;
    [SerializeField] private Sprite _blockedCellSprite;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleOpenStorageUIEvent(gameEventType, data);
        HandleCloseStorageUIEvents(gameEventType);
    }

    private void HandleOpenStorageUIEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.OpenStorageUI)
        {
            return;
        }

        SetCanvasGroupActivity(true);
    }

    private void HandleCloseStorageUIEvents(GameEventType gameEventType)
    {
        bool isClosing = gameEventType == GameEventType.CloseStorageUI || gameEventType == GameEventType.DisplayPopupNotification;

        if (!isClosing)
        {
            return;
        }

        SetCanvasGroupActivity(false);
    }

    private void SetCanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}