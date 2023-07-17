using System.Collections.Generic;
using UnityEngine;
using Pautik;
using UnityEngine.UI;

/// <summary>
/// Manages the item shop functionality, including item selection and purchase.
/// </summary>
public class ItemShopManager : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private Items _items;

    [Header("Shop Items")]
    [SerializeField] private ShopItemButton[] _shopItemButtons;

    [Header("List Of Selected Items")]
    [SerializeField] private List<ShopItemButton> _selectedItems = new List<ShopItemButton>();

    [Header("UI Elements")]
    [SerializeField] private Btn _buyButton;
    [SerializeField] private Btn _canselButton;
    [SerializeField] private BtnTxt _selectedItemsTotalText;

    private float _selectedItemsTotalCost;

    private object[] _selectedItemsData;
    private object[] _purchaseRequirementData = new object[2];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _buyButton.OnSelect += OnBuyButtonSelect;
        _canselButton.OnSelect += OnCloseButtonSelect;
    }

    /// <summary>
    /// Handles game events.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateShopItems(gameEventType, data);
        OnItemSelect(gameEventType, data);
        OnItemDeselect(gameEventType, data);
        OnTabActivity(gameEventType);
    }

    /// <summary>
    /// Updates the shop items based on the game event data.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void UpdateShopItems(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateShopItems)
        {
            return;
        }

        UpdateShopItems(count: (int)data[0], priceRange: (System.Tuple<int, int>)data[1]);
        RestoreDefaultState(_shopItemButtons);      
    }

    /// <summary>
    /// Handles item selection events.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void OnItemSelect(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.OnShopItemButtonSelect)
        {
            return;
        }

        PlaySoundEffect(0, 8);
        DeselectCommandButtons();
        UpdateSelectedItemsList((ShopItemButton)data[0]);
        UpdateSelectedItemsTotalCostText();
        CheckPurchaseRequirements();
    }

    /// <summary>
    /// Handles the event when an item is deselected.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void OnItemDeselect(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.OnShopItemButtonDeselect)
        {
            return;
        }

        RemoveItemFromSelectedItemsList(shopItemButton: (ShopItemButton)data[0]);
        CheckPurchaseRequirements();
    }

    /// <summary>
    /// Raises the event when the close button is selected.
    /// </summary>
    private void OnCloseButtonSelect()
    {
        GameEventHandler.RaiseEvent(GameEventType.SellingBuyingTabActivity);
    }

    /// <summary>
    /// Handles the buy button selection event.
    /// </summary>
    private void OnBuyButtonSelect()
    {
        bool hasSelectedItem = _selectedItems.Count > 0;

        if (!hasSelectedItem)
        {
            return;
        }

        SendSelectedItemsForConfirmation();
        RestoreDefaultState(_selectedItems.ToArray());
    }

    /// <summary>
    /// Sends the selected items for confirmation.
    /// </summary>
    private void SendSelectedItemsForConfirmation()
    {
        _selectedItemsData = _selectedItems.ToArray();
        GameEventHandler.RaiseEvent(GameEventType.TryBuySelectedShopItem, _selectedItemsData);
    }

    /// <summary>
    /// Handles the event when a tab activity occurs.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    private void OnTabActivity(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        RestoreDefaultState(_shopItemButtons);
    }

    /// <summary>
    /// Restores the default state of the shop item buttons.
    /// </summary>
    /// <param name="shopItemButtons">The array of shop item buttons.</param>
    private void RestoreDefaultState(ShopItemButton[] shopItemButtons)
    {
        DeselectItems(shopItemButtons);
        UpdateSelectedItemsList();
        UpdateSelectedItemsTotalCostText();           
        SetBuyButtonInteractability();
    }

    /// <summary>
    /// Updates the shop items based on the specified count and price range.
    /// </summary>
    /// <param name="count">The number of items to update.</param>
    /// <param name="priceRange">The price range for the items.</param>
    private void UpdateShopItems(int count, System.Tuple<int, int> priceRange)
    {
        int itemsCount = count;
        int minRange = priceRange.Item1;
        int maxRange = priceRange.Item2;

        for (int i = 0; i < _items.Collection.Count; i++)
        {
            bool isIndexInRange = i < itemsCount;
            bool isIndexOutOfBounds = i >= _shopItemButtons.Length;

            if (isIndexInRange)
            {
                _shopItemButtons[i].UpdateItem(item: _items.Collection[Random.Range(0, _items.Collection.Count)], minRange, maxRange);
                continue;
            }

            if (isIndexOutOfBounds)
            {
                break;
            }

            _shopItemButtons[i].SetCellEmpty(true);
        }
    }

    /// <summary>
    /// Deselects the command buttons.
    /// </summary>
    private void DeselectItems(ShopItemButton[] shopItemButtons)
    {
        GlobalFunctions.Loop<ShopItemButton>.Foreach(shopItemButtons, shopItemButton => { shopItemButton.Deselect(true); });
    }

    /// <summary>
    /// Updates the list of selected items.
    /// </summary>
    /// <param name="shopItemButton">The selected shop item button.</param>
    private void DeselectCommandButtons()
    {
        _buyButton.Deselect();
        _canselButton.Deselect();
    }

    /// <summary>
    /// Removes the specified shop item button from the selected items list.
    /// </summary>
    /// <param name="shopItemButton">The shop item button to remove.</param>
    private void RemoveItemFromSelectedItemsList(ShopItemButton shopItemButton)
    {
        if(_selectedItems.Exists(itemButton => itemButton == shopItemButton))
        {
            _selectedItems.Remove(shopItemButton);
            UpdateSelectedItemsTotalCostText();
        }
    }

    /// <summary>
    /// Updates the list of selected items.
    /// </summary>
    /// <param name="shopItemButton">The selected shop item button.</param>
    private void UpdateSelectedItemsList(ShopItemButton shopItemButton = null)
    {
        if(shopItemButton == null)
        {
            _selectedItems = new List<ShopItemButton>();
            return;
        }

        _selectedItems.Add(shopItemButton);
    }

    /// <summary>
    /// Updates the text displaying the total cost of selected items.
    /// </summary>
    private void UpdateSelectedItemsTotalCostText(bool resetText = false)
    {
        float total = 0f;

        if (resetText)
        {
            SetTotalCostValueAndText(total);
            return;
        }

        GlobalFunctions.Loop<ShopItemButton>.Foreach(_selectedItems, selectedItem => total += selectedItem.Price);
        SetTotalCostValueAndText(total);
    }

    /// <summary>
    /// Sets the total cost value and updates the total cost text.
    /// </summary>
    /// <param name="total">The total cost value to set.</param>
    private void SetTotalCostValueAndText(float total)
    {
        _selectedItemsTotalCost = total;
        _selectedItemsTotalText.SetButtonTitle($"${Converter.ThousandsSeparatorString((int)_selectedItemsTotalCost, true)}");
    }

    /// <summary>
    /// Checks the purchase requirements for the selected items.
    /// </summary>
    private void CheckPurchaseRequirements()
    {
        _purchaseRequirementData[0] = _selectedItemsTotalCost;
        _purchaseRequirementData[1] = this;
        GameEventHandler.RaiseEvent(GameEventType.MeetsPurchaseRequirements, _purchaseRequirementData);
    }

    /// <summary>
    /// Sets the interactability of the buy button.
    /// </summary>
    /// <param name="isInteractable">Whether the buy button should be interactable.</param>
    public void SetBuyButtonInteractability(bool isInteractable = true)
    {
        _buyButton.IsInteractable = isInteractable;
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}