using System.Collections;
using UnityEngine;

public class StorageItemButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Btn _button;

    [Header("UI Elements")]
    [SerializeField] private Btn_Icon _icon;

    private bool _isSelected;
    private object[] _data = new object[1];

    public Item AssosiatedItem { get; private set; }
    public bool HasItem => AssosiatedItem != null;




    private void OnEnable()
    {
        _button.OnPointerUpHandler += OnSelect;
    }

    private void OnSelect()
    {
        SetSelectionState(!_isSelected);

        if (!_isSelected)
        {
            StartCoroutine(DoubleClickToDeselect());
        }

        if (!HasItem)
        {
            return;
        }

        SendItem();
    }

    public void AssignItem(Item item)
    {
        AssosiatedItem = item;
        ChangeIcon(AssosiatedItem.Icon);
    }

    public void RemoveItem(Sprite sprite)
    {
        AssosiatedItem = null;
        ChangeIcon(sprite);
    }

    public void BlockCell(Sprite sprite)
    {
        ChangeIcon(sprite);
    }

    public void Deselect()
    {
        _button.Deselect();
        SetSelectionState(false);
    }

    private void SendItem()
    {
        _data[0] = this;
        GameEventHandler.RaiseEvent(GameEventType.SelectStorageItem, _data);
    }

    private IEnumerator DoubleClickToDeselect()
    {
        yield return new WaitForSeconds(0.1f);
        _button.Deselect();
    }

    private void ChangeIcon(Sprite sprite)
    {
        _icon.IconSpriteChangeDelegate(sprite);
        _icon.ChangeReleasedSpriteDelegate();
    }

    private void SetSelectionState(bool isSelected)
    {
        _isSelected = isSelected;
    }
}