using UnityEngine;
using UnityEngine.EventSystems;
using Pautik;

public class InventoryItemDragDropUIResponder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Elements")]
    [SerializeField] protected Btn _button;
    [SerializeField] protected Btn_Icon _buttonIcon;
    [SerializeField] protected RectTransform _buttonRectTransform;
    [SerializeField] protected CanvasGroup[] _canvasGroups;

    [Header("Sprites")]
    [SerializeField] protected Sprite _defaultSprite;
    [SerializeField] protected Sprite _highlightedSprite;

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
        SetUIElementAlpha(1f);
        HandleButtonInteraction(_defaultSprite, false);
    }

    protected virtual void ExecuteOnDragRelease(object[] data)
    {
        SetUIElementAlpha(1f);
        HandleButtonInteraction(_defaultSprite, false);
    }

    protected virtual void ExecuteOnHover(object[] data)
    {
        GetInventoryItemButton((PlayerInventoryItemButton)data[1]);
        HandleButtonInteraction(_highlightedSprite, true, false);
        CalculateIconAlpha(mousePosition: (Vector2)data[2], out float alpha);
        SetUIElementAlpha(alpha);
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

    protected virtual void CalculateIconAlpha(Vector2 mousePosition, out float alpha)
    {
        float mouseDistance = Vector2.Distance(mousePosition, _buttonRectTransform.position);
        alpha = Mathf.InverseLerp(0f, _buttonRectTransform.sizeDelta.x, mouseDistance);
    }

    protected virtual void SetUIElementAlpha(float alpha)
    {
        if (!_isTriggered)
        {
            return;
        }

        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups, canvasGroup => canvasGroup.alpha = alpha);
    }

    protected virtual void HandleButtonInteraction(Sprite sprite, bool _isTriggered, bool deselect = true)
    {
        SetButtonSprites(sprite);

        if (deselect)
        {
            DeselectButton();
        }

        SetTriggeredState(_isTriggered);
    }

    protected virtual void SetButtonSprites(Sprite sprite)
    {
        if (!_isTriggered)
        {
            return;
        }

        _buttonIcon.IconSpriteChangeDelegate(sprite);
        _buttonIcon.ChangeReleasedSpriteDelegate();
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