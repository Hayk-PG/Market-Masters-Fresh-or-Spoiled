
public class PlayerInventoryManager : EntityInventoryManager
{
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

    protected override bool HavePermission()
    {
        return photonView.IsMine;
    }
}