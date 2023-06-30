using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryHoverInfoManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private PlayerInventoryItemButton _playerInventoryItemButton;

    private object[] _data = new object[1];




    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerInventoryItemHoverInfoDisplay(_playerInventoryItemButton);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TriggerInventoryItemHoverInfoDisplay(null);
    }

    private void TriggerInventoryItemHoverInfoDisplay(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _data[0] = playerInventoryItemButton;
        GameEventHandler.RaiseEvent(GameEventType.InventoryItemHoverInfoDisplay, _data);
    }
}