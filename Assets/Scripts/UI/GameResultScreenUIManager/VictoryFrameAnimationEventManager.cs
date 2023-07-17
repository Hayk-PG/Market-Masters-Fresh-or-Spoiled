using UnityEngine;

public class VictoryFrameAnimationEventManager : MonoBehaviour
{
    [Header("Victory Screen UI Manager")]
    [SerializeField] private VictoryScreenUIManager _vicotryScreenUIManager;




    // Animation callback
    private void TriggerAnimationEvent()
    {
        _vicotryScreenUIManager.PlayVictoryScreenAnimation();
    }
}