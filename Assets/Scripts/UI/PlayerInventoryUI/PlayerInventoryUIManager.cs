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

    private const string _errorAnimation = "ErrorAnim";
    private const string _confirmButtonSelectAnim = "ConfirmButtonSelectAnim";
    private const string _blockAnimation = "BlockAnim";
    private const string _unblockAnimation = "Unblock";
    private object[] _sellingInventoryItemData = new object[3];
    private bool _isItemConfirmed;
    private bool _isSaleRestricted;
    private int _saleRestrictionDuration;
    private TeamIndex _controllerTeamIndex;
    private List<PlayerInventoryItemButton> _selectedItemButtonsList = new List<PlayerInventoryItemButton>();

    /// <summary>
    /// Gets a boolean value indicating if the player can confirm an item.
    /// </summary>
    private bool CanConfirmItem
    {
        get => GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn == _controllerTeamIndex && !_isItemConfirmed;
    }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _confirmButton.OnSelect += OnConfirmButtonSelect;
    }

    /// <summary>
    /// Assigns an item to the inventory.
    /// </summary>
    /// <param name="item">The item to be assigned.</param>
    public void AssignInvetoryItem(Item item)
    {
        IterateInventoryItemButtons(inventoryItemButton => 
        {
            inventoryItemButton?.AssignItem(item);
            inventoryItemButton?.Deselect();
        });
    }

    /// <summary>
    /// Assigns an item to the inventory with a new lifetime.
    /// </summary>
    /// <param name="item">The item to be assigned.</param>
    /// <param name="newLifetime">The new lifetime value for the item.</param>
    public void AssignInvetoryItem(Item item, int newLifetime)
    {
        IterateInventoryItemButtons(inventoryItemButton =>
        {
            inventoryItemButton?.AssignItem(item, newLifetime);
            inventoryItemButton?.Deselect();
        });
    }

    /// <summary>
    /// Iterates through the inventory item buttons and performs the update action on the first available button.
    /// </summary>
    /// <param name="updateInventoryButton">The action to update the inventory button.</param>
    private void IterateInventoryItemButtons(System.Action<PlayerInventoryItemButton> UpdateInventoryButton)
    {
        for (int i = 0; i < _inventoryItemButtons.Length; i++)
        {
            bool canAssignItem = _inventoryItemButtons[i].AssociatedItem == null;

            if (canAssignItem)
            {
                UpdateInventoryButton?.Invoke(_inventoryItemButtons[i]);
                return;
            }
        }
    }

    /// <summary>
    /// Sets the team index of the controller.
    /// </summary>
    /// <param name="teamIndex">The team index of the controller.</param>
    public void GetControllerTeam(TeamIndex teamIndex)
    {
        _controllerTeamIndex = teamIndex;
    }

    /// <summary>
    /// Handles the game event based on the specified event type and data.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">The data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnInventoryItemSelect(gameEventType, data);
        AllowItemConfirmation(gameEventType);
        SellSpoiledItems(gameEventType);
        ApplySaleRestriction(gameEventType, data);
        CheckSaleRestriction(gameEventType, data);
    }

    /// <summary>
    /// Executes actions when an inventory item is selected for sale.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void OnInventoryItemSelect(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.SelectInventoryItemForSale)
        {
            return;
        }

        AddSelectedItemToList(playerInventoryItemButton: (PlayerInventoryItemButton)data[0]);
        PlaySoundEffect(0, 9);
        DeselectConfirmButton();
        UpdateConfirmButtonIcon();
    }

    /// <summary>
    /// Allows item confirmation when the game turn is updated.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    private void AllowItemConfirmation(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        _isItemConfirmed = false;
    }

    /// <summary>
    /// Sells spoiled items when the corresponding game event occurs.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    private void SellSpoiledItems(GameEventType gameEventType)
    {
        if (gameEventType != GameEventType.SellSpoiledItems)
        {
            return;
        }

        DeselectConfirmButton();
        UpdateConfirmButtonIcon(false);
        RemoveAllSelectedItems();

        foreach (var inventoryItemButton in _inventoryItemButtons)
        {
            if (inventoryItemButton.AssociatedItem == null)
            {
                continue;
            }

            if (inventoryItemButton.ItemSpoilPercentage > 10)
            {
                inventoryItemButton.DestroySpoiledItemOnSeparateSale();
            }

            inventoryItemButton.Deselect();
        }
    }

    /// <summary>
    /// Applies the sale restriction based on the specified data.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void ApplySaleRestriction(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.RestrictSaleAbility)
        {
            return;
        }

        RetrieveRestrictionData(data, out int duration);
        ToggleSaleRestrictionStatus(true);
        _saleRestrictionDuration += GameSceneReferences.Manager.GameTurnManager.TurnCount + duration;
    }

    /// <summary>
    /// Checks if the sale restriction is active and resets it if necessary.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the game event.</param>
    private void CheckSaleRestriction(GameEventType gameEventType, object[] data)
    {
        bool isSaleRestrictionActive = gameEventType == GameEventType.UpdateGameTurn && _isSaleRestricted && _controllerTeamIndex == (TeamIndex)data[2];

        if (!isSaleRestrictionActive)
        {
            return;
        }

        ResetSaleRestriction(data);
    }

    /// <summary>
    /// Retrieves the sale restriction data and calculates the duration.
    /// </summary>
    /// <param name="data">Additional data associated with the game event.</param>
    /// <param name="duration">The calculated duration of the sale restriction.</param>
    private void RetrieveRestrictionData(object[] data, out int duration)
    {
        int restrictionTurnsCount = (int)data[0];
        duration = GameSceneReferences.Manager.GameTurnManager.CurrentTeamTurn == _controllerTeamIndex ? restrictionTurnsCount * 2 : (restrictionTurnsCount * 2) - 1;
    }

    /// <summary>
    /// Toggles the sale restriction status and plays corresponding animations and sound effects.
    /// </summary>
    /// <param name="isSaleRestricted">Indicates whether the sale is currently restricted.</param>
    private void ToggleSaleRestrictionStatus(bool isSaleRestricted)
    {
        _isSaleRestricted = isSaleRestricted;

        if (_isSaleRestricted)
        {
            PlayAnimation(_blockAnimation);
            PlaySoundEffect(8, 0);
        }
        else
        {
            PlayAnimation(_unblockAnimation);
            PlaySoundEffect(8, 1);
        }
    }

    /// <summary>
    /// Resets the sale restriction based on the specified data.
    /// </summary>
    /// <param name="data">Additional data associated with the game event.</param>
    private void ResetSaleRestriction(object[] data)
    {
        bool isSaleRestrictionExpired = _saleRestrictionDuration < (int)data[3];

        if (isSaleRestrictionExpired)
        {
            ToggleSaleRestrictionStatus(false);
            _saleRestrictionDuration = 0;
        }
    }

    /// <summary>
    /// Executes actions when the confirm button is selected.
    /// </summary>
    private void OnConfirmButtonSelect()
    {
        if (_isSaleRestricted)
        {
            return;
        }

        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            DismissItemConfirmation(true);
            return;
        }

        TryConfirmSelectedItem(out bool isNoBuyingItemSelected);
        DismissItemConfirmation(isNoBuyingItemSelected);
        RemoveAllSelectedItems();
        PlaySoundEffect(0, 11);
        UpdateConfirmButtonIcon(false);
    }

    /// <summary>
    /// Tries to confirm the selected item for sale and updates relevant variables.
    /// </summary>
    /// <param name="isNoBuyingItemSelected">Indicates whether no buying item was selected.</param>
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
            bool isSelectedItemBuyingItem = _selectedItemButtonsList[i].AssociatedItem == GameSceneReferences.Manager.ItemsBuyerManager.BuyingItem;

            if (isSelectedItemBuyingItem)
            {               
                sellingItemQuantity++;
                sellingItemId = _selectedItemButtonsList[i].AssociatedItem.ID;
                sellingItemSpoilPercentage += _selectedItemButtonsList[i].ItemSpoilPercentage;
                isNoBuyingItemSelected = false;
                _selectedItemButtonsList[i].RemoveAssociatedItem();
                _selectedItemButtonsList[i] = null;
                _isItemConfirmed = true;
                continue;
            }

            _selectedItemButtonsList[i].Deselect();
        }

        SendSellingItemData(sellingItemQuantity, sellingItemId, sellingItemSpoilPercentage);
        PlayConfirmButtonSelectAnimation(sellingItemQuantity > 0f);
    }

    /// <summary>
    /// Sends the data of the selected item for sale to the game event handler.
    /// </summary>
    /// <param name="sellingItemQuantity">The quantity of the item being sold.</param>
    /// <param name="sellingItemId">The ID of the item being sold.</param>
    /// <param name="sellingItemSpoilPercentage">The spoil percentage of the item being sold.</param>
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

    /// <summary>
    /// Dismisses the item confirmation and plays error animation and sound if no buying item is selected.
    /// </summary>
    /// <param name="isNoBuyingItemSelected">Indicates whether no buying item was selected.</param>
    private void DismissItemConfirmation(bool isNoBuyingItemSelected)
    {
        if (isNoBuyingItemSelected)
        {
            PlayErrorAnimationAndSound();
        }
    }

    /// <summary>
    /// Deselects the confirm button.
    /// </summary>
    private void DeselectConfirmButton()
    {
        _confirmButton.Deselect();
    }

    /// <summary>
    /// Updates the icon of the confirm button based on the selection status.
    /// </summary>
    /// <param name="isItemSelected">Indicates whether an item is currently selected.</param>
    private void UpdateConfirmButtonIcon(bool isItemSelected = true)
    {
        _confirmButtonIcon.IconSpriteChangeDelegate(isItemSelected ? _confirmButtonIconSprites[1] : _confirmButtonIconSprites[0]);
        _confirmButtonIcon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Removes all selected items from the list.
    /// </summary>
    private void RemoveAllSelectedItems()
    {
        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            return;
        }

        _selectedItemButtonsList = new List<PlayerInventoryItemButton>();
    }

    /// <summary>
    /// Adds the selected item to the list of selected items.
    /// </summary>
    /// <param name="playerInventoryItemButton">The button representing the selected item.</param>
    private void AddSelectedItemToList(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _selectedItemButtonsList.Add(playerInventoryItemButton);
    }

    /// <summary>
    /// Plays the error animation and sound effect when there is an error during item confirmation.
    /// </summary>
    private void PlayErrorAnimationAndSound()
    {
        PlayAnimation(_errorAnimation);
        UISoundController.PlaySound(4, 0);
    }

    /// <summary>
    /// Plays the animation for selecting the confirm button when there are items to confirm.
    /// </summary>
    /// <param name="canPlay">Indicates whether the animation can be played.</param>
    private void PlayConfirmButtonSelectAnimation(bool canPlay)
    {
        if (!canPlay)
        {
            return;
        }

        PlayAnimation(_confirmButtonSelectAnim);
    }

    /// <summary>
    /// Plays the specified animation.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName, 0, 0);
    }

    /// <summary>
    /// Plays the specified sound effect.
    /// </summary>
    /// <param name="listIndex">The index of the sound effect list.</param>
    /// <param name="soundIndex">The index of the sound effect within the list.</param>
    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}