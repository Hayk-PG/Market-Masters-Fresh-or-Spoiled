using UnityEngine;
using TMPro;

public class CustomInputField : MonoBehaviour, IReset
{
    [Header("TMP InputField")]
    [SerializeField] private TMP_InputField _inputField;

    /// <summary>
    /// The text value of the input field.
    /// </summary>
    public string Text
    {
        get
        {
            return _inputField.text;
        }
        set
        {
            _inputField.text = value;
        }
    }

    /// <summary>
    /// Sets the input field text to its default value.
    /// </summary>
    public void SetDefault()
    {
        Text = "";
    }
}