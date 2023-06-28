using UnityEngine;
using Pautik;

[RequireComponent(typeof(CanvasGroup))]
public class BaseMainHUDTab : MonoBehaviour, IMainHUDTab
{
    protected CanvasGroup _canvasGroup;

    protected virtual bool IsActive
    {
        get
        {
            return _canvasGroup?.interactable ?? false;
        }
    }




    protected virtual void Awake()
    {
        GetCanvasGroup();
    }

    /// <summary>
    /// Retrieves the CanvasGroup component from the game object.
    /// </summary>
    protected virtual void GetCanvasGroup()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
    }

    public virtual void Open()
    {       
        MainHUD.Manager.OpenMainHUDTab(this);
        SetCanvasGroupActive();
    }

    public virtual void Close()
    {
        SetCanvasGroupActive(false);
    }

    public void SetCanvasGroupActive(bool isActive = true)
    {
        if(_canvasGroup == null)
        {
            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}