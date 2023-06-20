using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHubManager : BaseMainHUDTab
{
    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        
    }
}