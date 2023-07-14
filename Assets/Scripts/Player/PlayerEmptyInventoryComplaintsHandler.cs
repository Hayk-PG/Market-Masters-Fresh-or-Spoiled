using UnityEngine;

public class PlayerEmptyInventoryComplaintsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;
    [SerializeField] private EntityInventoryManager _entityInventoryManager;

    private bool _isInventoryEmpty;
    private object[] _notificationData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles game events related to the player.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        bool canReceiveEvent = _entityManager.PlayerPhotonview.IsMine;

        if (!canReceiveEvent)
        {
            return;
        }

        ExecuteOnTurnUpdate(gameEventType, data);
    }

    /// <summary>
    /// Executes actions when the game turn is updated.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The data associated with the game event.</param>
    private void ExecuteOnTurnUpdate(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        TeamIndex currentTeamTurn = (TeamIndex)data[2];
        bool isPlayerTeamTurn = currentTeamTurn == _entityIndexManager.TeamIndex;

        if (isPlayerTeamTurn)
        {
            SetInventoryEmptyStatus(isInventoryEmpty: _entityInventoryManager.InventoryItems.Count < 1);
            return;
        }

        if (_isInventoryEmpty)
        {          
            UpdateReputation();
            DisplayNotification();
            SetInventoryEmptyStatus(false);
        }
    }

    /// <summary>
    /// Sets the status of the inventory empty flag.
    /// </summary>
    /// <param name="isInventoryEmpty">The status of the inventory being empty.</param>
    private void SetInventoryEmptyStatus(bool isInventoryEmpty)
    {
        _isInventoryEmpty = isInventoryEmpty;
    }

    /// <summary>
    /// Updates the reputation when the inventory is empty.
    /// </summary>
    private void UpdateReputation()
    {
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputationOnEmptyInventory);
    }

    /// <summary>
    /// Displays a notification for empty inventory complaints.
    /// </summary>
    private void DisplayNotification()
    {
        int emptyInventoryComplaintsIndex = Random.Range(0, EmptyInventoryComplaints.Texts.Length);
        _notificationData[0] = new Notification(NotificationType.DisplayReadNotification, EmptyInventoryComplaints.Texts[emptyInventoryComplaintsIndex].Item1, EmptyInventoryComplaints.Texts[emptyInventoryComplaintsIndex].Item2);
        GameEventHandler.RaiseEvent(GameEventType.QueueNotification, _notificationData);
    }
}