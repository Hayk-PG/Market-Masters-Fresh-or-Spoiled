using TMPro;
using UnityEngine;

public class MainNumberUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _numberText;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    private const string _updateMainNumberAnim = "UpdateMainNumberAnim";

    public int Number
    {
        get => int.Parse(_numberText.text);
        private set => _numberText.text = value.ToString();
    }




    private void Start()
    {
        Number = 0;
    }

    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateNumber(gameEventType, data);
    }

    private void UpdateNumber(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.PublishTeamCombinedNumber)
        {
            return;
        }

        Number += (int)data[0];
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