using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ScriptableObjectsCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Item item = (Item)target;

        if (GUILayout.Button("Set Item Price", GUILayout.Height(40)))
        {
            item.SetItemPrice();
        }
    }
}

[CustomEditor(typeof(Items))]
public class ScriptableObjectsItemsCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Items items = (Items)target;

        if (GUILayout.Button("Set Items ID", GUILayout.Height(40)))
        {
            items.SetItemsID();
        }
    }
}