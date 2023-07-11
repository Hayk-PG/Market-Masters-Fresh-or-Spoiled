using UnityEngine;

public class NotificationImagesGroupManager : MonoBehaviour
{
    [Header("Grid Elements")]
    [SerializeField] private NotificationImage[] _notificationImages;




    public void Setup(Sprite[] sprites)
    {
        for (int i = 0; i < _notificationImages.Length; i++)
        {
            if(i >= sprites.Length)
            {
                _notificationImages[i].gameObject.SetActive(false);
                continue;
            }

            _notificationImages[i].gameObject.SetActive(true);
            _notificationImages[i].SetIcon(sprites[i]);
        }
    }
}