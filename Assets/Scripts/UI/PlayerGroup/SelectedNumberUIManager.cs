using UnityEngine;
using TMPro;
using Pautik;

public class SelectedNumberUIManager : MonoBehaviour
{
    [Header("Team Group UI")]
    [SerializeField] private TeamGroupPanelManager _teamGroupUIManager;

    [Header("Components")]
    [SerializeField] private PlayerUIGroupManager _playerUIGroupManager;
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private CanvasGroup _numberTextCanvasGroup;
    [SerializeField] private CanvasGroup _iconCanvasGroup;

    private const string _numberTextBounceAnimation = "BounceChoosedNumberTextAnim";

    public int SelectedNumber
    {
        get => int.Parse(_numberText.text);
        private set => _numberText.text = value.ToString();
    }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        EraseNumber(gameEventType, data);
        TryRetrieveDataAndExecute(gameEventType, data);
    }

    private void EraseNumber(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if(SelectedNumber > 0 && (TeamIndex)data[2] == _teamGroupUIManager.TeamIndex)
        {
            UpdateNumber(0);
            SetCanvasGroupActive(_iconCanvasGroup, _numberTextCanvasGroup);
            PlayAnimation(_numberTextBounceAnimation);
        }
    }

    private void TryRetrieveDataAndExecute(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.PublishSellingItemQuantity)
        {
            return;
        }

        if (CanUpdateNumber(ownerName: (string)data[0], ownerActorNumber: (int)data[1])) 
        {
            int number = (int)data[2];

            UpdateTeamCombinedNumber(number);
            UpdateNumber(number);
            SetCanvasGroupActive(_iconCanvasGroup, _numberTextCanvasGroup, false);
            PlayAnimation(_numberTextBounceAnimation);
            PlaySoundEffect(0);
        }
    }

    private bool CanUpdateNumber(string ownerName, int ownerActorNumber)
    {
        return ownerName == _playerUIGroupManager.OwnerName && ownerActorNumber == _playerUIGroupManager.OwnerActorNumber;
    }

    private void UpdateTeamCombinedNumber(int number)
    {
        bool hasTeammateSelectedNumber = _teamGroupUIManager.TeamCombinedSellingItemQuantity > 0;

        _teamGroupUIManager.UpdateTeamCombinedNumber(number);

        if (hasTeammateSelectedNumber)
        {
            GameSceneReferences.Manager.RemoteRPCWrapper.OverrideGameTime(11f);
        }
    }

    private void UpdateNumber(int number)
    {
        SelectedNumber = number;
    }

    private void PlayAnimation(string stateName)
    {
        _animator.Play(stateName, 0, 0);
    }

    private void SetCanvasGroupActive(CanvasGroup firstCanvasGroup, CanvasGroup secondCanvasGroup, bool isActive = true)
    {
        GlobalFunctions.CanvasGroupActivity(firstCanvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(secondCanvasGroup, !isActive);
    }

    private void PlaySoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(3, clipIndex);
    }
}