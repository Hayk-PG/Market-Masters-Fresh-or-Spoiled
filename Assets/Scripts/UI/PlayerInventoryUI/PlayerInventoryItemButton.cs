using UnityEngine;

public class PlayerInventoryItemButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Btn _button;
    [SerializeField] private Btn_Icon _icon;

    private Item _item;
    private object[] _buttonData = new object[3];

    public Btn Button => _button;




    private void OnEnable()
    {
        _button.OnSelect += OnSelect;
    }

    public void AssignItem(Item item)
    {
        _item = item;
        _icon.ChangeIconHolder(item.Icon);
    }

    private void OnSelect()
    {
        WrapData();
        SendData();
    }

    public void Deselect()
    {
        _button.Deselect();
    }

    private void WrapData()
    {
        _buttonData[0] = this;    
    }

    private void SendData()
    {
        GameEventHandler.RaiseEvent(GameEventType.SelectInventoryItem, _buttonData);
    }
}