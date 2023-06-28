using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Coffee.UIEffects;
using Pautik;

public class ItemsBuyerUIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Image _buyingItemIcon;
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField] private TMP_Text _moneyAmountText;
    [SerializeField] private UIHsvModifier _uiHsModifier;

    private const string _updateMainNumberAnim = "UpdateMainNumberAnim";




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    public void UpdateBuyingItemData(Sprite icon, float percentage, int moneyAmount)
    {
        _buyingItemIcon.sprite = icon;
        _percentageText.text = percentage < 100 ? $"-{percentage}%" : percentage > 100 ? $"+{percentage}%" : $"{percentage}%";
        _moneyAmountText.text = $"${moneyAmount}";

        ChangePercentageIdicatorColor(percentage);
    }

    private void ChangePercentageIdicatorColor(float percentage)
    {
        Conditions<float>.Compare(percentage, 100f, () => SetUIHsModifierValues(0.05f), () => SetUIHsModifierValues(0.286f, 0f, -0.069f), () => SetUIHsModifierValues(0.03f));
    }

    private void SetUIHsModifierValues(float hue, float saturation = 0f, float value = 0f)
    {
        _uiHsModifier.hue = hue;
        _uiHsModifier.saturation = saturation;
        _uiHsModifier.value = value;
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