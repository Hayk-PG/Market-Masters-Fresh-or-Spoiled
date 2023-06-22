using UnityEngine;

public class BotInventoryManager : EntityInventoryManager
{
    public string BotName => _entityManager.EntityName;
    public int BotActorNumber => _entityManager.EntityActorNumber;

    private byte[] _itemsIndexes;




    protected override void InitializeInventory()
    {
        _itemsIndexes = new byte[_inventorySize];

        for (int i = 0; i < _itemsIndexes.Length; i++)
        {
            _itemsIndexes[i] = (byte)Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count);         
        }

        GameSceneReferences.Manager.RemoteRPCWrapper.InitializeBotInventory(BotName, BotActorNumber, _itemsIndexes);
    }
}