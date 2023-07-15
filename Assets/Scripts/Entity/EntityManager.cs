using Photon.Pun;

public class EntityManager : MonoBehaviourPun
{
    public virtual string EntityName { get; protected set; }
    public virtual int EntityActorNumber { get; }
    public virtual PhotonView PlayerPhotonview { get; }
}
