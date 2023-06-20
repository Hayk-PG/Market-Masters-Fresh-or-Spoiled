using System.Collections;
using UnityEngine;

public class SideButtonsPopUpEffectController : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator[] _buttonsAnimators;

    private const string _animationClip = "Button Bounce Anim";




    private void Start()
    {
        StartCoroutine(PlayPopUpAnimation());
    }

    private IEnumerator PlayPopUpAnimation()
    {
        int index = 0;

        if(_buttonsAnimators == null || _buttonsAnimators.Length < 1)
        {
            yield break;
        }

        while(index < _buttonsAnimators.Length)
        {
            _buttonsAnimators[index].Play(_animationClip, 0, 0);
            UISoundController.PlaySound(1, 0);

            index++;
            yield return new WaitForSeconds(0.15f);
        }
    }
}