using UnityEngine;
using System;

[Serializable]
public class ErrorMessageGroup 
{
    [SerializeField] private string[] _errorMessages;
    [SerializeField] private Sprite[] _errorIcons;

    private object[] _data = new object[2];

    public string[] ErrorMessages => _errorMessages;
    public Sprite[] ErrorIcons => _errorIcons;




    public void DisplayErrorMessage(int errorMessageIndex, int errorIconIndex, int soundListIndex, int soundClipIndex)
    {
        _data[0] = _errorMessages[errorMessageIndex];
        _data[1] = _errorIcons[errorIconIndex];
        GameEventHandler.RaiseEvent(GameEventType.DisplayErrorMessage, _data);
        UISoundController.PlaySound(soundListIndex, soundClipIndex);
    }
}