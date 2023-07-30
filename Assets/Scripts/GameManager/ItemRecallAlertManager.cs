using UnityEngine;
using Pautik;
using System.Collections.Generic;
using Photon.Pun;

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

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            return;
        }

        if(gameEventType != GameEventType.UpdateGameTurn)
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

    private void SetItemRecallAlertDuration()
    {
        _itemRecallAlertDuration = (short)(GameSceneReferences.Manager.GameTurnManager.TurnCount + Random.Range(3, 13));
    }

    private void PopulateRecallItemsHashSet()
    {
        _recallItemsIdsHashSet = new HashSet<short>();

        for (int i = 0; i < Random.Range(1, 11); i++)
        {
            _recallItemsIdsHashSet.Add((short)GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)].ID);
        }
    }

    private void CopyRecallItemsToArray()
    {
        if (_recallItemsIdsHashSet.Count > 0)
        {
            _recallItemsIdsArray = new short[_recallItemsIdsHashSet.Count];
            _recallItemsIdsHashSet.CopyTo(_recallItemsIdsArray);
        }
    }

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

    [PunRPC]
    private void RaiseItemRecallAlertEventRPC(short[] recallItemsIdsArray, short itemRecallAlertDuration, short recallLevel)
    {
        _itemRecallAlertData[0] = recallItemsIdsArray;
        _itemRecallAlertData[1] = itemRecallAlertDuration;
        _itemRecallAlertData[2] = recallLevel;
        GameEventHandler.RaiseEvent(GameEventType.TriggerItemRecallAlert, _itemRecallAlertData);
    }
}