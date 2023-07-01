using UnityEngine;

public class PlayerShopItemUpdater : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;   
    [SerializeField] private PlayerReputationManager _playerReputationManager;

    private object[] _shopItemsData = new object[2];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Event handler for game events.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!_entityManager.PlayerPhotonview.IsMine)
        {
            return;
        }

        UpdateShopItems(gameEventType, data);
    }

    /// <summary>
    /// Updates the shop items based on the game event.
    /// </summary>
    /// <param name="gameEventType">The type of the game event.</param>
    /// <param name="data">Additional data associated with the event.</param>
    private void UpdateShopItems(GameEventType gameEventType, object[] data)
    {
        bool canUpdateShopItems = gameEventType == GameEventType.UpdateGameTurn && _entityIndexManager.TeamIndex != (TeamIndex)data[2];

        if (!canUpdateShopItems)
        {
            return;
        }

        _shopItemsData[0] = GetShopItemsCount();
        _shopItemsData[1] = GeneratePricePercentageRange();
        GameEventHandler.RaiseEvent(GameEventType.UpdateShopItems, _shopItemsData);
    }

    /// <summary>
    /// Gets the random number of shop items based on the player's reputation state.
    /// </summary>
    /// <returns>The number of shop items.</returns>
    private int GetShopItemsCount()
    {
        int itemsCount = GameSceneReferences.Manager.Items.Collection.Count;
        int itemsNewCount = 0;

        switch (_playerReputationManager.ReputationState)
        {
            case ReputationState.Terrible: itemsNewCount = Random.Range(0, Mathf.RoundToInt(itemsCount * 0.2f)); break;
            case ReputationState.Poor: itemsNewCount = Random.Range(Mathf.RoundToInt(itemsCount * 0.2f), Mathf.RoundToInt(itemsCount * 0.4f)); break;
            case ReputationState.Neutral: itemsNewCount = Random.Range(Mathf.RoundToInt(itemsCount * 0.4f), Mathf.RoundToInt(itemsCount * 0.6f)); break;
            case ReputationState.Good: itemsNewCount = Random.Range(Mathf.RoundToInt(itemsCount * 0.6f), Mathf.RoundToInt(itemsCount * 0.9f)); break;
            case ReputationState.Excellent: itemsNewCount = itemsCount; break;
            default: itemsNewCount = itemsCount; break;
        }

        return itemsNewCount;
    }

    /// <summary>
    /// Generates the price percentage range based on the player's reputation state.
    /// </summary>
    /// <returns>A tuple representing the minimum and maximum price percentages.</returns>
    private System.Tuple<int, int> GeneratePricePercentageRange()
    {
        int minRange = 0;
        int maxRange = 0;

        switch (_playerReputationManager.ReputationState)
        {
            case ReputationState.Terrible: minRange = 100; maxRange = 400; break;
            case ReputationState.Poor: minRange = 90; maxRange = 400; break;
            case ReputationState.Neutral: minRange = 40; maxRange = 400; break;
            case ReputationState.Good: minRange = 70; maxRange = 150; break;
            case ReputationState.Excellent: minRange = 40; maxRange = 70; break;
            default: minRange = 390; maxRange = 400; break;
        }

        return new System.Tuple<int, int>(minRange, maxRange);
    }
}