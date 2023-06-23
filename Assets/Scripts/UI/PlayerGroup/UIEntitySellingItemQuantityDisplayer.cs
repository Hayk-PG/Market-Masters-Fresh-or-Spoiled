using UnityEngine;
using TMPro;
using Pautik;

public class UIEntitySellingItemQuantityDisplayer : MonoBehaviour
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

    private int SelectedNumber
    {
        get => int.Parse(_numberText.text);
        set => _numberText.text = value.ToString();
    }




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handle the game events and execute corresponding actions.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        EraseNumber(gameEventType, data);
        TryRetrieveDataAndExecute(gameEventType, data);
    }

    /// <summary>
    /// Erase the number when the game turn is updated.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void EraseNumber(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        bool isValidSelection = SelectedNumber > 0 && (TeamIndex)data[2] == _teamGroupUIManager.TeamIndex;

        if (!isValidSelection)
        {
            return;
        }

        UpdateNumber(0);
        SetCanvasGroupActive(_iconCanvasGroup, _numberTextCanvasGroup);
        PlayAnimation(_numberTextBounceAnimation);
    }

    /// <summary>
    /// Try to retrieve data and execute actions based on the game event.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void TryRetrieveDataAndExecute(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.PublishSellingItemQuantity)
        {
            return;
        }

        if (CanUpdateNumber(ownerName: (string)data[0], ownerActorNumber: (int)data[1])) 
        {
            byte itemQuantity = (byte)data[2];
            byte itemSpoilPercentage = (byte)data[4];

            UpdateTeamCombinedNumber(itemQuantity, itemSpoilPercentage);
            UpdateNumber(itemQuantity);
            SetCanvasGroupActive(_iconCanvasGroup, _numberTextCanvasGroup, false);
            PlayAnimation(_numberTextBounceAnimation);
            PlaySoundEffect(0);
        }
    }

    /// <summary>
    /// Check if the number can be updated based on the owner's name and actor number.
    /// </summary>
    /// <param name="ownerName">The name of the owner.</param>
    /// <param name="ownerActorNumber">The actor number of the owner.</param>
    /// <returns><c>true</c> if the number can be updated, otherwise <c>false</c>.</returns>
    private bool CanUpdateNumber(string ownerName, int ownerActorNumber)
    {
        return ownerName == _playerUIGroupManager.OwnerName && ownerActorNumber == _playerUIGroupManager.OwnerActorNumber;
    }

    /// <summary>
    /// Update the team's combined number and perform additional actions if there is a teammate with a selected number.
    /// </summary>
    /// <param name="itemQuantity">The quantity of the item being sold.</param>
    /// <param name="itemSpoilPercentage">The spoilage percentage of the item being sold.</param>
    private void UpdateTeamCombinedNumber(int itemQuantity, int itemSpoilPercentage)
    {
        bool hasTeammateSelectedNumber = _teamGroupUIManager.TeamCombinedSellingItemQuantity > 0;

        _teamGroupUIManager.GetTeamCombinedSellingItemData(itemQuantity, itemSpoilPercentage);

        if (hasTeammateSelectedNumber)
        {
            GameSceneReferences.Manager.RemoteRPCWrapper.OverrideGameTime(11f);
        }
    }

    /// <summary>
    /// Update the displayed number.
    /// </summary>
    /// <param name="number">The new number to be displayed.</param>
    private void UpdateNumber(int number)
    {
        SelectedNumber = number;
    }

    /// <summary>
    /// Play the specified animation state.
    /// </summary>
    /// <param name="stateName">The name of the animation state to play.</param>
    private void PlayAnimation(string stateName)
    {
        _animator.Play(stateName, 0, 0);
    }

    /// <summary>
    /// Set the activity of the specified canvas groups.
    /// </summary>
    /// <param name="firstCanvasGroup">The first canvas group.</param>
    /// <param name="secondCanvasGroup">The second canvas group.</param>
    /// <param name="isActive">The activity state to set (default is <c>true</c>).</param>
    private void SetCanvasGroupActive(CanvasGroup firstCanvasGroup, CanvasGroup secondCanvasGroup, bool isActive = true)
    {
        GlobalFunctions.CanvasGroupActivity(firstCanvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(secondCanvasGroup, !isActive);
    }

    /// <summary>
    /// Play a sound effect based on the specified clip index.
    /// </summary>
    /// <param name="clipIndex">The index of the clip to play.</param>
    private void PlaySoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(3, clipIndex);
    }
}