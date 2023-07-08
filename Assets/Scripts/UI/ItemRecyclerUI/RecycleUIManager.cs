using UnityEngine;
using UnityEngine.UI;
using Pautik;

public class RecycleUIManager : InventoryItemDragDropUIResponder
{
    [Header("Error Message")]
    [SerializeField] private ErrorMessageGroup _errorMessageGroup;

    private int _recyclingStartTurnCount = 0;
    private int _availableRecyclingTurnCount = 0;
    
    private bool HasItem => _inventoryItemButton != null;
    private bool CanRecycle => GameSceneReferences.Manager.GameTurnManager.TurnCount >= _availableRecyclingTurnCount || GameSceneReferences.Manager.GameTurnManager.TurnCount <= _recyclingStartTurnCount;




    protected override void ExecuteOnDragRelease(object[] data)
    {
        RecycleItem();
        base.ExecuteOnDragRelease(data);
    }

    protected override void ExecuteOnHover(object[] data)
    {
        base.ExecuteOnHover(data);
        ToggleItemStoringDisplay(sprite: HasItem && CanRecycle ? _iconSprites[0] : _iconSprites[1]);
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
            DisplayError();
        }
    }

    private void DisplayError()
    {
        int availableData = _availableRecyclingTurnCount - GameSceneReferences.Manager.GameTurnManager.TurnCount;
        _errorMessageGroup.ErrorMessages[0] = GlobalFunctions.PartiallyTransparentText("Apologies for the inconvenience, but recycling services for items are currently unavailable due to a machine jam. Our team is actively working to resolve the issue. Please try again in ") +
                                              GlobalFunctions.WhiteColorText(availableData.ToString()) + GlobalFunctions.WhiteColorText(availableData <= 1 ? " day." : " days.") +
                                              GlobalFunctions.PartiallyTransparentText(" Thank you for your understanding.");
        _errorMessageGroup.DisplayErrorMessage(0, 0);
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