using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class PosterDisplay : MonoBehaviour
{
    public PosterObject poster;  

    public Text CatName;
    public Text LocationHint;
    public Text Description;
    public Text MoneyAward;
    public Text TimeLimit;

    public Image CatImage;
    public Image CatImage2;

    public float zoomFactor;
    private bool zoom = false;
    private Vector3 cornerPlace;

    private void Awake()
    {
        CatName.text = poster.CatName;
        LocationHint.text = poster.LocationHint;
        Description.text = poster.Description;
        MoneyAward.text = poster.MoneyAward.ToString();
        TimeLimit.text = poster.TimeLimit.ToString();

        CatImage.sprite = poster.CatImage;
        CatImage2.sprite = poster.CatImage2;
    }
    private void Update()
    {                
        if (poster.posterProgress == PosterProgress.In_Progress && Input.GetKeyDown(KeyCode.R))
        {
            PosterAccept();
            //actually should check first for the first one in the row
        }
        if (poster.posterProgress == PosterProgress.Complete)
        {
            //play some animation first that the poster is done with a checkmark or something
            Destroy(gameObject); 
        }
    }

    public void PosterAccept()
    {
        if (poster.posterProgress == PosterProgress.In_Progress)
        {
            if (zoom)
            {
                transform.localScale /= zoomFactor;
                transform.SetPositionAndRotation(cornerPlace, Quaternion.identity);
                zoom = false;

            }
            else if (!zoom)
            {
                cornerPlace = transform.position;
                float x = (Screen.width / 2);
                float y = (Screen.height /2);
                transform.SetPositionAndRotation(new Vector3(x,y,0f), Quaternion.identity);
                transform.localScale *= zoomFactor;
                zoom = true;
                GameEventManager.Raise(new PosterSoundEvent());
            }

        }

        if (poster.posterProgress == PosterProgress.Posted)
        {
            if (zoom)
            {
                poster.posterProgress = PosterProgress.In_Progress; //sets poster status to in progress            
                GameEventManager.Raise(new PosterAcceptedEvent(poster, gameObject));
                gameObject.GetComponentInChildren<PosterConfirm>().Askpanel.SetActive(false);
                zoom = false;                
            } else if (!zoom)
            {                
                zoom = true;
                if(gameObject != null) { GameEventManager.Raise(new PosterLookingEvent(gameObject)); }
                
            }
        }        
    }

    public void PosterDecline()
    {
        zoom = false;
        GameEventManager.Raise(new PosterLookingEvent(gameObject));
    }
}
