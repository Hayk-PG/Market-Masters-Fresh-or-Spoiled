using UnityEngine;
using UnityEngine.UI;
using Pautik;

public class PlayerInventoryDragNDropManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private PlayerInventoryItemButton[] _inventoryItemButtons;
    [SerializeField] private Image _dragItemIcon;
    [SerializeField] private CanvasGroup _dragItemIconCanvasGroup;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.InventoryItemDragNDrop)
        {
            return;
        }

        bool isDragging = (bool)data[0];

        if (!isDragging)
        {
            SetDragItemIconActive(false);
            return;
        }

        SetDragItemIconActive(true);
        SetDragItemIcon(icon: ((PlayerInventoryItemButton)data[1]).AssosiatedItem.Icon);
        SetDragItemIconPosition(position: (Vector2)data[2]);
    }

    private void SetDragItemIconActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_dragItemIconCanvasGroup, isActive);
    }

    private void SetDragItemIcon(Sprite icon)
    {
        _dragItemIcon.sprite = icon;
    }

    private void SetDragItemIconPosition(Vector2 position)
    {
        _dragItemIcon.transform.position = position;
    }
}