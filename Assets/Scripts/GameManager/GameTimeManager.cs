using System.Collections;
using UnityEngine;
using Pautik;
using Photon.Pun;

public class GameTimeManager : MonoBehaviourPun
{
    private float _gameTime = 5f;
    private float _roundDuration = 11f;

    private object[] _data = new object[2];




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        OverrideGameTime(gameEventType, data);
    }

    private void Start()
    {
        StartCoroutine(RunGameTimer());
    }

    private IEnumerator RunGameTimer()
    {
        while(MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            UpdateGameTime();           
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateGameTime()
    {
        if(_gameTime <= 0f)
        {
            _gameTime = _roundDuration;
        }

        _gameTime -= 1f;
        PublishGameTime();
    }

    private void OverrideGameTime(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.OverrideGameTime)
        {
            return;
        }

        float targetGameTime = (float)data[0];

        if(_gameTime > 1f)
        {
            _gameTime = targetGameTime;
        }
    }

    private void PublishGameTime()
    {
        photonView.RPC("PublishGameTimeRPC", RpcTarget.AllViaServer, _gameTime);
    }

    [PunRPC]
    private void PublishGameTimeRPC(float gameTime)
    {
        WrapGameTime(gameTime);
        GameEventHandler.RaiseEvent(GameEventType.UpdateGameTime, _data);
    }

    private void WrapGameTime(float gameTime)
    {
        _data[0] = gameTime;
        _data[1] = _roundDuration;
    }
}