
public class PlayerInventoryManager : EntityInventoryManager
{
    private bool _isInventoryUIManagerTeamSet;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles game events.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!HavePermission())
        {
            return;
        }

        SetInventoryUIManagerTeam(gameEventType, data);
        DestroySpoiledItem(gameEventType, data);
    }

    /// <summary>
    /// Initializes the player's inventory.
    /// </summary>
    protected override void InitializeInventory()
    {
        if (!HavePermission())
        {
            return;
        }

        for (int i = 0; i < _inventorySize; i++)
        {
            AddRandomItemFromCollection(out Item item);
            GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(item);          
        }
    }

    /// <summary>
    /// Sets the team for the inventory UI manager.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void SetInventoryUIManagerTeam(GameEventType gameEventType, object[] data)
    {
        bool canSetInventoryUIManagerTeam = gameEventType == GameEventType.UpdateGameTurn && !_isInventoryUIManagerTeamSet;

        if (!canSetInventoryUIManagerTeam)
        {
            return;
        }

        GameSceneReferences.Manager.PlayerInventoryUIManager.GetControllerTeam(_entityIndexManager.TeamIndex);
        _isInventoryUIManagerTeamSet = true;
    }

    /// <summary>
    /// Destroys a spoiled item from the inventory.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">The event data.</param>
    private void DestroySpoiledItem(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.DestroySpoiledItem)
        {
            return;
        }

        int spoiledItemId = (int)data[0];
        RemoveItem(null, spoiledItemId);
    }

    /// <summary>
    /// Checks if the player has permission to manage the inventory.
    /// </summary>
    /// <returns><c>true</c> if the player has permission; otherwise, <c>false</c>.</returns>
    protected override bool HavePermission()
    {
        return _entityManager.PlayerPhotonview.IsMine;
    }
}