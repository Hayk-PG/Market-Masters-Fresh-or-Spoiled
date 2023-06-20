
public class BotIndexManager : EntityIndexManager
{
    public void Initialize(int actorNumber, int botIndex, int teamIndex)
    {
        _entityIndex = (EntityIndex)botIndex;
        _teamIndex = (TeamIndex)teamIndex;

        GameSceneReferences.Manager.PlayerUIGroups[botIndex].SetOwnership(ownerName: _entityManager.EntityName, actorNumber: actorNumber, false);
    }
}