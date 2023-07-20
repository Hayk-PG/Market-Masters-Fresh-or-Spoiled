using UnityEngine;

public class PlayerInventoryItemButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInventoryItemSpoilUIManager _playerInventoryItemSpoilUIManager;

    [Header("UI Elements")]
    [SerializeField] private Btn _button;
    [SerializeField] private Btn_Icon _icon;

    [Header("Assosiated Item")]
    [SerializeField] private Item _item;

    [Header("Particle")]
    [SerializeField] private ParticleSystem _itemSellParticle;

    private object[] _buttonData = new object[3];
    private object[] _spoiledItemData = new object[1];

    /// <summary>
    /// The associated item of the button.
    /// </summary>
    public Item AssociatedItem => _item;

    /// <summary>
    /// The spoil percentage of the associated item.
    /// </summary>
    public int ItemSpoilPercentage => _playerInventoryItemSpoilUIManager.ItemSpoilPercentage;

    /// <summary>
    /// The lifetime of the associated item.
    /// </summary>
    public int ItemLifetime
    {
        get => _playerInventoryItemSpoilUIManager.Lifetime;
        set => _playerInventoryItemSpoilUIManager.Lifetime = value;
    }




    private void OnEnable()
    {
        _button.OnSelect += OnSelect;
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles the game event related to the player's game turn.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (_item != null)
        {
            int currentTurnCount = (int)data[3];
            RunLifeTimeCycle(currentTurnCount);
            DestroyItemIfSpoiled();
        }
    }

    /// <summary>
    /// Assigns an item to the button.
    /// </summary>
    /// <param name="item">The item to be assigned.</param>
    public void AssignItem(Item item)
    {
        UpdateItemAndIcon(item);
        ResetLifetimeCycle();
    }

    /// <summary>
    /// Assigns an item to the button with a specified lifetime.
    /// </summary>
    /// <param name="item">The item to be assigned.</param>
    /// <param name="newLifetime">The new lifetime value for the item.</param>
    public void AssignItem(Item item, int newLifetime)
    {
        UpdateItemAndIcon(item);
        _playerInventoryItemSpoilUIManager.ContinueLifetimeCycle(newLifetime);
    }

    /// <summary>
    /// Removes the associated item from the button.
    /// </summary>
    public void RemoveAssociatedItem()
    {
        _item = null;
        ResetLifetimeCycle();
    }

    /// <summary>
    /// Destroys the spoiled item on separate sale.
    /// </summary>
    public void DestroySpoiledItemOnSeparateSale()
    {
        _playerInventoryItemSpoilUIManager.ResetSpoilageOnSeparateSale();
        DestroyItemIfSpoiled();
    }

    public void PlayItemSellParticle()
    {
        _itemSellParticle.Play(true);
    }

    /// <summary>
    /// Updates the associated item and the icon.
    /// </summary>
    /// <param name="item">The item to be updated.</param>
    private void UpdateItemAndIcon(Item item)
    {
        _item = item;
        _icon.IconSpriteChangeDelegate(item.Icon);
        _icon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Resets the lifetime cycle for the associated item
    /// </summary>
    private void ResetLifetimeCycle()
    {
        _playerInventoryItemSpoilUIManager.ResetLifetimeCycle(_item);
    }

    /// <summary>
    /// Runs the lifetime cycle for the associated item.
    /// </summary>
    /// <param name="currentTurnCount">The current turn count.</param>
    private void RunLifeTimeCycle(int currentTurnCount)
    {
        _playerInventoryItemSpoilUIManager.RunLifeTimeCycle(currentTurnCount);     
    }

    /// <summary>
    /// Destroys the associated item if it is spoiled.
    /// </summary>
    private void DestroyItemIfSpoiled()
    {
        if (ItemSpoilPercentage >= 100)
        {
            _spoiledItemData[0] = _item.ID;
            GameEventHandler.RaiseEvent(GameEventType.DestroySpoiledItem, _spoiledItemData);
            _item = null; 
        }
    }

    /// <summary>
    /// Handles the button select event.
    /// </summary>
    private void OnSelect()
    {
        UpdateBtnDefaultIcon();
        WrapData();
        SendData();
    }

    /// <summary>
    /// Deselects the button.
    /// </summary>
    public void Deselect()
    {
        _button.Deselect();
    }

    /// <summary>
    /// Updates the button's default icon.
    /// </summary>
    private void UpdateBtnDefaultIcon()
    {
        _icon.ChangeReleasedSpriteDelegate();
    }

    /// <summary>
    /// Wraps the data to be sent with the button event.
    /// </summary>
    private void WrapData()
    {
        _buttonData[0] = this;    
    }

    /// <summary>
    /// Sends the data with the button event.
    /// </summary>
    private void SendData()
    {
        GameEventHandler.RaiseEvent(GameEventType.SelectInventoryItemForSale, _buttonData);
    }
}