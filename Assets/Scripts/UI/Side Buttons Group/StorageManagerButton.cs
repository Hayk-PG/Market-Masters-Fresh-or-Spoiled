using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageManagerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    /// <summary>
    /// Handles game events and performs appropriate actions based on the event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        HandleInventoryItemDragNDropEvent(gameEventType, data);
        SubmitStorageItem(gameEventType, data);
    }

    /// <summary>
    /// Handles the button selection event.
    /// </summary>
    private void OnSelect()
    {
        if (!_coopButtonsGroup.IsActive)
        {
            return;
        }

        _storedItemsData[0] = _storageItemsList;
        GameEventHandler.RaiseEvent(GameEventType.RequestStorageUIOpen, _storedItemsData);
    }

    /// <summary>
    /// Handles the pointer enter event.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    /// <summary>
    /// Handles the pointer exit event.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    /// <summary>
    /// Handles the inventory item drag-and-drop event and performs appropriate actions.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
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

    /// <summary>
    /// Submits the storage item for verification when the appropriate game event is triggered.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
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

    /// <summary>
    /// Checks if the drag operation has been released.
    /// </summary>
    /// <param name="isDragRelease">A boolean indicating if the drag operation has been released.</param>
    /// <returns>Returns true if the drag operation has been released; otherwise, false.</returns>
    private bool IsDragRelease(bool isDragRelease)
    {
        return isDragRelease;
    }

    /// <summary>
    /// Handles the interaction of the storage manager button, including button sprites, selection state, and inventory item button retrieval.
    /// </summary>
    /// <param name="sprite">The sprite to set for the button.</param>
    /// <param name="_isTriggered">The triggered state of the button.</param>
    /// <param name="deselect">Whether to deselect the button.</param>
    private void HandleButtonInteraction(Sprite sprite, bool _isTriggered, bool deselect = true)
    {
        SetButtonSprites(sprite);

        if (deselect)
        {
            DeselectButton();
        }

        SetTriggeredState(_isTriggered);
    }

    /// <summary>
    /// Retrieves the inventory item button associated with the storage manager button.
    /// </summary>
    /// <param name="playerInventoryItemButton">The inventory item button to retrieve.</param>
    private void GetInventoryItemButton(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _inventoryItemButton = playerInventoryItemButton;
    }

    /// <summary>
    /// Stores the inventory item in the storage based on the current conditions.
    /// </summary>
    private void StoreItem()
    {
        if(_inventoryItemButton == null || _inventoryItemButton.AssociatedItem == null || _inventoryItemButton.ItemSpoilPercentage > 20 || _storageItemsList.Count >= _storageCapacity)
        {
            PlaySoundEffect(4, 1);
            return;
        }

        int currentTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount;
        int itemSavedLifetime = _inventoryItemButton.ItemLifetime;
        _storageItemsList.Add(new StorageItem(_inventoryItemButton.AssociatedItem, currentTurnCount, itemSavedLifetime));
        _inventoryItemButton.DestroySpoiledItemOnSeparateSale();
        PlaySoundEffect(7, 3);
    }

    /// <summary>
    /// Sets the sprites for the button based on the triggered state.
    /// </summary>
    /// <param name="sprite">The sprite to set for the button.</param>
    private void SetButtonSprites(Sprite sprite)
    {
        if (!_isTriggered)
        {
            return;
        }

        _btnIcon.IconSpriteChangeDelegate(sprite);
        _btnIcon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Deselects the button if it is triggered.
    /// </summary>
    private void DeselectButton()
    {
        if (!_isTriggered)
        {
            return;
        }

        _button.Deselect();
    }

    /// <summary>
    /// Calculates the alpha value for the icon based on the mouse position.
    /// </summary>
    /// <param name="mousePosition">The current mouse position.</param>
    /// <param name="alpha">The calculated alpha value.</param>
    private void CalculateIconAlpha(Vector2 mousePosition, out float alpha)
    {
        float mouseDistance = Vector2.Distance(mousePosition, _iconRectTransform.position);
        alpha = Mathf.InverseLerp(0f, _iconRectTransform.sizeDelta.x, mouseDistance);
    }

    /// <summary>
    /// Sets the alpha value for the icon.
    /// </summary>
    /// <param name="value">The alpha value to set.</param>
    private void SetIconAlpha(float value)
    {
        if (!_isTriggered)
        {
            return;
        }

        _iconCanvasGroup.alpha = value;
    }

    /// <summary>
    /// Sets the triggered state of the button.
    /// </summary>
    /// <param name="isTriggered">The triggered state to set.</param>
    private void SetTriggeredState(bool isTriggered)
    {
        if (_isTriggered == isTriggered)
        {
            return;
        }

        _isTriggered = isTriggered;
    }

    /// <summary>
    /// Plays a sound effect based on the provided list index and clip index.
    /// </summary>
    /// <param name="listIndex">The index of the sound effect list.</param>
    /// <param name="clipIndex">The index of the sound effect clip.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}