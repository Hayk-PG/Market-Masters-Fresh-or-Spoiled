using AssetKits.ParticleImage;
using System.Collections;
using UnityEngine;

public class ItemsChangeParticleController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleImage _particle;

    private object[] _inventoryItemChangeData = new object[2];




    public void CustomizeParticleAndPlay(Item item, PlayerInventoryItemButton playerInventoryItemButton)
    {
        _particle.texture = item.Icon.texture;
        _particle.attractorTarget = playerInventoryItemButton.transform;
        _particle.Play();
        StartCoroutine(TryChangeInventoryItem(playerInventoryItemButton, item));
    }

    private IEnumerator TryChangeInventoryItem(PlayerInventoryItemButton playerInventoryItemButton, Item item)
    {
        Vector2 particlePreviousPosition = Vector2.down;
        Vector2 particleCurrentPosition = Vector2.up;

        while(particlePreviousPosition != particleCurrentPosition)
        {
            if(_particle.particles.Count > 0)
            {
                particlePreviousPosition = particleCurrentPosition;
                particleCurrentPosition = _particle.particles[0].Position;
            }
          
            yield return null;
        }

        if(playerInventoryItemButton.AssociatedItem == null)
        {
            yield break;
        }

        playerInventoryItemButton.DestroySpoiledItemOnSeparateSale();
        _inventoryItemChangeData[0] = playerInventoryItemButton;
        _inventoryItemChangeData[1] = item;
        GameEventHandler.RaiseEvent(GameEventType.ChangeInventoryItem, _inventoryItemChangeData);
    }
}