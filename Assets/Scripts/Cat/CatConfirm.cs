using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CatConfirm : MonoBehaviour
{
    int CaughtCat; 
    public GameObject Askpanel;

    private void Start()
    {
        Askpanel.SetActive(false);
    }
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<CatCaughtEvent>(OnCatCaughtEvent);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<CatCaughtEvent>(OnCatCaughtEvent); 
    }

    public virtual void OnCatCaughtEvent(CatCaughtEvent e)
    {
        CaughtCat = e.CatObj; 
        if (Askpanel.activeSelf==false)
        {
            Askpanel.SetActive(true);
            GameEventManager.Raise(new UIOpened(Askpanel.activeSelf, gameObject));
        }

    }

    public void RightCat() //will edit to pass in cat object script
    {
        GameEventManager.Raise(new RightCatEvent(CaughtCat));
        GameEventManager.Raise(new GenericButtonPressedEvent());
        Askpanel.SetActive(false);
        //GameEventManager.Raise(new UIOpened(Askpanel.activeSelf, gameObject));

    }

    public void WrongCat()
    {
        GameEventManager.Raise(new WrongCatEvent(CaughtCat, false));
        GameEventManager.Raise(new GenericButtonPressedEvent());
        Askpanel.SetActive(false);
        GameEventManager.Raise(new UIOpened(Askpanel.activeSelf, gameObject));
    }
}
