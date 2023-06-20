using UnityEngine;
using TMPro;

public class TimerUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _timeText;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    private const string _timerTextAnim = "TimerTextAnim";
    private const string _shakeHourglassIcon = "HourglassShakeAnim";




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateTimeText(gameEventType, data);
        PlayHourglassShakeAnimation(gameEventType);
    }

    private void UpdateTimeText(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        float time = (float)data[0];
        _timeText.text = time.ToString();
        PlayAnimation(_timerTextAnim);
        PlayRoundChangeSoundEffect(time % 2 == 0 ? 1 : 2, true);
    }

    private void PlayHourglassShakeAnimation(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        PlayAnimation(_shakeHourglassIcon, true);
        PlayRoundChangeSoundEffect(0, true);
    }

    private void PlayAnimation(string stateName, bool canPlayAnimation = true)
    {
        if (!canPlayAnimation)
        {
            return;
        }

        _animator.Play(stateName, 0, 0);
    }

    private void PlayRoundChangeSoundEffect(int clipIndex, bool canPlay = true)
    {
        if (!canPlay)
        {
            return;
        }

        UISoundController.PlaySound(2, clipIndex);
    }
}