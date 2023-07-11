using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Pautik;
using Photon.Realtime;

public class DemandDrivenItemsManager : MonoBehaviourPun
{
    private HashSet<byte> _itemsInDemandHashSet;
    private byte[] _itemsInDemandArray;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        UpdateItemsInDemand(gameEventType, data);
    }

    /// <summary>
    /// Updates the items in demand based on the game event type.
    /// </summary>
    /// <param name="gameEventType">The game event type.</param>
    /// <param name="data">Additional data for the game event.</param>
    private void UpdateItemsInDemand(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        bool isFirstTurn = GameSceneReferences.Manager.GameTurnManager.TurnCount == 1;
        bool isTurnCountDivisibleByTen = GameSceneReferences.Manager.GameTurnManager.TurnCount % 10 == 0;
        bool isMasterClient = MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);
        bool canGenerateItemsInDemand = isTurnCountDivisibleByTen && isMasterClient || isFirstTurn && isMasterClient;

        if (!canGenerateItemsInDemand)
        {
            return;
        }

        InitializeItemsInDemandHashSet();
        GenerateRandomItemsInDemand();
        ConvertItemsInDemandToArray();
        RaisePhotonNetworkEvent();
    }

    /// <summary>
    /// Initializes the items in demand hash set.
    /// </summary>
    private void InitializeItemsInDemandHashSet()
    {
        _itemsInDemandHashSet = new HashSet<byte>();
    }

    /// <summary>
    /// Generates random items in demand by adding them to the hash set.
    /// </summary>
    private void GenerateRandomItemsInDemand()
    {
        for (int i = 0; i < 10; i++)
        {
            byte randomItemId = (byte)GameSceneReferences.Manager.Items.Collection[Random.Range(0, GameSceneReferences.Manager.Items.Collection.Count)].ID;
            _itemsInDemandHashSet.Add(randomItemId);
        }
    }

    /// <summary>
    /// Converts the items in demand hash set to an array.
    /// </summary>
    private void ConvertItemsInDemandToArray()
    {
        _itemsInDemandArray = _itemsInDemandHashSet.ToArray();
    }

    /// <summary>
    /// Raises a Photon Network event with the items in demand array.
    /// </summary>
    private void RaisePhotonNetworkEvent()
    {
        EventInfo.Content_DemandDrivenItemsId = _itemsInDemandArray;
        PhotonNetwork.RaiseEvent(EventInfo.Code_DemandDrivenItemsId, EventInfo.Content_DemandDrivenItemsId, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
    }
}