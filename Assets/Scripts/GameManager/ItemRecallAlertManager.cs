using UnityEngine;
using Pautik;
using System.Collections.Generic;
using Photon.Pun;

/// <summary>
/// Manages the item recall alert feature in the game.
/// </summary>
public class ItemRecallAlertManager : MonoBehaviourPun
{       
    private short _itemRecallAlertDuration;
    private short _recallLevel;
    private short[] _recallItemsIdsArray;    
    private HashSet<short> _recallItemsIdsHashSet;
    private object[] _itemRecallAlertData = new object[3];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    /// <summary>
    /// Handles the game turn update event to trigger the item recall alert.
    /// </summary>
    /// <param name="gameEventType">The type of game event.</param>
    /// <param name="data">Data associated with the game event.</param>
    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        if (!MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            return;
        }

        bool isRandomNumberTriggerNumber = Random.Range(0, 21) == 12;
        bool isItemRecallAlertActive = GameSceneReferences.Manager.GameTurnManager.TurnCount <= _itemRecallAlertDuration;

        if (!isRandomNumberTriggerNumber)
        {
            return;
        }

        if(isItemRecallAlertActive)
        {
            return;
        }

        SetItemRecallAlertDuration();
        PopulateRecallItemsHashSet();
        CopyRecallItemsToArray();
        RaiseItemRecallAlertEvent();
    }

    /// <summary>
    /// Sets the duration of the item recall alert.
    /// </summary>
    private void SetItemRecallAlertDuration()
    {
        _itemRecallAlertDuration = (short)(GameSceneReferences.Manager.GameTurnManager.TurnCount + Random.Range(3, 13));
    }

    /// <summary>
    /// Populates the recall items hash set with random items.
    /// </summary>
    private void PopulateRecallItemsHashSet()
    {
        _recallItemsIdsHashSet = new HashSet<short>();

        for (int i = 0; i < Random.Range(1, 11); i++)
        {
            _recallItemsIdsHashSet.Add((short)GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)].ID);
        }
    }

    /// <summary>
    /// Copies the recall items from the hash set to the array.
    /// </summary>
    private void CopyRecallItemsToArray()
    {
        if (_recallItemsIdsHashSet.Count > 0)
        {
            _recallItemsIdsArray = new short[_recallItemsIdsHashSet.Count];
            _recallItemsIdsHashSet.CopyTo(_recallItemsIdsArray);
        }
    }

    /// <summary>
    /// Raises the item recall alert event if there are items to be recalled.
    /// </summary>
    private void RaiseItemRecallAlertEvent()
    {
        bool hasItemForRecall = _recallItemsIdsArray != null && _recallItemsIdsArray.Length > 0;

        if (!hasItemForRecall)
        {
            return;
        }

        _recallLevel = (short)Random.Range(0, 6);
        photonView.RPC("RaiseItemRecallAlertEventRPC", RpcTarget.AllViaServer, _recallItemsIdsArray, _itemRecallAlertDuration, _recallLevel);
    }

    /// <summary>
    /// RPC method to raise the item recall alert event for all players in the game.
    /// </summary>
    /// <param name="recallItemsIdsArray">The array of item IDs to be recalled.</param>
    /// <param name="itemRecallAlertDuration">The duration of the item recall alert.</param>
    /// <param name="recallLevel">The recall level for the items.</param>
    [PunRPC]
    private void RaiseItemRecallAlertEventRPC(short[] recallItemsIdsArray, short itemRecallAlertDuration, short recallLevel)
    {
        _itemRecallAlertDuration = itemRecallAlertDuration;

        _itemRecallAlertData[0] = recallItemsIdsArray;
        _itemRecallAlertData[1] = itemRecallAlertDuration;
        _itemRecallAlertData[2] = recallLevel;
        GameEventHandler.RaiseEvent(GameEventType.TriggerItemRecallAlert, _itemRecallAlertData);
    }
}