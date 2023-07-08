using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Btn _button;
    [SerializeField] private Animator _animator;

    [Header("UI Elements")]
    [SerializeField] private Btn_Icon _icon;

    [Header("Sprites")]
    [SerializeField] private Sprite[] _sprites;

    [Header("Queued Notifications")]
    [SerializeField] private List<Notification> _queuedNotifications = new List<Notification>();

    private Notification _currentDisplayedNotification;
    private const string _newNotificationAnim = "NotificationButtonAnim";
    private object[] _notificationData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
        _button.OnSelect += OnSelect;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        QueueNotification(gameEventType, data);
        TryDisplayNextNotification(gameEventType, data);
    }

    private void OnSelect()
    {
        PlaySoundEffect(0, 6);
        StartCoroutine(DeselectAfterDelay());
        DisplayNotification();      
    }

    private IEnumerator DeselectAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        DeselectButton();
    }

    private void QueueNotification(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.QueueNotification)
        {
            return;
        }

        StoreNotification(data);
        SetIcon();
        PlaySoundEffect(7, 7);
        PlayAnimation(_newNotificationAnim);
    }

    private void TryDisplayNextNotification(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.DisplayNextNotification)
        {
            return;
        }

        if (!HasQueuedNotification())
        {
            return;
        }

        DeselectButton();
        DisplayNotification();
    }

    private void DisplayNotification()
    {
        AssignCurrentDisplayedNotification();
        WrapNotificationData();
        GameEventHandler.RaiseEvent(GameEventType.DisplayNotification, _notificationData);
        RemoveNotification(_currentDisplayedNotification);
        SetIcon();
    }

    private void StoreNotification(object[] data)
    {
        _queuedNotifications.Add(new Notification(notificationType: (NotificationType)data[0], notificationTitle: (string)data[1], notificationMessage: (string)data[2], acceptAction: (System.Action)data[3]));
    }

    private void RemoveNotification(Notification notification)
    {
        _queuedNotifications.Remove(notification);
    }

    private void SetIcon()
    {
        _icon.IconSpriteChangeDelegate(_queuedNotifications.Count < 1 ? _sprites[0] : _sprites[1]);
        _icon.ChangeReleasedSpriteDelegate();
    }

    private void AssignCurrentDisplayedNotification()
    {
        if(!HasQueuedNotification())
        {
            return;
        }

        _currentDisplayedNotification = _queuedNotifications[_queuedNotifications.Count - 1];
    }

    private void WrapNotificationData()
    {
        _notificationData[0] = _currentDisplayedNotification;
    }

    private bool HasQueuedNotification()
    {
        return _queuedNotifications.Count > 0;
    }

    private void DeselectButton()
    {
        _button.Deselect();
    }

    private void PlaySoundEffect(int listIndex, int clipIndex)
    {
        UISoundController.PlaySound(listIndex, clipIndex);
    }

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName, 0, 0);
    }
}