using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pautik;

public class NotificationManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private Btn _denyButton;
    [SerializeField] private Btn _acceptButton;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;

    private object[] _data = new object[1];




    private void OnEnable()
    {
        _denyButton.OnSelect += OnDeny;
        _acceptButton.OnSelect += OnAccept;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Open();
        }
    }

    private void OnAccept()
    {
        _data[0] = 40;
        GameEventHandler.RaiseEvent(GameEventType.SellSpoiledItems, _data);
        PlaySoundEffect(0, 11);
        Close();
    }

    private void OnDeny()
    {
        Close();
        PlaySoundEffect(4, 2);
    }

    private void Open()
    {
        PlaySoundEffect(7, 0);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        _denyButton.Deselect();
        _acceptButton.Deselect();
    }

    private void Close()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }
}