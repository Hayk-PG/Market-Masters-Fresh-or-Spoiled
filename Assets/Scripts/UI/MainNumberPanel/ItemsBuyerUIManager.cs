using UnityEngine;
using UnityEngine.UI;

public class ItemsBuyerUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _buyingItemIcon;




    public void UpdateBuyingItemIcon(Sprite icon)
    {
        _buyingItemIcon.sprite = icon;
    }
}