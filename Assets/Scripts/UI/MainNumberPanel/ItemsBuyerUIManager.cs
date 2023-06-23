using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsBuyerUIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Image _buyingItemIcon;
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField] private TMP_Text _moneyAmountText;

    private const string _updateMainNumberAnim = "UpdateMainNumberAnim";




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    public void UpdateBuyingItemData(Sprite icon, float percentage, int moneyAmount)
    {
        _buyingItemIcon.sprite = icon;
        _percentageText.text = percentage < 100 ? $"-{percentage}%" : percentage > 100 ? $"+{percentage}%" : $"{percentage}";
        _moneyAmountText.text = $"${moneyAmount}";
        print($"Money Amoun: {moneyAmount}");
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        PlayAnimationAndSoundEffects(gameEventType, data);
    }

    private void PlayAnimationAndSoundEffects(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.PublishTeamCombinedSellingItemQuantity)
        {
            return;
        }

        print($"Publish Team Combined Selling Item Quantity");
        PlayAnimation(_updateMainNumberAnim);
        PlaySoundEffect();
    }

    private void PlayAnimation(string animationState)
    {
        _animator.Play(animationState, 0, 0);
    }

    private void PlaySoundEffect()
    {
        UISoundController.PlaySound(3, 1);
    }
}