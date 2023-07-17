using System.Collections;
using UnityEngine;

public class VictoryScreenUIManager : BaseGameResultScreenUIManager
{
    [Header("Victory Frame")]
    [SerializeField] private GameObject _victoryFrame;

    [Header("Confetti")]
    [SerializeField] private GameObject _confetti;

    private const string _animationClipName = "VictoryScreenAnim";




    private void Start()
    {
        PlaySoundEffect(2);
        StartCoroutine(PopupVictoryFrameAfterDelay());
    }

    public void PlayVictoryScreenAnimation()
    {
        _animator.Play(_animationClipName, 0, 0);
        _confetti.gameObject.SetActive(true);
        PlaySoundEffect(0);
        PlaySoundEffect(3);
    }

    private IEnumerator PopupVictoryFrameAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        _victoryFrame.gameObject.SetActive(true);
        PlaySoundEffect(1);
    }

    private void PlaySoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(11, clipIndex);
    }
}