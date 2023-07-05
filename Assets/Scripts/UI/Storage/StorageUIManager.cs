using UnityEngine;
using Pautik;
using System.Collections.Generic;

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

    private object[] selectedStorageItemButtonData = new object[2];
    private List<Item> _tempItemsList;
    private List<StorageItemButton> _selectedStorageItemButton = new List<StorageItemButton>(); 




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _commandButtons[0].OnSelect += SelectCloseButton;
        _commandButtons[1].OnSelect += SelectSendButton;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateStorageAndOpen(gameEventType, data);
        CloseStorage(gameEventType);
        OnStorageItemButtonSelected(gameEventType, data);
    }

    private void SelectCloseButton()
    {
        GameEventHandler.RaiseEvent(GameEventType.CloseStorageUI);
    }

    private void SelectSendButton()
    {       
        selectedStorageItemButtonData[0] = _selectedStorageItemButton;
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

    private void UpdateSelectedStorageItemButtonsList(StorageItemButton storageItemButton, bool removeAll = false)
    {
        if (removeAll)
        {
            _selectedStorageItemButton = new List<StorageItemButton>();
            return;
        }

        if(_selectedStorageItemButton.Exists(button => button == storageItemButton))
        {
            _selectedStorageItemButton.Remove(storageItemButton);
        }
        else
        {
            _selectedStorageItemButton.Add(storageItemButton);
        }
    }

    private void UpdateStorageItems(object[] data)
    {
        _tempItemsList = (List<Item>)data[0];

        for (int i = 0; i < _itemButtons.Length; i++)
        {
            if(i >= _tempItemsList.Count)
            {
                _itemButtons[i].RemoveItem(_emptyCellSprite);
                _itemButtons[i].transform.SetSiblingIndex(_itemButtons.Length);
                continue;
            }

            _itemButtons[i].AssignItem(_tempItemsList[i]);
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
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}