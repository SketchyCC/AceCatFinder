using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateMewseum : MonoBehaviour
{
    public GameObject prefab;
    public GameManager gameManager;
    List<Image> images=new List<Image>();
    int numberToCreate;

    void Start()
    {
        numberToCreate = gameManager.SetPosters.Length;
        if (SaveData.current.posterData != null)
        {
            for (int i = 0; i < numberToCreate; i++)
            {
                gameManager.SetPosters[i].IsUnlocked = SaveData.current.posterData[i].Unlocked;
            }
        }
        Populate();    
    }

    void Populate()
    {
        GameObject newObj;

        for(int i=0; i<numberToCreate; i++)
        {
            newObj = Instantiate(prefab, transform);
            images.Add(newObj.GetComponent<Image>());
            if (gameManager.SetPosters[i].IsUnlocked==true)
            {
                images[i].sprite = gameManager.SetPosters[i].CatImage;
            }

        }
    }
    public void CheckUnlockedImages()
    {
        for (int i = 0; i < numberToCreate; i++)
        {
            if (gameManager.SetPosters[i].IsUnlocked==true)
            {
                images[i].sprite = gameManager.SetPosters[i].CatImage;
                images[i].color = Color.white;
            }

        }
    }
}
