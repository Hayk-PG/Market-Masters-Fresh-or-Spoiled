using UnityEngine;
using Coffee.UIEffects;

public class PlayerInventoryItemButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInventoryItemSpoilUIManager _playerInventoryItemSpoilUIManager;

    [Header("UI Elements")]
    [SerializeField] private Btn _button;
    [SerializeField] private Btn_Icon _icon;

    private Item _item;
    private object[] _buttonData = new object[3];
    private object[] _spoiledItemData = new object[1];

    public Item AssosiatedItem => _item;
    public int ItemSpoilPercentage => _playerInventoryItemSpoilUIManager.ItemSpoilPercentage;




    private void OnEnable()
    {
        _button.OnSelect += OnSelect;
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (AssosiatedItem != null)
        {
            int currentTurnCount = (int)data[3];
            RunLifeTimeCycle(currentTurnCount);
            DestroyItemIfSpoiled();
        }
    }

    public void AssignItem(Item item)
    {
        _item = item;
        _icon.IconSpriteChangedDelegate (item.Icon);
        ResetLifetimeCycle();
    }

    public void RemoveAssosiatedItem()
    {
        _item = null;
        ResetLifetimeCycle();
    }

    private void ResetLifetimeCycle()
    {
        _playerInventoryItemSpoilUIManager.ResetLifetimeCycle(AssosiatedItem);
    }

    private void RunLifeTimeCycle(int currentTurnCount)
    {
        _playerInventoryItemSpoilUIManager.RunLifeTimeCycle(currentTurnCount);     
    }

    private void DestroyItemIfSpoiled()
    {
        if (ItemSpoilPercentage >= 100)
        {
            _spoiledItemData[0] = AssosiatedItem.ID;
            GameEventHandler.RaiseEvent(GameEventType.DestroySpoiledItem, _spoiledItemData);
            _item = null; 
        }
    }

    private void OnSelect()
    {
        UpdateBtnDefaultIcon();
        WrapData();
        SendData();
    }

    public void Deselect()
    {
        _button.Deselect();
    }

    private void UpdateBtnDefaultIcon()
    {
        _icon.ChangeReleasedSpriteDelegate();
    }

    private void WrapData()
    {
        _buttonData[0] = this;    
    }

    private void SendData()
    {
        GameEventHandler.RaiseEvent(GameEventType.SelectInventoryItemForSale, _buttonData);
    }
}