using UnityEngine;
using UnityEngine.UI;
using Pautik;
using Coffee.UIEffects;

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

    [Header("Assosiated Item")]
    [SerializeField] private Item _item;

    private object[] _shopItemButtonData = new object[1];

    public Item AssosiatedItem => _item;
    public float Price { get; private set; }




    private void OnEnable()
    {
        _itemButton.OnSelect += OnSelect;
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnSelect()
    {
        _shopItemButtonData[0] = this;
        GameEventHandler.RaiseEvent(GameEventType.OnShopItemButtonSelect, _shopItemButtonData);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PlayShineEffect(gameEventType);
    }

    public void UpdateItem(Item item)
    {
        SetCellEmpty(false);
        AssignItem(item);
        ChangeIcon(item);
        DetermineItemCurrentPrice(item, out float newPrice);
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

    public void Deselect()
    {
        if(AssosiatedItem == null)
        {
            return;
        }

        _itemButton.Deselect();
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

    private void DetermineItemCurrentPrice(Item item, out float newPrice)
    {
        float itemOriginalPrice = item.Price;
        newPrice = itemOriginalPrice / 100 * (Random.Range(40, 400));
    }

    private void SetItemPriceLabelColor(Color color)
    {
        _itemPriceLabel.color = color;
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