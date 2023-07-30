using Photon.Pun;
using Pautik;
using UnityEngine;

/// <summary>
/// Manages the item spoilage surge feature in the game.
/// </summary>
public class ItemSpoilageSurgeManager : MonoBehaviourPun
{
    private short _spoilageSurgeDuration = 1500;
    private object[] _spoilageSurgeData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles the game events to trigger the spoilage surge event.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        RaiseItemSpoilageSurgeEvent(gameEventType, data);
    }

    /// <summary>
    /// Raises the spoilage surge event when the game time updates.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void RaiseItemSpoilageSurgeEvent(GameEventType gameEventType, object[] data)
    {
        if (gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        bool isReadyToHandle = MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) && GameSceneReferences.Manager.GameManager.IsGameStarted;

        if (!isReadyToHandle)
        {
            return;
        }

        float gameRemainingTime = (float)data[2];
        bool isRandomNumberTriggerNumber = Random.Range(0, 21) == 17;
        bool isReadyToProceed = isRandomNumberTriggerNumber && gameRemainingTime < _spoilageSurgeDuration && gameRemainingTime % 12 == 0;

        if (!isReadyToProceed)
        {
            return;
        }

        _spoilageSurgeDuration = (short)(gameRemainingTime - 40);
        photonView.RPC("RaiseItemSpoilageSurgeEventRPC", RpcTarget.AllViaServer, _spoilageSurgeDuration);
    }

    /// <summary>
    /// RPC method to raise the spoilage surge event for all players in the game.
    /// </summary>
    /// <param name="spoilageSurgeDuration">The updated duration of the spoilage surge event.</param>
    [PunRPC]
    private void RaiseItemSpoilageSurgeEventRPC(short spoilageSurgeDuration)
    {
        _spoilageSurgeDuration = spoilageSurgeDuration;
        _spoilageSurgeData[0] = spoilageSurgeDuration;
        GameEventHandler.RaiseEvent(GameEventType.ItemSpoilageSurge, _spoilageSurgeData);
    }
}