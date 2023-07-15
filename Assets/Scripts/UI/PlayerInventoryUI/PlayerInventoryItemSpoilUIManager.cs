using UnityEngine;
using Coffee.UIEffects;

public class PlayerInventoryItemSpoilUIManager : MonoBehaviour
{
    [Header("Disolve Effect")]
    [SerializeField] private UIDissolve _iconDisolve;

    private int CurrentLifetimeCycle { get; set; }
    public int ItemSpoilPercentage { get; private set; }
    public int Lifetime { get; set; }




    /// <summary>
    /// Resets the lifetime cycle, spoilage percentage, and disolve effect for the item.
    /// </summary>
    /// <param name="item">The item to reset the spoilage for.</param>
    public void ResetLifetimeCycle(Item item)
    {
        AssignLifetime(item);
        SetDisolveEffectFactor(0f);
        SetItemSpoilPercentage(0);
    }

    /// <summary>
    /// Continues the lifetime cycle for the item with a new lifetime value.
    /// </summary>
    /// <param name="newLifetime">The new lifetime value for the item.</param>
    public void ContinueLifetimeCycle(int newLifetime)
    {
        Lifetime = newLifetime;
        AssignCurrentLifetimeCycle(value: GameSceneReferences.Manager.GameTurnManager.TurnCount - Lifetime);
        SetDisolveEffectFactor(value: Mathf.InverseLerp(0, 10, CurrentLifetimeCycle));
        SetItemSpoilPercentage(value: Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, _iconDisolve.effectFactor) * 100f));
    }

    /// <summary>
    /// Runs the lifetime cycle for the item based on the current turn count.
    /// </summary>
    /// <param name="currentTurnCount">The current turn count.</param>
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

    /// <summary>
    /// Assigns the lifetime value for the given item.
    /// </summary>
    /// <param name="item">The item to assign the lifetime for.</param>
    private void AssignLifetime(Item item)
    {
        if(item == null)
        {
            return;
        }

        Lifetime = GameSceneReferences.Manager.GameTurnManager.TurnCount + ItemSpoilageFormula.SpoilageLevel(item.ItemDurabilityLevel);
    }

    /// <summary>
    /// Assigns the current lifetime cycle value.
    /// </summary>
    /// <param name="value">The value to assign.</param>
    private void AssignCurrentLifetimeCycle(int value)
    {
        CurrentLifetimeCycle = value;
    }

    /// <summary>
    /// Sets the dissolve effect factor.
    /// </summary>
    /// <param name="value">The value to set.</param>
    private void SetDisolveEffectFactor(float value)
    {
        _iconDisolve.effectFactor = value;
    }

    /// <summary>
    /// Sets the item spoil percentage.
    /// </summary>
    /// <param name="value">The value to set.</param>
    private void SetItemSpoilPercentage(int value)
    {
        ItemSpoilPercentage = value;        
    }
}