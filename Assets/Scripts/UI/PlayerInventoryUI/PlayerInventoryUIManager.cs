using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private PlayerInventoryItemButton[] _inventoryItemButtons;
    [SerializeField] private Btn _confirmButton;

    [Header("Components")]
    [SerializeField] private Animator _animator;

    private string _errorAnimation = "ErrorAnim";

    private List<PlayerInventoryItemButton> _selectedItemButtonsList = new List<PlayerInventoryItemButton>();




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _confirmButton.OnSelect += OnConfirmButtonSelect;
    }

    public void AssignInvetoryItem(int inventoryItemButtonIndex, Item item)
    {
        _inventoryItemButtons[inventoryItemButtonIndex].AssignItem(item);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SelectInventoryItem)
        {
            return;
        }

        PlayClickSoundEffect(9);
        UpdateSelectedItemButtonsList(playerInventoryItemButton: (PlayerInventoryItemButton)data[0]);
    }

    private void OnConfirmButtonSelect()
    {
        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            return;
        }

        foreach (var itemButton in _selectedItemButtonsList)
        {
            itemButton.Deselect();
        }

        _confirmButton.Deselect();
        DismissConfirmation();
    }

    private void UpdateSelectedItemButtonsList(PlayerInventoryItemButton playerInventoryItemButton, bool isRemoving = false)
    {
        if (!isRemoving)
        {
            _selectedItemButtonsList.Add(playerInventoryItemButton);
            return;
        }

        bool isListEmpty = _selectedItemButtonsList.Count == 0;

        if (isListEmpty)
        {
            return;
        }

        _selectedItemButtonsList.Remove(playerInventoryItemButton);
    }

    private void DismissConfirmation()
    {
        _animator.Play(_errorAnimation, 0, 0);
        UISoundController.PlaySound(4, 0);
    }

    private void DeselectButtons(Btn button = null)
    {
        for (int i = 0; i < _inventoryItemButtons.Length; i++)
        {
            if (_inventoryItemButtons[i] == button)
            {
                continue;
            }

            _inventoryItemButtons[i].Button.Deselect();
        }
    }

    private void OnSelect(int number = 0)
    {
        //bool isConfirmButtonSelected = number == 0;

        //if (isConfirmButtonSelected)
        //{
        //    PlayClickSoundEffect(11);
        //    ConfirmSelectedNumber();
        //    DeselectButtons(_inventoryItemButtons[8]);
        //    return;
        //}

        //PlayClickSoundEffect(9);
        //SetSelectedNumber(number);
        //DeselectButtons(_inventoryItemButtons[number - 1]);
    }

    //private void ConfirmSelectedNumber()
    //{
    //    _data[0] = _selectedNumber;
    //    GameEventHandler.RaiseEvent(GameEventType.ConfirmSelectedNumber, _data);
    //}

    //private void SetSelectedNumber(int number)
    //{
    //    _selectedNumber = number;
    //}

    

    private void PlayClickSoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(0, clipIndex);
    }
}