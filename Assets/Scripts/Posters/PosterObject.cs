using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Poster Mission", menuName = "Poster")]
public class PosterObject:ScriptableObject
{
    //Missions for missing cats
    public string CatName;
    public string LocationHint;
    public string Description;
    public float MoneyAward;
    public float TimeLimit;
    public Sprite CatImage;
    public Sprite CatImage2;
    public TextAsset ReturnedDialogue; //flavour text when you return the cat, not sure where to put this

    public bool IsUnlocked; //for the mewseum

    public Cat CatPrefab; //the cat required to complete the mission
    public GameObject posterPrefab; 


    public PosterProgress posterProgress = PosterProgress.Inactive; //poster status, set to inactive on start, posted when posted, in progress when accepted etc
}

public enum PosterProgress
{
    Inactive,
    Posted,
    In_Progress,
    Complete,
    Failed
}