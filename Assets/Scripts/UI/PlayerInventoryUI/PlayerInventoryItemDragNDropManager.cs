using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventoryItemDragNDropManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private PlayerInventoryItemButton _playerInventoryItemButton;

    private Vector2 _dragStartMousePosition;
    private object[] _dragNDropData = new object[3];

    private bool _isPointerEntered;
    private bool _canStartDragging;
    private bool _isDragStarted;
    private bool _isDragging;

    private bool HasItem => _playerInventoryItemButton.AssosiatedItem != null;
    private bool IsMouseButtonHeld => Input.GetMouseButton(0);
    private bool IsDragRelease => _isDragging && !IsMouseButtonHeld;
    private bool IsIdle => !_isPointerEntered && !_isDragging;
    private bool IsClickStarted => !IsMouseButtonHeld && !_isDragStarted;
    private bool IsDraggable => IsMouseButtonHeld && _canStartDragging && !_isDragStarted;
    private bool IsDragThresholdExceeded => _isDragStarted && Vector2.Distance(Input.mousePosition, _dragStartMousePosition) > 50 && !_isDragging;
    private bool IsDraggingAndMouseButtonHeld => _isDragging && IsMouseButtonHeld;




    private void Update()
    {
        if(!HasItem)
        {
            return;
        }

        if (IsDragRelease)
        {
            SetDraggingState(false);
            RaiseInventoryItemDragNDropData(false);
            return;
        }

        if (IsIdle)
        {
            SetCanStartDraggingState(false);
            SetDragStartedState(false);
            SetDraggingState(false);
            return;
        }

        if (IsClickStarted)
        {
            SetCanStartDraggingState(true);
            return;
        }

        if (IsDraggable)
        {
            SetDragStartPosition();
            SetDragStartedState(true);
        }

        if (IsDragThresholdExceeded)
        {
            SetDraggingState(true);
        }

        if (IsDraggingAndMouseButtonHeld)
        {
            RaiseInventoryItemDragNDropData(true, _playerInventoryItemButton, Input.mousePosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    private void SetDraggingState(bool isDragging)
    {
        _isDragging = isDragging;
    }

    private void SetCanStartDraggingState(bool canStartDragging)
    {
        _canStartDragging = canStartDragging;
    }

    private void SetDragStartedState(bool isDragStarted)
    {
        _isDragStarted = isDragStarted;
    }

    private void SetDragStartPosition()
    {
        _dragStartMousePosition = Input.mousePosition;
    }

    private void RaiseInventoryItemDragNDropData(bool isDragging, PlayerInventoryItemButton playerInventoryItemButton = null, Vector2 mousePosition = default)
    {
        _dragNDropData[0] = isDragging;
        _dragNDropData[1] = playerInventoryItemButton;
        _dragNDropData[2] = mousePosition;
        GameEventHandler.RaiseEvent(GameEventType.InventoryItemDragNDrop, _dragNDropData);
    }
}