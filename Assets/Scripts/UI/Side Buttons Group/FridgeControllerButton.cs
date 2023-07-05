using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FridgeControllerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Coop Buttons Group")]
    [SerializeField] private CoOpButtonsGroup _coopButtonsGroup;

    [Header("Components")]
    [SerializeField] private Btn _button;

    [Header("UI Elements")]
    [SerializeField] private Btn_Icon _btnIcon;
    [SerializeField] private CanvasGroup _iconCanvasGroup;
    [SerializeField] private RectTransform _iconRectTransform;

    [Header("Sprites")]
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _highlightedSprite;

    private PlayerInventoryItemButton _inventoryItemButton;
    private List<StorageItem> _storageItemsList = new List<StorageItem>();
    private int _storageCapacity = 8;
    private bool _isPointerEntered;
    private bool _isTriggered;
    private object[] _storedItemsData = new object[1];
    private object[] _submittedStorageItemsData = new object[3];

    private bool IsPointerExited => !_isPointerEntered;
    private bool IsButtonsGroupHidden => !_coopButtonsGroup.IsActive;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _button.OnSelect += OnSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleInventoryItemDragNDropEvent(gameEventType, data);
        SubmitStorageItem(gameEventType, data);
    }

    private void OnSelect()
    {
        if (!_coopButtonsGroup.IsActive)
        {
            return;
        }

        _storedItemsData[0] = _storageItemsList;
        GameEventHandler.RaiseEvent(GameEventType.RequestStorageUIOpen, _storedItemsData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    private void HandleInventoryItemDragNDropEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.InventoryItemDragNDrop)
        {
            return;
        }

        if (IsPointerExited || IsButtonsGroupHidden)
        {
            SetIconAlpha(1f);
            HandleButtonInteraction(_defaultSprite, false);
            return;
        }

        if(IsDragRelease(isDragRelease: !(bool)data[0]))
        {
            SetIconAlpha(1f);
            HandleButtonInteraction(_defaultSprite, false);
            StoreItem();
            return;
        }

        GetInventoryItemButton((PlayerInventoryItemButton)data[1]);
        HandleButtonInteraction(_highlightedSprite, true, false);
        CalculateIconAlpha(mousePosition: (Vector2)data[2], out float alpha);
        SetIconAlpha(alpha);
    }

    private void SubmitStorageItem(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SendStorageItemForVerification)
        {
            return;
        }

        _submittedStorageItemsData[0] = (List<StorageItemButton>)data[0];
        _submittedStorageItemsData[1] = _storageItemsList;
        _submittedStorageItemsData[2] = (Sprite)data[1];      
        GameEventHandler.RaiseEvent(GameEventType.SubmitStorageItem, _submittedStorageItemsData);
    }

    private bool IsDragRelease(bool isDragRelease)
    {
        return isDragRelease;
    }

    private void HandleButtonInteraction(Sprite sprite, bool _isTriggered, bool deselect = true)
    {
        SetButtonSprites(sprite);

        if (deselect)
        {
            DeselectButton();
        }

        SetTriggeredState(_isTriggered);
    }

    private void GetInventoryItemButton(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _inventoryItemButton = playerInventoryItemButton;
    }

    private void StoreItem()
    {
        if(_inventoryItemButton == null || _inventoryItemButton.AssosiatedItem == null || _inventoryItemButton.ItemSpoilPercentage > 20 || _storageItemsList.Count >= _storageCapacity)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        _storageItemsList.Add(new StorageItem(_inventoryItemButton.AssosiatedItem, _inventoryItemButton.ItemLifetime));
        _inventoryItemButton.DestroySpoiledItemOnSeparateSale();
        PlaySoundEffect(7, 3);
    }

    private void SetButtonSprites(Sprite sprite)
    {
        if (!_isTriggered)
        {
            return;
        }

        _btnIcon.IconSpriteChangeDelegate(sprite);
        _btnIcon.ChangeReleasedSpriteDelegate();
    }

    private void DeselectButton()
    {
        if (!_isTriggered)
        {
            return;
        }

        _button.Deselect();
    }

    private void CalculateIconAlpha(Vector2 mousePosition, out float alpha)
    {
        float mouseDistance = Vector2.Distance(mousePosition, _iconRectTransform.position);
        alpha = Mathf.InverseLerp(0f, _iconRectTransform.sizeDelta.x, mouseDistance);
    }

    private void SetIconAlpha(float value)
    {
        if (!_isTriggered)
        {
            return;
        }

        _iconCanvasGroup.alpha = value;
    }

    private void SetTriggeredState(bool isTriggered)
    {
        if (_isTriggered == isTriggered)
        {
            return;
        }

        _isTriggered = isTriggered;
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}