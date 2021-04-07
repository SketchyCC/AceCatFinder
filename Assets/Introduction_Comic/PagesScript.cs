﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PagesScript : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private GameObject activePage;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject button;  
    [SerializeField] private GameObject skipButton;
    
    //for disabeling audio when skipping the whole comic
    [SerializeField] private GameObject audioEffects;
    [SerializeField] private GameObject audioSource;
    
    void Start()
    {
        //Sets all Pages inactive
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        activePage = pages[0];
        activePage.SetActive(true);
    }
    public void SetNextPageActive()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].activeInHierarchy == true)
            {
                activePage = pages[i + 1];
                pages[i].SetActive(false);
                activePage.SetActive(true);
                {
                    LoadNewGame(1);
                    button.SetActive(false);
                    skipButton.SetActive(false);
   

    public void SkipComic(int sceneIndex)
        for (int i = 0; i < pages.Length; i++)
        activePage = pages[12];
    
    IEnumerator LoadAsynchronously(int sceneIndex)
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }

        operation.allowSceneActivation = true;
