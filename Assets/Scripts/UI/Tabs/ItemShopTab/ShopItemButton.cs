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

    [Header("Assosiated Item")]
    [SerializeField] private Item _item;

    private object[] _shopItemButtonData = new object[1];
    private bool _isSelected;

    public Item AssosiatedItem => _item;
    public float Price { get; private set; }




    private void OnEnable()
    {      
        GameEventHandler.OnEvent += OnGameEvent;
        _itemButton.OnPointerUpHandler += OnSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PlayShineEffect(gameEventType);
    }

    private void OnSelect()
    {
        ToggleSelectionState(!_isSelected);
        TogglePriceLabelColorBasedOnSelectionState();

        if (!_isSelected)
        {
            RaiseShopItemButtonEvent(GameEventType.OnShopItemButtonDeselect);
            StartCoroutine(DoubleClickToDeselect());
            return;
        }

        RaiseShopItemButtonEvent(GameEventType.OnShopItemButtonSelect);
    }

    public void UpdateItem(Item item, int priceMinRange, int priceMaxRange)
    {
        SetCellEmpty(false);
        AssignItem(item);
        ChangeIcon(item);
        DetermineItemCurrentPrice(item, priceMinRange, priceMaxRange, out float newPrice);
        SetItemPriceLabelColor(color: newPrice < item.Price ? _itemPriceLabelColors[0] : newPrice > item.Price ? _itemPriceLabelColors[2] : _itemPriceLabelColors[1]);
        UpdatePriceText(newPrice);
    }

    public void RemoveItem()
    {
        AssignItem(null);
        SetItemPriceLabelColor(new Color(255f, 255f, 255f, 0f));
    }

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

    private void RaiseShopItemButtonEvent(GameEventType gameEventType)
    {
        _shopItemButtonData[0] = this;
        GameEventHandler.RaiseEvent(gameEventType, _shopItemButtonData);
    }

    private IEnumerator DoubleClickToDeselect()
    {
        yield return new WaitForSeconds(0.01f);
        Deselect();
    }

    private void ToggleSelectionState(bool isSelected)
    {
        _isSelected = isSelected;
    }

    public void Deselect(bool updateSelectionState = false)
    {
        if (AssosiatedItem == null)
        {
            return;
        }

        _itemButton.Deselect();
        TogglePriceLabelColorBasedOnSelectionState();

        if (updateSelectionState)
        {
            ToggleSelectionState(false);
        }
    }

    private void AssignItem(Item item)
    {
        _item = item;
    }

    private void ChangeIcon(Item item)
    {
        _itemIcon.IconSpriteChangeDelegate(item.Icon);
        _itemIcon.ChangeReleasedSpriteDelegate();
    }

    private void DetermineItemCurrentPrice(Item item, int priceMinRange, int priceMaxRange, out float newPrice)
    {
        float itemOriginalPrice = item.Price;
        newPrice = itemOriginalPrice / 100 * (Random.Range(priceMinRange, priceMaxRange));
    }

    private void SetItemPriceLabelColor(Color color)
    {
        _priceLabelDefaultColor = color;
        _itemPriceLabel.color = color;
    }

    private void TogglePriceLabelColorBasedOnSelectionState()
    {
        _itemPriceLabel.color = _isSelected ? new Color(255f, 255f, 255f, 0f) : _priceLabelDefaultColor;
    }

    private void UpdatePriceText(float newPrice)
    {
        Price = newPrice;
        _itemPriceText.SetButtonTitle($"${Converter.ThousandsSeparatorString((int)newPrice, true)}");
    }

    private void PlayShineEffect(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.SellingBuyingTabActivity)
        {
            return;
        }

        _uiShiny.Play();
    }
}