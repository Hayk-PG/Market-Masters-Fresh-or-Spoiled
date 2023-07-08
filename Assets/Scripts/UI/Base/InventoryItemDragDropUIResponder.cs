using UnityEngine;
using UnityEngine.EventSystems;
using Pautik;

public class InventoryItemDragDropUIResponder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Elements")]
    [SerializeField] protected Btn _button;
    [SerializeField] protected Btn_Icon _icon;
    [SerializeField] protected CanvasGroup _canvasGroup1;
    [SerializeField] protected CanvasGroup _canvasGroup2;

    [Header("Sprites")]
    [SerializeField] protected Sprite[] _iconSprites;

    protected PlayerInventoryItemButton _inventoryItemButton;
    protected bool _isPointerEntered;
    protected bool _isTriggered;

    protected virtual bool IsPointerExited => !_isPointerEntered;
    protected virtual bool IsButtonsGroupHidden => !GameSceneReferences.Manager.CoOpButtonsGroup.IsActive;





    protected virtual void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected virtual void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleInventoryItemDragNDropEvent(gameEventType, data);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    protected virtual void HandleInventoryItemDragNDropEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.InventoryItemDragNDrop)
        {
            return;
        }

        if (IsPointerExited || IsButtonsGroupHidden)
        {
            ExecuteOnPointerExited(data);           
            return;
        }

        if(IsDragReleased(isDragReleased: !(bool)data[0]))
        {
            ExecuteOnDragRelease(data);
            return;
        }

        ExecuteOnHover(data);
    }

    protected virtual void ExecuteOnPointerExited(object[] data)
    {
        HandleButtonInteraction(false);
        ToggleUIElementsVisibility(true);
    }

    protected virtual void ExecuteOnDragRelease(object[] data)
    {
        HandleButtonInteraction(false);
        ToggleUIElementsVisibility(true);
    }

    protected virtual void ExecuteOnHover(object[] data)
    {
        GetInventoryItemButton((PlayerInventoryItemButton)data[1]);
        HandleButtonInteraction(true, false);
        ToggleUIElementsVisibility(false);
    }

    protected virtual void ToggleUIElementsVisibility(bool isCanvasGroup1Active)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup1, isCanvasGroup1Active);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup2, !isCanvasGroup1Active);
    }

    protected virtual void ToggleItemStoringDisplay(Sprite sprite)
    {
        _icon.IconSpriteChangeDelegate(sprite);
        _icon.ChangeReleasedSpriteDelegate();
    }

    protected virtual bool IsDragReleased(bool isDragReleased)
    {
        return isDragReleased;
    }

    protected virtual void SetTriggeredState(bool isTriggered)
    {
        if (_isTriggered == isTriggered)
        {
            return;
        }

        _isTriggered = isTriggered;
    }

    protected virtual void GetInventoryItemButton(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _inventoryItemButton = playerInventoryItemButton;
    }

    protected virtual void HandleButtonInteraction(bool _isTriggered, bool deselect = true)
    {
        if (deselect)
        {
            DeselectButton();
        }

        SetTriggeredState(_isTriggered);
    }

    protected virtual void DeselectButton()
    {
        if (!_isTriggered)
        {
            return;
        }

        _button.Deselect();
    }
}