using UnityEngine;
using Photon.Pun;

public class EntityIndexManager: MonoBehaviourPun
{
    [Header("Entity & Team Indexes")]
    [SerializeField] protected EntityIndex _entityIndex;
    [SerializeField] protected TeamIndex _teamIndex;

    [Header("EntityManager")]
    [SerializeField] protected EntityManager _entityManager;

    public EntityManager EntityManager => _entityManager;
    public EntityIndex EntityIndex => _entityIndex;
    public TeamIndex TeamIndex => _teamIndex;
    public int EntityNumber => (int)EntityIndex;
    public int GroupNumber => (int)TeamIndex;
}