using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;

    [Header("Particle Images")]
    [SerializeField] private ItemsChangeParticleController[] _itemsChangeParticleControllers;

    [Header("Audio Source")]
    [SerializeField] private ExternalSoundSource _audioSource;

    


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.Play("HelicopterMovementAnim", 0, 0);
            _audioSource.gameObject.SetActive(true);
        }
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
