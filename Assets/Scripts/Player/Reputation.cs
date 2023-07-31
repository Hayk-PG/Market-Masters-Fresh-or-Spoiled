public struct Reputation 
{
    public Reputation(int reputationPoints)
    {
        GameEventHandler.RaiseEvent(GameEventType.UpdateReputation, new object[] { reputationPoints });
    }
}