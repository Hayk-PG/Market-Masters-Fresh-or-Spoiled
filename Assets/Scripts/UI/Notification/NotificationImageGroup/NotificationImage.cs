using UnityEngine;
using UnityEngine.UI;

public class NotificationImage : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _icon;




    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}