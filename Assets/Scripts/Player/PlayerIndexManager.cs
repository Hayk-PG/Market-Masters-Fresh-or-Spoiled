using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;
using Pautik;
using System;
using System.Collections;

public class PlayerIndexManager : EntityIndexManager
{
    private PlayerUIGroupManager _firstPlayerUIGroup;
    private PlayerUIGroupManager _secondPlayerUIGroup;
    private PlayerUIGroupManager _myPreviousPlayerUIGroup;
    private PlayerUIGroupManager _teamMatePreviousPlayerUIGroup;




    private void OnEnable()
    {
        ManagePhotonNetworkEventSubscription();
    }

    private void ManagePhotonNetworkEventSubscription(bool isSubscribing = true)
    {
        Conditions<bool>.Compare(isSubscribing, () => PhotonNetwork.NetworkingClient.EventReceived += OnPhotonNetworkEvent, () => PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonNetworkEvent);
    }

    private void OnPhotonNetworkEvent(EventData eventData)
    {
        if (eventData.Code != EventInfo.Code_SetPlayerIndex)
        {
            return;
        }

        RetrieveData(data: (object[])eventData.CustomData);     
    }

    private void RetrieveData(object[] data)
    {
        bool isRightPlayer = (int)data[0] == _entityManager.EntityActorNumber;

        if (!isRightPlayer)
        {
            return;
        }

        _entityIndex = (EntityIndex)(int)data[1];
        _teamIndex = (TeamIndex)(int)data[2];

        SetPlayerUIGroupOwnership();       
        StartCoroutine(TryChangePlayerUIGroupsOwnerships());
        ManagePhotonNetworkEventSubscription(false);
    }

    private void SetPlayerUIGroupOwnership()
    {
        GameSceneReferences.Manager.PlayerUIGroups[EntityNumber].SetOwnership(ownerName: _entityManager.EntityName, actorNumber: _entityManager.EntityActorNumber, isLocalPlayerOwner: true);
    }

    private IEnumerator TryChangePlayerUIGroupsOwnerships()
    {
        bool canChangeOwnerships = photonView.IsMine;

        if (!canChangeOwnerships)
        {
            yield break;
        }

        yield return new WaitUntil(AreAllTeamsSet);

        ChangeTeamGroupUIIndexes();
        TryTakeFirstPlayerUIGroupOwnership();
        TrySetSecondPlayerUIGroupOwnership();
    }

    private void ChangeTeamGroupUIIndexes()
    {
        GameSceneReferences.Manager.TeamGroupPanels[0].ChangeTeamIndex((_teamIndex));
        GameSceneReferences.Manager.TeamGroupPanels[1].ChangeTeamIndex((_teamIndex == TeamIndex.Team1 ? TeamIndex.Team2 : TeamIndex.Team1));
    }

    private void TryTakeFirstPlayerUIGroupOwnership()
    {
        _firstPlayerUIGroup = GameSceneReferences.Manager.PlayerUIGroups[0];
        _myPreviousPlayerUIGroup = GameSceneReferences.Manager.PlayerUIGroups[EntityNumber];

        bool amIOwner = _firstPlayerUIGroup.OwnerName == _entityManager.EntityName;

        if (amIOwner)
        {
            return;
        }

        string previousOwnerName = _firstPlayerUIGroup.OwnerName;
        int previousOwnerActorNumber = _firstPlayerUIGroup.OwnerActorNumber;

        ChangeDesiredPlayerUIGroupOwnership(_firstPlayerUIGroup, _entityManager.EntityName, _entityManager.EntityActorNumber, true);
        ChangeDesiredPlayerUIGroupOwnership(_myPreviousPlayerUIGroup, previousOwnerName, previousOwnerActorNumber);
    }

    private void TrySetSecondPlayerUIGroupOwnership()
    {
        EntityIndexManager teamMateIndexManager = GlobalFunctions.ObjectsOfType<EntityIndexManager>.Find(entity => entity.TeamIndex == TeamIndex);
        bool canChangeTeammateOwnership = teamMateIndexManager != null && GameSceneReferences.Manager.PlayerUIGroups[1].OwnerName != Get<EntityManager>.From(teamMateIndexManager.gameObject).EntityName;

        if (!canChangeTeammateOwnership)
        {
            return;
        }

        _secondPlayerUIGroup = GameSceneReferences.Manager.PlayerUIGroups[1];
        _teamMatePreviousPlayerUIGroup = GameSceneReferences.Manager.PlayerUIGroups[teamMateIndexManager.EntityNumber];

        string previousOwnerName = _secondPlayerUIGroup.OwnerName;
        int previousOwnerActorNumber = _secondPlayerUIGroup.OwnerActorNumber;

        ChangeDesiredPlayerUIGroupOwnership(_secondPlayerUIGroup, teamMateIndexManager.EntityManager.EntityName, teamMateIndexManager.EntityManager.EntityActorNumber);
        ChangeDesiredPlayerUIGroupOwnership(_teamMatePreviousPlayerUIGroup, previousOwnerName, previousOwnerActorNumber);
    }

    private void ChangeDesiredPlayerUIGroupOwnership(PlayerUIGroupManager targetPlayerUIGroupManager, string targetEntityName, int targetEntityActorNumber, bool isLocalPlayerOwner = false)
    {
        targetPlayerUIGroupManager.SetOwnership(targetEntityName, targetEntityActorNumber, isLocalPlayerOwner);
    }

    private bool AreAllTeamsSet()
    {
        bool areAllTeamsSet = true;

        foreach (var playerUIGroup in GameSceneReferences.Manager.PlayerUIGroups)
        {
            if (String.IsNullOrEmpty(playerUIGroup.OwnerName))
            {
                areAllTeamsSet = false;
                break;
            }
        }

        return areAllTeamsSet;
    }
}