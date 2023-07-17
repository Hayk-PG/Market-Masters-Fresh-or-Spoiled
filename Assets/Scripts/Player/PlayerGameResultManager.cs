using UnityEngine;

public class PlayerGameResultManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private EntityManager _entityManager;
    [SerializeField] private EntityIndexManager _entityIndexManager;

    [Header("Game Result Screens")]
    [SerializeField] private BaseGameResultScreenUIManager _victoryScreenUI;
    [SerializeField] private BaseGameResultScreenUIManager _defeatScreenUI;

    private BaseGameResultScreenUIManager _targetScreenUI;
    private AudioSource _musicSource;
    private bool _isMusicChanged;
    private bool _isScreenDisplayed;

    private bool IsPhotonviewMie => _entityManager.PlayerPhotonview.IsMine;




    private void Awake()
    {
        _musicSource = SoundController.Instance.GetMusicSource();
    }

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

        ChangeMusic(gameEventType, data);
        DisplayGameResult(gameEventType);
    }

    private void ChangeMusic(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        float remainingTime = (float)data[2];

        if(remainingTime > 30f)
        {
            return;
        }

        if (_isMusicChanged)
        {
            return;
        }

        _musicSource.clip = SoundController.Instance.SoundsList[0]._clips[1]._clip;
        _musicSource.Play();

        //SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        //_musicSource.PlayOneShot(SoundController.Instance.SoundsList[0]._clips[1]._clip);
        _isMusicChanged = true;

        print("Music has been changed");
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

        int playerTeamStockAmount = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team1StockAmount :
                                                                                       GameSceneReferences.Manager.TeamStockManager.Team2StockAmount;
        int opponentTeamStockAmount = _entityIndexManager.TeamIndex == TeamIndex.Team1 ? GameSceneReferences.Manager.TeamStockManager.Team2StockAmount :
                                                                                     GameSceneReferences.Manager.TeamStockManager.Team1StockAmount;
        bool? isWin = playerTeamStockAmount > opponentTeamStockAmount ? true : playerTeamStockAmount < opponentTeamStockAmount ? false : null;

        InstantiateCorrectScreenResult(isWin, playerTeamStockAmount);
        _isScreenDisplayed = true;
    }

    private void InstantiateCorrectScreenResult(bool? isWin, int playerTeamStockAmount)
    {
        if (!isWin.HasValue)
        {
            InstantiateScreenUI(_victoryScreenUI, playerTeamStockAmount);
            return;
        }

        if (isWin.Value)
        {
            InstantiateScreenUI(_victoryScreenUI, playerTeamStockAmount);
            return;
        }

        if (!isWin.Value)
        {
            InstantiateScreenUI(_defeatScreenUI, playerTeamStockAmount);
            return;
        }
    }

    private void InstantiateScreenUI(BaseGameResultScreenUIManager targetScreenUI, int playerTeamStockAmount)
    {
        _targetScreenUI = Instantiate(targetScreenUI, FindObjectOfType<MainHUD>().transform);
        _targetScreenUI.UpdateMoneyAmountText(playerTeamStockAmount);
    }
}