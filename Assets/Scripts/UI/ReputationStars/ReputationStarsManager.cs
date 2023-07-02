using UnityEngine;
using UnityEngine.UI;

public class ReputationStarsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] _stars;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.SendReputationPoints)
        {
            return;
        }

        UpdateStarFillAmount(reputationPoints: (int)data[0]);
    }

    private void UpdateStarFillAmount(int reputationPoints)
    {
        float fillAmount = 0;
        float target = reputationPoints * 0.05f;

        foreach (var star in _stars)
        {
            star.fillAmount = (target - fillAmount);
            fillAmount += star.fillAmount;
        }
    }
}