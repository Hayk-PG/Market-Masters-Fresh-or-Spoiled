using UnityEngine;

public class AuthenticationManager : BaseMainHUDTab
{
    [Header("UI Elements")]
    [SerializeField] private CustomInputField _inputField;
    [SerializeField] private Btn _button;




    private void OnEnable()
    {
        _button.OnSelect += OnAuthenticationConfirm;
    }

    private void OnAuthenticationConfirm()
    {
        print(_inputField.Text);
        Network.Manager.Connect(_inputField.Text, _inputField.Text);
    }
}