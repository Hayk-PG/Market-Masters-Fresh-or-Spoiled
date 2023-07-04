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
        if(gameEventType != GameEventType.OpenStorageUI)
        {
            return;
        }

        bool isActive = (bool)data[0];

        if (isActive)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    private void Close()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }
}