using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private PlayerInventoryItemButton[] _inventoryItemButtons;
    [SerializeField] private Btn _confirmButton;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    private string _errorAnimation = "ErrorAnim";

    private List<PlayerInventoryItemButton> _selectedItemButtonsList = new List<PlayerInventoryItemButton>();




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _confirmButton.OnSelect += OnConfirmButtonSelect;
    }

    public void AssignInvetoryItem(int inventoryItemButtonIndex, Item item)
    {
        _inventoryItemButtons[inventoryItemButtonIndex].AssignItem(item);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnInventoryItemSelect(gameEventType, data);
    }

    private void OnInventoryItemSelect(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.SelectInventoryItem)
        {
            return;
        }

        AddSelectedItemToList(playerInventoryItemButton: (PlayerInventoryItemButton)data[0]);
        PlayClickSoundEffect(9);
        DeselectConfirmButton();
    }

    private void OnConfirmButtonSelect()
    {
        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            return;
        }

        TryConfirmSelectedItem(out bool isNoBuyingItemSelected);
        DismissItemConfirmation(isNoBuyingItemSelected);
        RemoveAllSelectedItems();
        PlayClickSoundEffect(11);
    }

    private void TryConfirmSelectedItem(out bool isNoBuyingItemSelected)
    {
        isNoBuyingItemSelected = true;

        for (int i = 0; i < _selectedItemButtonsList.Count; i++)
        {
            bool isSelectedItemBuyingItem = _selectedItemButtonsList[i].AssosiatedItem == GameSceneReferences.Manager.ItemsBuyerManager.BuyingItem;

            if (isSelectedItemBuyingItem)
            {
                _selectedItemButtonsList[i] = null;
                isNoBuyingItemSelected = false;
                continue;
            }

            _selectedItemButtonsList[i].Deselect();
        }
    }

    private void DismissItemConfirmation(bool isNoBuyingItemSelected)
    {
        if (isNoBuyingItemSelected)
        {
            PlayErrorAnimationAndSound();
        }
    }

    private void DeselectConfirmButton()
    {
        _confirmButton.Deselect();
    }

    private void RemoveAllSelectedItems()
    {
        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            return;
        }

        _selectedItemButtonsList = new List<PlayerInventoryItemButton>();
    }

    private void AddSelectedItemToList(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _selectedItemButtonsList.Add(playerInventoryItemButton);
    }

    private void PlayErrorAnimationAndSound()
    {
        _animator.Play(_errorAnimation, 0, 0);
        UISoundController.PlaySound(4, 0);
    }

    private void PlayClickSoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(0, clipIndex);
    }
}