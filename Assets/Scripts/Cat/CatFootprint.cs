using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFootprint : MonoBehaviour,IPooledObject
{

    public float FootprintLifetime; //set to minutes
    float Timeleft;
    Vector3 OriginalSize;

    private void Start()
    {
        FootprintLifetime *= 60f; //set to seconds
        Timeleft = FootprintLifetime;
        OriginalSize = this.transform.localScale;
    }
    public void OnObjectSpawn()
    {
        Timeleft = FootprintLifetime;
    }

    void Update()
    {
        float PercentTimeleft = Timeleft / FootprintLifetime;

        this.transform.localScale = new Vector3(OriginalSize.x * PercentTimeleft, OriginalSize.y * PercentTimeleft, OriginalSize.z * PercentTimeleft);

        if (Timeleft < 0)
        {
            transform.localScale = Vector3.zero;
        }
        Timeleft -= Time.deltaTime;
    }
}
