using UnityEngine;

public class BotManager : EntityManager
{
    [Header("Components")]
    [SerializeField] private BotIndexManager _botIndexManager;

    [Header("Identification")]
    [SerializeField] private string _botName;
    [SerializeField] private int _actorNumber;

    public override string EntityName
    {
        get => _botName;
        protected set => _botName = value;
    }
    public override int EntityActorNumber => _actorNumber;




    public void Initialize(string botName, int actorNumber, int botIndex, int teamIndex)
    {
        EntityName = botName;
        _actorNumber = actorNumber;
        _botIndexManager.Initialize(actorNumber, botIndex, teamIndex);
    }
}