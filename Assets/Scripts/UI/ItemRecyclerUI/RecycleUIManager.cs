using UnityEngine;

public class RecycleUIManager : InventoryItemDragDropUIResponder
{
    private int _recyclingStartTurnCount = 0;
    private int _availableRecyclingTurnCount = 0;




    protected override void ExecuteOnDragRelease(object[] data)
    {
        RecycleItem();
        base.ExecuteOnDragRelease(data);
    }

    protected override void SetButtonSprites(Sprite sprite)
    {
        
    }

    private void RecycleItem()
    {
        bool hasItem = _inventoryItemButton != null;
        bool canRecycle = GameSceneReferences.Manager.GameTurnManager.TurnCount >= _availableRecyclingTurnCount ||
                          GameSceneReferences.Manager.GameTurnManager.TurnCount <= _recyclingStartTurnCount;

        if (hasItem && canRecycle)
        {
            DefineRecyclingStartTurnCount();
            DefineAvailableRecyclingTurnCount();
            DestroyItemFromInventory();
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
}