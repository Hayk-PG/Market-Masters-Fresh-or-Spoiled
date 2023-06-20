using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerManager : EntityManager
{
    [Header("Player PhotonView Component")]
    [SerializeField] private PhotonView _photonView;

    [Header("Player Nickname")]
    [SerializeField] private string _nickName;

    /// <summary>
    /// The PhotonView component associated with this player.
    /// </summary>
    public PhotonView PlayerPhotonview
    {
        get => _photonView;
    }

    /// <summary>
    /// The Photon Player associated with this player manager.
    /// </summary>
    public Player Player { get; private set; }

    /// <summary>
    /// The tag object associated with the Photon Player.
    /// </summary>
    public object PlayerTagObject { get; private set; }

    /// <summary>
    /// The nickname of the player.
    /// </summary>
    public override string EntityName
    {
        get => _nickName;
        protected set => _nickName = value;
    }
    public override int EntityActorNumber => PlayerPhotonview.CreatorActorNr;




    /// <summary>
    /// Initializes the player with the specified actor number.
    /// </summary>
    /// <param name="actorNumber">The actor number of the player.</param>
    public void InitializePlayer(int actorNumber)
    {
        PlayerPhotonview.RPC("Initialize", RpcTarget.AllBufferedViaServer, actorNumber);
    }

    [PunRPC]
    private void Initialize(int actorNumber)
    {
        Player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerTagObject = Player.TagObject = this;
        EntityName = Player.NickName;
        gameObject.name = Player.NickName;
        transform.SetParent(GameSceneReferences.Manager.PlayersContainer);
    }
}