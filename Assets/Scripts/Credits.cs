using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Credits : MonoBehaviour
{
    public GameObject credits;
    private Vector3 creditsObject;

    public float startposition;
    public float endpositon;
    public float movespeed;

    private float yAdittion;
    private float yPosition;



    void Start()
    {
        creditsObject = credits.GetComponent<RectTransform>().position;
        credits.GetComponent<RectTransform>().position = new Vector3 (creditsObject.x, startposition, 0);
        yAdittion = Time.deltaTime / movespeed;
        yPosition = startposition;
    }

    void Update()
    {
        if(creditsObject.y < endpositon)
        {
            yPosition += yAdittion;
            credits.GetComponent<RectTransform>().position = new Vector3(creditsObject.x, yPosition, 0);
        }
        
    }

    public void resetPosition()
    {
        credits.GetComponent<RectTransform>().position = new Vector3(creditsObject.x, startposition, 0);
        yPosition = startposition;
    }
}
