using UnityEngine;

public class MainHUD : MonoBehaviour
{
    public static MainHUD Manager { get; private set; }

    private IMainHUDTab[] _tabs;




    private void Awake()
    {
        SetInstance();
        GetChildTabs();
    }

    /// <summary>
    /// Sets the instance of the MainHUD manager.
    /// </summary>
    private void SetInstance()
    {
        Manager = this;
    }

    /// <summary>
    /// Retrieves all child main HUD tabs.
    /// </summary>
    private void GetChildTabs()
    {
        _tabs = GetComponentsInChildren<IMainHUDTab>();
    }

    /// <summary>
    /// Opens the specified main HUD tab and closes other tabs.
    /// </summary>
    /// <param name="mainHUDTab">The main HUD tab to open.</param>
    public void OpenMainHUDTab(IMainHUDTab mainHUDTab)
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            if(_tabs[i] == mainHUDTab)
            {
                continue;
            }

            _tabs[i].Close();
        }
    }
}