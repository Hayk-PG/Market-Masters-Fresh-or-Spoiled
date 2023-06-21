using UnityEngine;
using UnityEngine.UI;

public class ItemsBuyerUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _buyingItemIcon;

    private Item _currentBuyingItem;



    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateBuyingItemIcon(gameEventType, data);
    }

    private void UpdateBuyingItemIcon(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateBuyingItem)
        {
            return;
        }

        _currentBuyingItem = (Item)data[0];
        _buyingItemIcon.sprite = _currentBuyingItem.Icon;
    }
}