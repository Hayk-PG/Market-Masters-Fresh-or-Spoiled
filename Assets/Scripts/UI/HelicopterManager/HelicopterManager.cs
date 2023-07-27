using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("Particle Images")]
    [SerializeField] private ItemsChangeParticleController[] _itemsChangeParticleControllers;

    [Header("Audio Source")]
    [SerializeField] private ExternalSoundSource _audioSource;

    


    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.ActivateItemsDroppingHelicopter)
        {
            return;
        }

        ActivateItemsDroppingHelicopter();
    }

    private void ActivateItemsDroppingHelicopter()
    {
        _animator.Play("HelicopterMovementAnim", 0, 0);
        _audioSource.gameObject.SetActive(true);
    }

    // Animation Event
    private void ShootItemParticles()
    {
        UISoundController.PlaySound(6, 2);

        for (int i = 0; i < GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons.Length; i++)
        {
            _itemsChangeParticleControllers[i].CustomizeParticleAndPlay(item: GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)],
                                                                        playerInventoryItemButton: GameSceneReferences.Manager.PlayerInventoryUIManager.PlayerInventoryItemButtons[i]);
        }
    }
}
