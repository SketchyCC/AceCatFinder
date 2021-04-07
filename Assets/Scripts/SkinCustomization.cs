using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;

public class SkinCustomization : MonoBehaviour
{
    public Material[] skin;
    SkinnedMeshRenderer skinRenderer;
    int skinIndex;

    void Start()
    {
        skinRenderer = GetComponent<SkinnedMeshRenderer>();
    } 
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<ClothingChangeEvent>(OnClothingUpdate);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<ClothingChangeEvent>(OnClothingUpdate);
    }

    private void OnClothingUpdate(ClothingChangeEvent e)
    {
        GetComponent<SkinnedMeshRenderer>().material = skin[e.clothing];
    }
}
