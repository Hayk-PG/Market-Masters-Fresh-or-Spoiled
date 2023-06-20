using UnityEngine;

public class PlayerUIGroupManager : MonoBehaviour
{
    [Header("Ownership")]
    [SerializeField] private string _ownerName;
    [SerializeField] private int _ownerActorNumber;
    [SerializeField] private bool _isLocalPlayerOwner;

    private const string _numberTextBounceAnimation = "BounceChoosedNumberTextAnim";

    public string OwnerName => _ownerName;
    public int OwnerActorNumber => _ownerActorNumber;
    public bool IsLocalPlayerOwner => _isLocalPlayerOwner;




    public void SetOwnership(string ownerName, int actorNumber, bool isLocalPlayerOwner = false)
    {
        _ownerName = ownerName;
        _ownerActorNumber = actorNumber;
        _isLocalPlayerOwner = isLocalPlayerOwner;
    }
}