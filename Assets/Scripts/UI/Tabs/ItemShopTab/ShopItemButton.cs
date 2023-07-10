using UnityEngine;
using UnityEngine.UI;
using Pautik;
using Coffee.UIEffects;
using System.Collections;

public class ShopItemButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Btn _itemButton;
    [SerializeField] private Btn_Icon _itemIcon;  
    [SerializeField] private BtnTxt _itemPriceText;
    [SerializeField] private Image _itemPriceLabel;
    [SerializeField] private UIShiny _uiShiny;
    [SerializeField] private CanvasGroup _emtpyIndicatorCanvasGroup;

    [Header("Colors")]
    [SerializeField] private Color _emptyItemIconColor;
    [SerializeField] private Color[] _itemPriceLabelColors;
    private Color _priceLabelDefaultColor;
    private Color _transparent = new Color(255f, 255f, 255f, 0f);

    [Header("Assosiated Item")]
    [SerializeField] private Item _item;

    private object[] _shopItemButtonData = new object[1];
    private bool _isSelected;

    public Item AssociatedItem => _item;
    public bool HasAssociatedItem => AssociatedItem != null;
    public float Price { get; private set; }




    private void OnEnable()
    {      
        GameEventHandler.OnEvent += OnGameEvent;
        _itemButton.OnPointerUpHandler += OnSelect;
    }

    /// <summary>
    /// Handles the game events and triggers the shine effect if necessary.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Additional event data.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PlayShineEffect(gameEventType);
    }

    /// <summary>
    /// Handles the selection/deselection of the shop item button.
    /// </summary>
    private void OnSelect()
    {
        if (!HasAssociatedItem)
        {
            return;
        }

        ToggleSelectionState(!_isSelected);
        TogglePriceLabelColorBasedOnSelectionState();

        if (!_isSelected)
        {
            RaiseShopItemButtonEventBasedOnSelectionState(GameEventType.OnShopItemButtonDeselect);
            StartCoroutine(DoubleClickToDeselect());
            return;
        }

        RaiseShopItemButtonEventBasedOnSelectionState(GameEventType.OnShopItemButtonSelect);
    }

    /// <summary>
    /// Updates the shop item button with a new item and its associated details.
    /// </summary>
    /// <param name="item">The new item to be displayed.</param>
    /// <param name="priceMinRange">The minimum price range for the item.</param>
    /// <param name="priceMaxRange">The maximum price range for the item.</param>
    public void UpdateItem(Item item, int priceMinRange, int priceMaxRange)
    {
        SetCellEmpty(false);
        AssignItem(item);
        ChangeIcon(item);
        DetermineItemCurrentPrice(item, priceMinRange, priceMaxRange, out float newPrice);
        SetItemPriceLabelColor(color: newPrice < item.Price ? _itemPriceLabelColors[0] : newPrice > item.Price ? _itemPriceLabelColors[2] : _itemPriceLabelColors[1]);
        UpdatePriceText(newPrice);
    }

    /// <summary>
    /// Removes the currently displayed item from the shop item button.
    /// </summary>
    public void RemoveItem()
    {
        AssignItem(null);
        SetItemPriceLabelColor(_transparent);
    }

    /// <summary>
    /// Sets the empty state of the shop item button.
    /// </summary>
    /// <param name="isEmpty">Indicates if the button is empty.</param>
    public void SetCellEmpty(bool isEmpty)
    {
        GlobalFunctions.CanvasGroupActivity(_emtpyIndicatorCanvasGroup, isEmpty);
        _itemIcon.gameObject.SetActive(!isEmpty);
        _itemPriceLabel.gameObject.SetActive(!isEmpty);
        _itemButton.IsInteractable = !isEmpty;
        _uiShiny.enabled = !isEmpty;

        if (isEmpty)
        {
            RemoveItem();
        }
    }

    /// <summary>
    /// Deselects the shop item button.
    /// </summary>
    /// <param name="updateSelectionState">Specifies whether to update the selection state.</param>
    public void Deselect(bool updateSelectionState = false)
    {
        if (!HasAssociatedItem)
        {
            return;
        }

        _itemButton.Deselect();

        if (updateSelectionState)
        {
            ToggleSelectionState(false);
        }

        TogglePriceLabelColorBasedOnSelectionState();
    }

    /// <summary>
    /// Raises a game event based on the selection state of the shop item button.
    /// </summary>
    /// <param name="gameEventType">The type of game event to raise.</param>
    private void RaiseShopItemButtonEventBasedOnSelectionState(GameEventType gameEventType)
    {
        _shopItemButtonData[0] = this;
        GameEventHandler.RaiseEvent(gameEventType, _shopItemButtonData);
    }

    /// <summary>
    /// Coroutine that allows double-clicking to deselect the shop item button.
    /// </summary>
    private IEnumerator DoubleClickToDeselect()
    {
        yield return new WaitForSeconds(0.01f);
        Deselect();
    }

    /// <summary>
    /// Toggles the selection state of the shop item button.
    /// </summary>
    /// <param name="isSelected">Indicates whether the button is selected.</param>
    private void ToggleSelectionState(bool isSelected)
    {
        _isSelected = isSelected;
    }

    /// <summary>
    /// Assigns the specified item to the shop item button.
    /// </summary>
    /// <param name="item">The item to be assigned.</param>
    private void AssignItem(Item item)
    {
        _item = item;
    }

    /// <summary>
    /// Changes the icon of the shop item button to match the specified item.
    /// </summary>
    /// <param name="item">The item to use for the icon.</param>
    private void ChangeIcon(Item item)
    {
        _itemIcon.IconSpriteChangeDelegate(item.Icon);
        _itemIcon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Determines the current price of the item within the specified price range.
    /// </summary>
    /// <param name="item">The item to determine the price for.</param>
    /// <param name="priceMinRange">The minimum price range.</param>
    /// <param name="priceMaxRange">The maximum price range.</param>
    /// <param name="newPrice">The calculated new price.</param>
    private void DetermineItemCurrentPrice(Item item, int priceMinRange, int priceMaxRange, out float newPrice)
    {
        float itemOriginalPrice = item.Price;
        newPrice = itemOriginalPrice / 100 * (Random.Range(priceMinRange, priceMaxRange));
    }

    /// <summary>
    /// Sets the color of the item price label.
    /// </summary>
    /// <param name="color">The color to set.</param>
    private void SetItemPriceLabelColor(Color color)
    {
        bool isTargetColorTransparent = color == _transparent;

        if (!isTargetColorTransparent)
        {
            _priceLabelDefaultColor = color;
        }

        _itemPriceLabel.color = color;
    }

    /// <summary>
    /// Toggles the color of the item price label based on the selection state.
    /// </summary>
    private void TogglePriceLabelColorBasedOnSelectionState()
    {
        _itemPriceLabel.color = _isSelected ? _transparent : _priceLabelDefaultColor;
    }

    /// <summary>
    /// Updates the price text displayed on the shop item button.
    /// </summary>
    /// <param name="newPrice">The new price to display.</param>
    private void UpdatePriceText(float newPrice)
    {
        Price = newPrice;
        _itemPriceText.SetButtonTitle($"${Converter.ThousandsSeparatorString((int)newPrice, true)}");
    }

    /// <summary>
    /// Plays the shine effect on the shop item button.
    /// </summary>
    /// <param name="gameEventType">The type of game event triggering the shine effect.</param>
    private void PlayShineEffect(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        _uiShiny.Play();
    }
}