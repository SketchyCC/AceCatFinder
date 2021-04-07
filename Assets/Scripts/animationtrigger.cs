using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationtrigger : MonoBehaviour
{
    public void clicksleepbbutton()
    {
        GetComponent<Animator>().SetTrigger("ContinueButton");
    }
}
