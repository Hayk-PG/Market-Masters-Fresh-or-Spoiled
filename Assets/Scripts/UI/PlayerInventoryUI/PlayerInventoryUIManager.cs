using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class PlayerInventoryUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private PlayerInventoryItemButton[] _inventoryItemButtons;
    [SerializeField] private Btn _confirmButton;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    private string _errorAnimation = "ErrorAnim";
    private object[] _sellingInventoryItemData = new object[3];
    private bool _isItemConfirmed;
    private TeamIndex _controllerTeamIndex;
    private List<PlayerInventoryItemButton> _selectedItemButtonsList = new List<PlayerInventoryItemButton>();

    private bool CanConfirmItem
    {
        get => GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn == _controllerTeamIndex && !_isItemConfirmed;
    }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _confirmButton.OnSelect += OnConfirmButtonSelect;
    }

    public void AssignInvetoryItem(int inventoryItemButtonIndex, Item item)
    {
        _inventoryItemButtons[inventoryItemButtonIndex].AssignItem(item);
    }

    public void GetControllerTeam(TeamIndex teamIndex)
    {
        _controllerTeamIndex = teamIndex;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnInventoryItemSelect(gameEventType, data);
        AllowItemConfirmation(gameEventType);
    }

    private void OnInventoryItemSelect(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.SelectInventoryItemForSale)
        {
            return;
        }

        AddSelectedItemToList(playerInventoryItemButton: (PlayerInventoryItemButton)data[0]);
        PlayClickSoundEffect(9);
        DeselectConfirmButton();
    }

    private void AllowItemConfirmation(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        _isItemConfirmed = false;
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

        if (!CanConfirmItem)
        {
            GlobalFunctions.Loop<PlayerInventoryItemButton>.Foreach(_selectedItemButtonsList, button => button.Deselect());
            return;
        }

        int sellingItemQuantity = 0;
        int sellingItemId = 0;
        int sellingItemSpoilPercentage = 0;

        for (int i = 0; i < _selectedItemButtonsList.Count; i++)
        {
            bool isSelectedItemBuyingItem = _selectedItemButtonsList[i].AssosiatedItem == GameSceneReferences.Manager.ItemsBuyerManager.BuyingItem;

            if (isSelectedItemBuyingItem)
            {               
                sellingItemQuantity++;
                sellingItemId = _selectedItemButtonsList[i].AssosiatedItem.ID;
                sellingItemSpoilPercentage += _selectedItemButtonsList[i].ItemSpoilPercentage;
                isNoBuyingItemSelected = false;
                _selectedItemButtonsList[i].RemoveAssosiatedItem();
                _selectedItemButtonsList[i] = null;
                _isItemConfirmed = true;
                continue;
            }

            _selectedItemButtonsList[i].Deselect();
        }

        SendSellingItemData(sellingItemQuantity, sellingItemId, sellingItemSpoilPercentage);
    }

    private void SendSellingItemData(int sellingItemQuantity, int sellingItemId, int sellingItemSpoilPercentage)
    {
        if (sellingItemQuantity > 0)
        {
            _sellingInventoryItemData[0] = sellingItemQuantity;
            _sellingInventoryItemData[1] = sellingItemId;
            _sellingInventoryItemData[2] = sellingItemSpoilPercentage;
            GameEventHandler.RaiseEvent(GameEventType.ConfirmInventoryItemForSale, _sellingInventoryItemData);
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