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
        _canselButton.OnSelect += OnCanselButtonSelect;
    }

    /// <summary>
    /// Handles game events.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OnGameTurnUpdate(gameEventType);
        OnItemSelect(gameEventType, data);
    }

    /// <summary>
    /// Handles game turn updates.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    private void OnGameTurnUpdate(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        UpdateShopItems();
        UpdateSelectedItemsList();
        DeselectItems();
        SetBuyButtonInteractability();
        UpdateSelectedItemsTotalCostText();
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

        DeselectCommandButtons();
        UpdateSelectedItemsList((ShopItemButton)data[0]);
        UpdateSelectedItemsTotalCostText();
        CheckPurchaseRequirements();
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
        UpdateSelectedItemsTotalCostText();
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
    /// Handles the cancel button selection event.
    /// </summary>
    private void OnCanselButtonSelect()
    {
        UpdateSelectedItemsList();
        UpdateSelectedItemsTotalCostText();
        DeselectItems();
        SetBuyButtonInteractability();
    }

    /// <summary>
    /// Updates the shop items displayed in the item buttons.
    /// </summary>
    private void UpdateShopItems()
    {
        for (int i = 0; i < _items.Collection.Count; i++)
        {
            if(i >= _shopItemButtons.Length)
            {
                break;
            }

            _shopItemButtons[i].UpdateItem(item: _items.Collection[Random.Range(0, _items.Collection.Count)]);
        }
    }

    /// <summary>
    /// Deselects the command buttons.
    /// </summary>
    private void DeselectItems()
    {
        for (int i = 0; i < _shopItemButtons.Length; i++)
        {
            _shopItemButtons[i].Deselect();
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
    private void UpdateSelectedItemsTotalCostText()
    {
        float total = 0f;

        foreach (var item in _selectedItems)
        {
            total += item.Price;
        }

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
}