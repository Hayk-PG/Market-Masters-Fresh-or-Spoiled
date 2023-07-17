using UnityEngine;
using TMPro;

public class BaseGameResultScreenUIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Animator _animator;

    [Header("UI Elements")]
    [SerializeField] protected TMP_Text _moneyAmountText;




    public virtual void UpdateMoneyAmountText(int amount)
    {
        _moneyAmountText.text = $"${amount}";
    }
}