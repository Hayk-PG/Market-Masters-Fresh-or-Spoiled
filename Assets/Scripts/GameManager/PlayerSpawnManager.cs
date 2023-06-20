using Photon.Pun;
using UnityEngine;
using Pautik;

public class PlayerSpawnManager : MonoBehaviourPun
{
    private const string _playerPrefabName = "Player";
    private PlayerManager _playerManager;




    private void Start()
    {
        InstantiatePlayer();
    }

    /// <summary>
    /// Instantiates the player object and initializes it.
    /// </summary>
    private void InstantiatePlayer()
    {
        _playerManager = Get<PlayerManager>.From(PhotonNetwork.Instantiate(_playerPrefabName, Vector3.zero, Quaternion.identity));
        _playerManager.InitializePlayer(_playerManager.EntityActorNumber);
    }
}