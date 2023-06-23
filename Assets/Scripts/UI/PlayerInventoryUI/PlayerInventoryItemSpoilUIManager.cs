using UnityEngine;
using Coffee.UIEffects;

public class PlayerInventoryItemSpoilUIManager : MonoBehaviour
{
    [Header("Disolve Effect")]
    [SerializeField] private UIDissolve _iconDisolve;

    private int _lifetime;
    private int _currentLifetimeCycle;

    public int ItemSpoilPercentage { get; private set; }




    public void ResetLifetimeCycle(Item item)
    {
        AssignLifetime(item);
        SetDisolveEffectFactor(0f);
        SetItemSpoilPercentage(0);
    }

    public void RunLifeTimeCycle(int currentTurnCount)
    {
        if (currentTurnCount < _lifetime)
        {
            return;
        }

        AssignCurrentLifetimeCycle(value: currentTurnCount - _lifetime);
        SetDisolveEffectFactor(value: Mathf.InverseLerp(0, 10, _currentLifetimeCycle));
        SetItemSpoilPercentage(value: Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, _iconDisolve.effectFactor) * 100f));
        print($"{ItemSpoilPercentage}");
    }

    private void AssignLifetime(Item item)
    {
        if(item == null)
        {
            return;
        }

        _lifetime = GameSceneReferences.Manager.GameTurnManager.TurnCount + ItemSpoilageFormula.SpoilageLevel(item.ItemDurabilityLevel);
    }

    private void AssignCurrentLifetimeCycle(int value)
    {
        _currentLifetimeCycle = value;
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