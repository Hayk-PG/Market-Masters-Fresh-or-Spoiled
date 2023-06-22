
public class PlayerInventoryManager : EntityInventoryManager
{
    private bool _isInventoryUIManagerTeamSet;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!HavePermission())
        {
            return;
        }

        SetInventoryUIManagerTeam(gameEventType, data);
    }

    protected override void InitializeInventory()
    {
        if (!HavePermission())
        {
            return;
        }

        for (int i = 0; i < _inventorySize; i++)
        {
            AddRandomItemFromCollection(out Item item);
            GameSceneReferences.Manager.PlayerInventoryUIManager.AssignInvetoryItem(i, item);          
        }
    }

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

    protected override bool HavePermission()
    {
        return photonView.IsMine;
    }
}