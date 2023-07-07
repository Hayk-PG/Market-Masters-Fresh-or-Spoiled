using UnityEngine;
using UnityEngine.UI;
using Pautik;

public class RecycleUIManager : InventoryItemDragDropUIResponder
{
    [Header("Sprites For Icon")]
    [SerializeField] private Sprite _iconClosedRecycleBinSprite;
    [SerializeField] private Sprite _iconOpenedRecycleBinSprite;

    [Header("UI Elements")]
    [SerializeField] private Btn_Icon _icon;

    private Image _iconImage;
    private int _recyclingStartTurnCount = 0;
    private int _availableRecyclingTurnCount = 0;
    
    private bool HasItem => _inventoryItemButton != null;
    private bool CanRecycle => GameSceneReferences.Manager.GameTurnManager.TurnCount >= _availableRecyclingTurnCount || GameSceneReferences.Manager.GameTurnManager.TurnCount <= _recyclingStartTurnCount;




    private void Awake()
    {
        _iconImage = Get<Image>.From(_icon.gameObject);
    }

    protected override void ExecuteOnDragRelease(object[] data)
    {
        RecycleItem();
        base.ExecuteOnDragRelease(data);
    }

    protected override void SetButtonSprites(Sprite sprite)
    {
        base.SetButtonSprites(sprite);

        if (!CanRecycle && _iconImage.sprite == _iconClosedRecycleBinSprite)
        {
            return;
        }

        SetIconSprites(sprite);
    }

    private void SetIconSprites(Sprite buttonSprite)
    {
        _icon.IconSpriteChangeDelegate(buttonSprite == _defaultSprite ? _iconClosedRecycleBinSprite : _iconOpenedRecycleBinSprite);
        _icon.ChangeReleasedSpriteDelegate();
    }

    private void RecycleItem()
    {
        if (HasItem && CanRecycle)
        {
            DefineRecyclingStartTurnCount();
            DefineAvailableRecyclingTurnCount();
            DestroyItemFromInventory();
            PlaySoundEffect(1, 4);
        }
        else
        {
            PlaySoundEffect(4, 1);
        }
    }

    private void DefineRecyclingStartTurnCount()
    {
        _recyclingStartTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount;
    }

    private void DefineAvailableRecyclingTurnCount()
    {
        _availableRecyclingTurnCount = GameSceneReferences.Manager.GameTurnManager.TurnCount + 5;
    }

    private void DestroyItemFromInventory()
    {
        _inventoryItemButton.DestroySpoiledItemOnSeparateSale();
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}