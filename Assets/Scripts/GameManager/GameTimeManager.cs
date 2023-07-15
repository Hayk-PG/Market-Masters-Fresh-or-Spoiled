using System.Collections;
using UnityEngine;
using Pautik;
using Photon.Pun;

public class GameTimeManager : MonoBehaviourPun
{
    private float _gameTime = 5f;
    private float _roundDuration = 11f;
    private float _sessionTime = 900f;
    private WaitForSeconds _delay = new WaitForSeconds(1f);
    private object[] _data = new object[3];




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
            yield return _delay;
        }
    }

    private void UpdateGameTime()
    {
        if(_gameTime <= 0f)
        {
            _gameTime = _roundDuration;
        }

        _gameTime -= 1f;
        _sessionTime -= 1f;
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
        photonView.RPC("PublishGameTimeRPC", RpcTarget.AllViaServer, (short)_gameTime, (short)_sessionTime);
    }

    [PunRPC]
    private void PublishGameTimeRPC(short gameTime, short sessionTime)
    {
        WrapGameTime(gameTime, sessionTime);
        GameEventHandler.RaiseEvent(GameEventType.UpdateGameTime, _data);
    }

    private void WrapGameTime(float gameTime, float sessionTime)
    {
        _data[0] = gameTime;
        _data[1] = _roundDuration;
        _data[2] = sessionTime;
    }
}