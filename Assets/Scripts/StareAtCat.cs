using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareAtCat : MonoBehaviour
{
    public List<Transform> catsInFront;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cat")
        {
            catsInFront.Add(other.gameObject.GetComponent<Transform>().transform);
            CheckForClosest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.tag == "Cat")
        {
            for (int i = 0; i < catsInFront.Count; i++)
            {
                if (other.gameObject.transform == catsInFront[i])
                {
                    catsInFront.RemoveAt(i);
                    break;
                }
            }
            CheckForClosest();
        }
    }

    //This here should remove a cat once the mission is completed and the cat deactivated
    public void ReadyToRemoveCat( int CatToRemove)
    {
        for (int i = 0; i < catsInFront.Count; i++)
        {
            int catID = catsInFront[i].GetComponentInParent<Cat>().CatId;
            if (catID == CatToRemove)
            {
                catsInFront.RemoveAt(i);
                break;
            }
        }
        CheckForClosest();
    }

    void CheckForClosest()
    {
        if(catsInFront.Count == 1)
        {
            GetComponentInParent<NetCatching>().catInFront = catsInFront[0];
        } else if (catsInFront.Count <= 0)
        {
            GetComponentInParent<NetCatching>().catInFront = null;
        }
        else if(catsInFront.Count >= 2)
        {
            int closestCat = 0;
            for (int i = -1 + catsInFront.Count; i <= 0; i--)
            {
                Transform playerPos = gameObject.GetComponentInParent<NetCatching>().GetComponentInParent<MovementScript>().transform;
                if (Vector3.Distance(playerPos.position, catsInFront[i].position) >= Vector3.Distance(playerPos.position, catsInFront[i--].position))
                {
                    closestCat = i;
                }
                else { closestCat = i--; }                
            }
            GetComponentInParent<NetCatching>().catInFront = catsInFront[closestCat];
        }      
    }
}
