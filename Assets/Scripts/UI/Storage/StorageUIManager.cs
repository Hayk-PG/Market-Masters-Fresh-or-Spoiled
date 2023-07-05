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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntered = false;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateStorageAndOpen(gameEventType, data);
        CloseStorage(gameEventType);
        OnStorageItemButtonSelected(gameEventType, data);
        CloseStorageIfDragNDropItemIsOverUI(gameEventType);
    }

    private void SelectCloseButton()
    {
        GameEventHandler.RaiseEvent(GameEventType.CloseStorageUI);
    }

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

        selectedStorageItemButtonData[0] = _selectedStorageItemsList;
        selectedStorageItemButtonData[1] = _emptyCellSprite;
        GameEventHandler.RaiseEvent(GameEventType.SendStorageItemForVerification, selectedStorageItemButtonData);
        DeselectItems();
        UpdateSelectedStorageItemButtonsList(null, true);
    }

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

    private void CloseStorage(GameEventType gameEventType)
    {
        bool isClosing = gameEventType == GameEventType.CloseStorageUI || gameEventType == GameEventType.DisplayPopupNotification;

        if (!isClosing)
        {
            return;
        }

        SetCanvasGroupActivity(false);
    }

    private void OnStorageItemButtonSelected(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SelectStorageItem)
        {
            return;
        }

        UpdateSelectedStorageItemButtonsList(storageItemButton: (StorageItemButton)data[0]);
    }

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

    private void DeselectItems()
    {
        foreach (var button in _itemButtons)
        {
            button.Deselect();
        }
    }

    private void SetCanvasGroupActivity(bool isActive)
    {
        if(_canvasGroup.interactable == isActive)
        {
            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        PlaySoundEffect(9, isActive ? 0 : 1);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}