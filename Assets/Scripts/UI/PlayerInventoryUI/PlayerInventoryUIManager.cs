using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class PlayerInventoryUIManager : MonoBehaviour
{
    [Header("Cells")]
    [SerializeField] private PlayerInventoryItemButton[] _inventoryItemButtons;

    [Header("UI Elements")]
    [SerializeField] private Btn _confirmButton;
    [SerializeField] private Btn_Icon _confirmButtonIcon;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("Confirm Button Icon Sprites")]
    [SerializeField] private Sprite[] _confirmButtonIconSprites;

    private string _errorAnimation = "ErrorAnim";
    private string _confirmButtonSelectAnim = "ConfirmButtonSelectAnim";
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

    public void AssignInvetoryItem(Item item)
    {
        for (int i = 0; i < _inventoryItemButtons.Length; i++)
        {
            bool canAssignItem = _inventoryItemButtons[i].AssosiatedItem == null;

            if (canAssignItem)
            {
                _inventoryItemButtons[i].AssignItem(item);
                _inventoryItemButtons[i].Deselect();
                return;
            }
        }
    }

    public void GetControllerTeam(TeamIndex teamIndex)
    {
        _controllerTeamIndex = teamIndex;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnInventoryItemSelect(gameEventType, data);
        AllowItemConfirmation(gameEventType);
        SellSpoiledItems(gameEventType);
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
        UpdateConfirmButtonIcon();
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
        UpdateConfirmButtonIcon(false);
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
        PlayConfirmButtonSelectAnimation(sellingItemQuantity > 0f);
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

    private void SellSpoiledItems(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellSpoiledItems)
        {
            return;
        }

        DeselectConfirmButton();
        UpdateConfirmButtonIcon(false);
        RemoveAllSelectedItems();

        foreach (var inventoryItemButton in _inventoryItemButtons)
        {
            if(inventoryItemButton.AssosiatedItem == null)
            {
                continue;
            }

            if(inventoryItemButton.ItemSpoilPercentage > 10)
            {
                inventoryItemButton.DestroySpoiledItemOnSeparateSale();
            }

            inventoryItemButton.Deselect();
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

    private void UpdateConfirmButtonIcon(bool isItemSelected = true)
    {
        _confirmButtonIcon.IconSpriteChangeDelegate(isItemSelected ? _confirmButtonIconSprites[1] : _confirmButtonIconSprites[0]);
        _confirmButtonIcon.ChangeReleasedSpriteDelegate();
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

    private void PlayConfirmButtonSelectAnimation(bool canPlay)
    {
        if (!canPlay)
        {
            return;
        }

        _animator.Play(_confirmButtonSelectAnim, 0, 0);
    }

    private void PlayClickSoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(0, clipIndex);
    }
}