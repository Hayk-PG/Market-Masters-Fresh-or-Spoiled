using UnityEngine;

public abstract class PlayerBaseEventGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityManager _entityManager;
    [SerializeField] protected EntityInventoryManager _entityInventoryManager;
    [SerializeField] protected EntityIndexManager _entityIndexManager;
    [SerializeField] protected PlayerReputationManager _playerReputationManager;

    protected bool CanReceiveGameEvent => _entityManager.PlayerPhotonview.IsMine;




    protected virtual void OnEnable()
    {
        SubscribeToGameEvents();
    }

    protected virtual void SubscribeToGameEvents()
    {
        if (!CanReceiveGameEvent)
        {
            return;
        }

        GameEventHandler.OnEvent += OnGameEvent;
    }

    protected abstract void OnGameEvent(GameEventType gameEventType, object[] data);
}