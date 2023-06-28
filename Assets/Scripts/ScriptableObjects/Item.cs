using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private ItemCategory _itemCategory;
    [SerializeField] private ItemDurabilityLevel _itemDurabilityLevel;
    [SerializeField] private ItemPriceLevel _itemPriceLevel;
   
    [SerializeField] private int _itemId;
    [SerializeField] private int _itemPrice;

    public int ID
    {
        get => _itemId;
        set => _itemId = value;
    }
    public int Price
    {
        get => _itemPrice;
        private set => _itemPrice = value;
    }
    public Sprite Icon => _itemIcon;
    public ItemDurabilityLevel ItemDurabilityLevel => _itemDurabilityLevel;




    public void SetItemPrice()
    {
        switch (_itemPriceLevel)
        {
            case ItemPriceLevel.LowPrice: Price = Random.Range(1, 10); break;
            case ItemPriceLevel.MidRange: Price = Random.Range(11, 50); break;
            case ItemPriceLevel.High: Price = Random.Range(51, 100); break;
            case ItemPriceLevel.Luxury: Price = Random.Range(101, 250); break;
        }
    }

    public void SetID(int id)
    {
        ID = id;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}