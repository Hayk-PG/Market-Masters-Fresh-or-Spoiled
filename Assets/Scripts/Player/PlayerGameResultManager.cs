using UnityEngine;

public class PlayerGameResultManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;

    [Header("Game Result Screens")]
    [SerializeField] private GameObject _victoryScreenPrefab;
    [SerializeField] private GameObject _defeatScreenPrefab;

    private bool _isScreenDisplayed;

    private bool IsPhotonviewMie => _entityManager.PlayerPhotonview.IsMine;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!IsPhotonviewMie)
        {
            return;
        }

        DisplayGameResult(gameEventType);
    }

    private void DisplayGameResult(GameEventType gameEventType)
    {
        if(gameEventType != GameEventType.EndGame)
        {
            return;
        }

        if (_isScreenDisplayed)
        {
            return;
        }

        _isScreenDisplayed = true;
        int playerTeamStockAmount = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team1StockAmount :
                                                                                       GameSceneReferences.Manager.TeamStockManager.Team2StockAmount;
        int opponentTeamStockAmount = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team2StockAmount :
                                                                                     GameSceneReferences.Manager.TeamStockManager.Team1StockAmount;
        bool? isWin = playerTeamStockAmount > opponentTeamStockAmount ? true : playerTeamStockAmount < opponentTeamStockAmount ? false : null;
        InstantiateCorrectScreenResult(isWin.Value);
    }

    private void InstantiateCorrectScreenResult(bool? isWin)
    {
        if (isWin.Value)
        {
            Instantiate(_victoryScreenPrefab, FindObjectOfType<MainHUD>().transform);
            return;
        }

        if (!isWin.Value)
        {
            Instantiate(_defeatScreenPrefab, FindObjectOfType<MainHUD>().transform);
            return;
        }

        else
        {
            // Instantiate draw screen
            return;
        }
    }
}