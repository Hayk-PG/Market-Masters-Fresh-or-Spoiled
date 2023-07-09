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
    [SerializeField] private Button _buyButtonComponent;
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

    private void UpdateShopItems(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateShopItems)
        {
            return;
        }

        UpdateShopItems(count: (int)data[0], priceRange: (System.Tuple<int, int>)data[1]);
        DeselectItems(_shopItemButtons);
        UpdateSelectedItemsList();
        UpdateSelectedItemsTotalCostText();
        SetBuyButtonInteractability();        
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

    private void OnItemDeselect(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.OnShopItemButtonDeselect)
        {
            return;
        }

        RemoveItemFromSelectedItemsList(shopItemButton: (ShopItemButton)data[0]);
    }

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
        UpdateSelectedItemsTotalCostText(true);
        UpdateSelectedItemsList();
        DeselectItems(_selectedItems.ToArray());
    }

    /// <summary>
    /// Sends the selected items for confirmation.
    /// </summary>
    private void SendSelectedItemsForConfirmation()
    {
        _selectedItemsData = _selectedItems.ToArray();
        GameEventHandler.RaiseEvent(GameEventType.TryBuySelectedShopItem, _selectedItemsData);
    }

    private void OnTabActivity(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        DeselectItems(_shopItemButtons);
    }

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
        for (int i = 0; i < shopItemButtons.Length; i++)
        {
            shopItemButtons[i].Deselect(true);
        }
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

        foreach (var item in _selectedItems)
        {
            total += item.Price;
        }

        SetTotalCostValueAndText(total);
    }

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
        _buyButtonComponent.interactable = isInteractable;
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}