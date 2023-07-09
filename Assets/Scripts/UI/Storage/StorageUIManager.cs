using UnityEngine;
using Pautik;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class StorageUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private StorageItemButton[] _itemButtons;
    [SerializeField] private Btn[] _commandButtons;

    [Header("Sprites")]
    [SerializeField] private Sprite _emptyCellSprite;
    [SerializeField] private Sprite _blockedCellSprite;

    private bool _isPointerEntered;
    private object[] selectedStorageItemButtonData = new object[2];
    private List<StorageItem> _tempStorageItemsList;
    private List<StorageItemButton> _selectedStorageItemsList = new List<StorageItemButton>(); 




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _commandButtons[0].OnSelect += SelectCloseButton;
        _commandButtons[1].OnSelect += SelectSendButton;
    }

    /// <summary>
    /// Event callback for when the pointer enters the storage UI.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    /// <summary>
    /// Event callback for when the pointer exits the storage UI.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    /// <summary>
    /// Handles game events and performs corresponding actions in the storage UI.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateStorageAndOpen(gameEventType, data);
        CloseStorage(gameEventType);
        OnStorageItemButtonSelected(gameEventType, data);
        CloseStorageIfDragNDropItemIsOverUI(gameEventType);
    }

    /// <summary>
    /// Selects the close button and raises an event to close the storage UI.
    /// </summary>
    private void SelectCloseButton()
    {
        GameEventHandler.RaiseEvent(GameEventType.CloseStorageUI);
    }

    /// <summary>
    /// Selects the send button and performs necessary actions based on whether any storage items are selected.
    /// </summary>
    private void SelectSendButton()
    {
        bool hasSelectedItem = _selectedStorageItemsList.Count > 0;

        if (!hasSelectedItem)
        {
            DeselectItems();
            UpdateSelectedStorageItemButtonsList(null, true);
            PlaySoundEffect(4, 1);
            return;
        }

        SendSelectedItemsVorVerification();
        DeselectItems();
        UpdateSelectedStorageItemButtonsList(null, true);
    }

    /// <summary>
    /// Sends the selected storage items for verification.
    /// </summary>
    private void SendSelectedItemsVorVerification()
    {
        selectedStorageItemButtonData[0] = _selectedStorageItemsList;
        selectedStorageItemButtonData[1] = _emptyCellSprite;
        GameEventHandler.RaiseEvent(GameEventType.SendStorageItemForVerification, selectedStorageItemButtonData);
    }

    /// <summary>
    /// Updates the storage UI and opens it if the game event type is for opening the storage UI.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The data associated with the event.</param>
    private void UpdateStorageAndOpen(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.OpenStorageUI)
        {
            return;
        }
       
        UpdateStorageItems(data);
        DeselectItems();
        SetCanvasGroupActivity(true);
    }

    /// <summary>
    /// Closes the storage UI based on the game event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    private void CloseStorage(GameEventType gameEventType)
    {
        bool isClosing = gameEventType == GameEventType.CloseStorageUI || gameEventType == GameEventType.DisplayPopupNotification;

        if (!isClosing)
        {
            return;
        }

        SetCanvasGroupActivity(false);
    }

    /// <summary>
    /// Handles the event when a storage item button is selected.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">The event data.</param>
    private void OnStorageItemButtonSelected(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SelectStorageItem)
        {
            return;
        }

        UpdateSelectedStorageItemButtonsList(storageItemButton: (StorageItemButton)data[0]);
    }

    /// <summary>
    /// Closes the storage UI if a drag-and-drop item is being dragged over the UI.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    private void CloseStorageIfDragNDropItemIsOverUI(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.InventoryItemDragNDrop)
        {
            return;
        }

        if (!_isPointerEntered)
        {
            return;
        }

        GameEventHandler.RaiseEvent(GameEventType.CloseStorageUI);
    }

    /// <summary>
    /// Updates the list of selected storage item buttons.
    /// </summary>
    /// <param name="storageItemButton">The storage item button.</param>
    /// <param name="removeAll">Flag indicating whether to remove all selected buttons.</param>
    private void UpdateSelectedStorageItemButtonsList(StorageItemButton storageItemButton, bool removeAll = false)
    {
        if (removeAll)
        {
            _selectedStorageItemsList = new List<StorageItemButton>();
            return;
        }

        if(_selectedStorageItemsList.Exists(button => button == storageItemButton))
        {
            _selectedStorageItemsList.Remove(storageItemButton);
        }
        else
        {
            _selectedStorageItemsList.Add(storageItemButton);
        }
    }

    /// <summary>
    /// Updates the storage items based on the data received.
    /// </summary>
    /// <param name="data">The data containing the storage items.</param>
    private void UpdateStorageItems(object[] data)
    {
        _tempStorageItemsList = (List<StorageItem>)data[0];

        for (int i = 0; i < _itemButtons.Length; i++)
        {
            if(i >= _tempStorageItemsList.Count)
            {
                _itemButtons[i].RemoveItem(_emptyCellSprite);
                _itemButtons[i].transform.SetSiblingIndex(_itemButtons.Length);
                continue;
            }

            _itemButtons[i].AssignStorageItem(_tempStorageItemsList[i]);
        }
    }

    /// <summary>
    /// Deselects all storage item buttons.
    /// </summary>
    private void DeselectItems()
    {
        foreach (var button in _itemButtons)
        {
            button.Deselect(true);
        }
    }

    /// <summary>
    /// Sets the activity of the canvas group to control the visibility of the storage UI.
    /// </summary>
    /// <param name="isActive">Flag indicating whether the storage UI should bevisible.</param>
    private void SetCanvasGroupActivity(bool isActive)
    {
        if(_canvasGroup.interactable == isActive)
        {
            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        PlaySoundEffect(9, isActive ? 0 : 1);
    }

    /// <summary>
    /// Plays a sound effect.
    /// </summary>
    /// <param name="listIndex">The index of the sound effect list.</param>
    /// <param name="clipIndex">The index of the sound clip to play.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}