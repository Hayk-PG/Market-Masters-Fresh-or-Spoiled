using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Pautik;
using Photon.Realtime;

public class DemandDrivenItemsManager : MonoBehaviourPun
{
    private List<byte> _entitiesInventoryItemsList = new List<byte>();
    private HashSet<byte> _itemsInDemandHashSet;
    private byte[] _itemsInDemandArray;

    private bool IsMasterClient => MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer);




    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonNetworkEvent;
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void OnPhotonNetworkEvent(EventData eventData)
    {
        if (!IsMasterClient)
        {
            return;
        }

        GetEntitiesInventory(eventData, eventData.CustomData);
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        if (!IsMasterClient)
        {
            return;
        }

        GetEntitiesInventory(gameEventType, data);
        UpdateItemsInDemand(gameEventType, data);
    }

    private void GetEntitiesInventory(EventData eventData, object data)
    {
        if (eventData.Code != EventInfo.Code_InventoryPublisher)
        {
            return;
        }

        AddEntitiesInventoryItemsListRange((byte[])data);
    }

    private void GetEntitiesInventory(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.PublishInventoryData)
        {
            return;
        }

        AddEntitiesInventoryItemsListRange((byte[])data[0]);
    }

    private void AddEntitiesInventoryItemsListRange(byte[] itemsIds)
    {
        _entitiesInventoryItemsList.AddRange(itemsIds);
        print($"Entities inventory items count: {_entitiesInventoryItemsList.Count}");
    }

    private void EmptyEntitiesInventoryItemsList()
    {
        _entitiesInventoryItemsList = new List<byte>();
    }

    private void UpdateItemsInDemand(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.UpdateGameTurn)
        {
            return;
        }

        bool isTargetTurn = GameSceneReferences.Manager.GameTurnManager.TurnCount == 2 || GameSceneReferences.Manager.GameTurnManager.TurnCount % 10 == 0;
        bool isEntitiesInventoryItemsListEmpty = _entitiesInventoryItemsList.Count == 0;

        if (isTargetTurn && !isEntitiesInventoryItemsListEmpty)
        {
            InitializeItemsInDemandHashSet();
            GenerateRandomItemsInDemand();
            ConvertItemsInDemandToArray();
            RaisePhotonNetworkEvent();
            EmptyEntitiesInventoryItemsList();
        }
    }

    private void InitializeItemsInDemandHashSet()
    {
        _itemsInDemandHashSet = new HashSet<byte>();
    }

    private void GenerateRandomItemsInDemand()
    {
        for (int i = 0; i < 10; i++)
        {
            byte randomItemId = _entitiesInventoryItemsList[Random.Range(0, _entitiesInventoryItemsList.Count)];
            _itemsInDemandHashSet.Add(randomItemId);
        }
    }

    private void ConvertItemsInDemandToArray()
    {
        _itemsInDemandArray = _itemsInDemandHashSet.ToArray();
    }

    private void RaisePhotonNetworkEvent()
    {
        EventInfo.Content_DemandDrivenItemsId = _itemsInDemandArray;
        PhotonNetwork.RaiseEvent(EventInfo.Code_DemandDrivenItemsId, EventInfo.Content_DemandDrivenItemsId, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
    }
}