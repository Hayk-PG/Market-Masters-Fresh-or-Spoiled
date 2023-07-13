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

    private List<Notification> _notifications = new List<Notification>();
    private Notification _currentDisplayedNotification;
    private const string _newNotificationAnim = "NotificationButtonAnim";
    private int _unreadNotificationCount;
    private object[] _notificationData = new object[3];




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
        DisplayNotification(indexPointer: HasUnreadNotification() ? 1 : 0);
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
        RaiseDisplayNotificationEvent(true);
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

        DisplayNotification(indexPointer: (int)data[0]);
    }

    private void DisplayNotification(int indexPointer)
    {
        if (!HasNotification())
        {
            return;
        }

        AssignCurrentDisplayedNotification(indexPointer);
        RaiseDisplayNotificationEvent(false);
        UpdateUnreadNotificationCount(-1);
        SetIcon();
    }

    private void StoreNotification(object[] data)
    {
        _notifications.Add((Notification)data[0]);
        UpdateUnreadNotificationCount(1);
    }

    private void SetIcon()
    {
        _icon.IconSpriteChangeDelegate(HasUnreadNotification() ? _sprites[1] : _sprites[0]);
        _icon.ChangeReleasedSpriteDelegate();
    }

    private void AssignCurrentDisplayedNotification(int indexPointer)
    {
        if(!HasNotification())
        {
            return;
        }

        int currentIndex = _notifications.IndexOf(_currentDisplayedNotification);
        int nextIndex = currentIndex + indexPointer;
        bool isOutOfRange = nextIndex >= _notifications.Count || nextIndex < 0;

        if (isOutOfRange)
        {
            return;
        }

        _currentDisplayedNotification = _notifications[nextIndex];
    }

    private void RaiseDisplayNotificationEvent(bool onlyUpdateNotification)
    {
        _notificationData[0] = onlyUpdateNotification;
        _notificationData[1] = _currentDisplayedNotification;
        _notificationData[2] = $"{_notifications.IndexOf(_currentDisplayedNotification) + 1}/{_notifications.Count}";
        GameEventHandler.RaiseEvent(GameEventType.DisplayNotification, _notificationData);
    }

    private bool HasNotification()
    {
        return _notifications.Count > 0;
    }

    private bool HasUnreadNotification()
    {
        return _unreadNotificationCount > 0;
    }

    private void UpdateUnreadNotificationCount(int notificationValue)
    {
        _unreadNotificationCount += notificationValue;

        if(_unreadNotificationCount < 0)
        {
            _unreadNotificationCount = 0;
        }
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