using System.Collections;
using UnityEngine;

public class DefeatScreenUIManager : BaseGameResultScreenUIManager
{
    private const string _animationClipName = "YouLoseFrameAnim";




    private void Start()
    {
        PlaySoundEffect(0);
        StartCoroutine(StartAnimationAfterDelay());
    }

    private IEnumerator StartAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.Play(_animationClipName, 0, 0);
    }

    // Animation callback
    private void PlayYouLosePopUpSoundEffect()
    {
        PlaySoundEffect(1);
    }

    // Animation callback
    private void PlayGameOverSoundEffect()
    {
        PlaySoundEffect(2);
    }

    private void PlaySoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(12, clipIndex);
    }
}