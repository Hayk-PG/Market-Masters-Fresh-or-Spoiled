using UnityEngine;

public class NumericButtonsPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Btn[] _buttons;

    private object[] _data = new object[1];
    private int _selectedNumber = 1;




    private void OnEnable()
    {
        _buttons[0].OnSelect += ()=> OnSelect(1);
        _buttons[1].OnSelect += () => OnSelect(2);
        _buttons[2].OnSelect += () => OnSelect(3);
        _buttons[3].OnSelect += () => OnSelect(4);
        _buttons[4].OnSelect += () => OnSelect(5);
        _buttons[5].OnSelect += () => OnSelect(6);
        _buttons[6].OnSelect += () => OnSelect(0);
    }

    private void OnSelect(int number = 0)
    {
        bool isConfirmButtonSelected = number == 0;

        if (isConfirmButtonSelected)
        {
            PlayClickSoundEffect(11);
            ConfirmSelectedNumber();
            DeselectButtons(_buttons[6]);
            return;
        }

        PlayClickSoundEffect(9);
        SetSelectedNumber(number);
        DeselectButtons(_buttons[number - 1]);
    }

    private void ConfirmSelectedNumber()
    {
        _data[0] = _selectedNumber;
        GameEventHandler.RaiseEvent(GameEventType.ConfirmSelectedNumber, _data);
    }

    private void SetSelectedNumber(int number)
    {
        _selectedNumber = number;
    }

    private void DeselectButtons(Btn btn = null)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if(_buttons[i] == btn)
            {
                continue;
            }

            _buttons[i].Deselect();
        }
    }

    private void PlayClickSoundEffect(int clipIndex)
    {
        UISoundController.PlaySound(0, clipIndex);
    }
}