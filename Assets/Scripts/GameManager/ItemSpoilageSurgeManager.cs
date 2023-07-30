using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Pautik;

public class ItemSpoilageSurgeManager : MonoBehaviourPun
{
    private short _spoilageSurgeDuration = 1500;
    private object[] _spoilageSurgeData = new object[1];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTime)
        {
            return;
        }

        if (!MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            return;
        }

        float gameRemainingTime = (float)data[2];

        if(gameRemainingTime > _spoilageSurgeDuration)
        {
            return;
        }

        if(gameRemainingTime % 12 != 0)
        {
            return;
        }

        _spoilageSurgeDuration = (short)(gameRemainingTime - 40);
    }

    [PunRPC]
    private void IncreaseSpoilageRateRPC(short spoilageSurgeDuration)
    {
        _spoilageSurgeDuration = spoilageSurgeDuration;
        _spoilageSurgeData[0] = _spoilageSurgeDuration;
        GameEventHandler.RaiseEvent(GameEventType.IncreaseSpoilageRate, _spoilageSurgeData);
    }
}