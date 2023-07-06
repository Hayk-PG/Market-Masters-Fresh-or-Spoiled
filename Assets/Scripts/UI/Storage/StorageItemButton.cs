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

    public StorageItem? AssociatedStorageItem { get; private set; }
    public bool HasStorageItem => AssociatedStorageItem.HasValue;




    private void OnEnable()
    {
        _button.OnPointerUpHandler += OnSelect;
    }

    /// <summary>
    /// Handles the selection/deselection of the storage item button.
    /// </summary>
    private void OnSelect()
    {
        SetSelectionState(!_isSelected);
        PlaySoundEffect();

        if (!_isSelected)
        {
            StartCoroutine(DoubleClickToDeselect());
        }

        if (!HasStorageItem)
        {
            return;
        }

        SendItem();
    }

    /// <summary>
    /// Assigns a storage item to the button and changes its icon.
    /// </summary>
    /// <param name="storageItem">The storage item to assign.</param>
    public void AssignStorageItem(StorageItem storageItem)
    {
        AssociatedStorageItem = storageItem;
        ChangeIcon(storageItem.AssociatedItem.Icon);
    }

    /// <summary>
    /// Removes the storage item and changes the icon to the specified sprite.
    /// </summary>
    /// <param name="sprite">The sprite to use as the new icon.</param>
    public void RemoveItem(Sprite sprite)
    {
        AssociatedStorageItem = null;
        ChangeIcon(sprite);
    }

    /// <summary>
    /// Blocks the storage cell and changes the icon to the specified sprite.
    /// </summary>
    /// <param name="sprite">The sprite to use as the blocked icon.</param>
    public void BlockCell(Sprite sprite)
    {
        ChangeIcon(sprite);
    }

    /// <summary>
    /// Deselects the storage item button.
    /// </summary>
    public void Deselect()
    {
        _button.Deselect();
        SetSelectionState(false);
    }

    /// <summary>
    /// Sends the storage item as data in a game event.
    /// </summary>
    private void SendItem()
    {
        _data[0] = this;
        GameEventHandler.RaiseEvent(GameEventType.SelectStorageItem, _data);
    }

    /// <summary>
    /// Coroutine for double-click behavior to deselect the button.
    /// </summary>
    private IEnumerator DoubleClickToDeselect()
    {
        yield return new WaitForSeconds(0.1f);
        _button.Deselect();
    }

    /// <summary>
    /// Changes the icon of the storage item button to the specified sprite.
    /// </summary>
    /// <param name="sprite">The sprite to use as the new icon.</param>
    private void ChangeIcon(Sprite sprite)
    {
        _icon.IconSpriteChangeDelegate(sprite);
        _icon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Sets the selection state of the storage item button.
    /// </summary>
    /// <param name="isSelected">The selection state to set.</param>
    private void SetSelectionState(bool isSelected)
    {
        _isSelected = isSelected;
    }

    /// <summary>
    /// Plays a sound effect based on the selection state.
    /// </summary>
    private void PlaySoundEffect()
    {
        UISoundController.PlaySound(0, _isSelected ? 1 : 2);
    }
}