using UnityEngine;
using Coffee.UIEffects;

public class PlayerInventoryItemSpoilUIManager : MonoBehaviour
{
    [Header("Disolve Effect")]
    [SerializeField] private UIDissolve _iconDisolve;

    private int CurrentLifetimeCycle { get; set; }
    public int ItemSpoilPercentage { get; private set; }
    public int Lifetime { get; set; }




    public void ResetLifetimeCycle(Item item)
    {
        AssignLifetime(item);
        SetDisolveEffectFactor(0f);
        SetItemSpoilPercentage(0);
    }

    public void ContinueLifetimeCycle(int savedLifetime)
    {
        Lifetime += savedLifetime;
        AssignCurrentLifetimeCycle(value: GameSceneReferences.Manager.GameTurnManager.TurnCount - Lifetime);
        SetDisolveEffectFactor(value: Mathf.InverseLerp(0, 10, CurrentLifetimeCycle));
        SetItemSpoilPercentage(value: Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, _iconDisolve.effectFactor) * 100f));
    }

    public void RunLifeTimeCycle(int currentTurnCount)
    {
        if (currentTurnCount < Lifetime)
        {
            return;
        }

        AssignCurrentLifetimeCycle(value: currentTurnCount - Lifetime);
        SetDisolveEffectFactor(value: Mathf.InverseLerp(0, 10, CurrentLifetimeCycle));
        SetItemSpoilPercentage(value: Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, _iconDisolve.effectFactor) * 100f));
    }

    /// <summary>
    /// Resets the spoilage of the item to its maximum level and other relevant values when the item is being sold separately.
    /// </summary>
    public void ResetSpoilageOnSeparateSale()
    {
        SetItemSpoilPercentage(100);
        SetDisolveEffectFactor(1);
        Lifetime = 0;
        CurrentLifetimeCycle = 0;
    }

    private void AssignLifetime(Item item)
    {
        if(item == null)
        {
            return;
        }

        Lifetime = GameSceneReferences.Manager.GameTurnManager.TurnCount + ItemSpoilageFormula.SpoilageLevel(item.ItemDurabilityLevel);
    }

    private void AssignCurrentLifetimeCycle(int value)
    {
        CurrentLifetimeCycle = value;
    }

    private void SetDisolveEffectFactor(float value)
    {
        _iconDisolve.effectFactor = value;
    }

    private void SetItemSpoilPercentage(int value)
    {
        ItemSpoilPercentage = value;        
    }
}