using UnityEngine;
using System;

[Serializable]
public class ErrorMessageGroup 
{
    [SerializeField] private string[] _errorMessages;
    [SerializeField] private Sprite[] _errorIcons;

    private object[] _data = new object[2];

    public string[] ErrorMessages { get => _errorMessages; set => _errorMessages = value; }
    public Sprite[] ErrorIcons { get => _errorIcons; set => _errorIcons = value; }




    public void DisplayErrorMessage(int errorMessageIndex, int errorIconIndex)
    {
        _data[0] = _errorMessages[errorMessageIndex];
        _data[1] = _errorIcons[errorIconIndex];
        GameEventHandler.RaiseEvent(GameEventType.DisplayErrorMessage, _data);
    }
}