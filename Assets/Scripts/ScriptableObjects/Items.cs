using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Items")]
public class Items : ScriptableObject
{
    [Header("Items Collection")]
    [SerializeField] private List<Item> _items;

    public List<Item> Collection => _items;




    public void SetItemsID()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetID(i);
        }
    }
}