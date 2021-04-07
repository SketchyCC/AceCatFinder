using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject Credits;
    public GameObject Mewseum;
    public GameObject NoTouching;
    public GameObject BGImage;
    public GameObject TeamLogo;
    public PopulateMewseum mewseumscript;
    public GameObject Mainmenu;
    public Button ResumeButton;

    public Slider slider;

    public void Start()
    {
        NoTouching.SetActive(false);
        if (SaveData.current == null)
        {
            ResumeButton.interactable = false;
        }
        else
        {
            ResumeButton.interactable = true;
        }
        loadingScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        BackToMain();

    }

    public void LoadNewGame(int sceneIndex)
    {
        NoTouching.SetActive(true);
        GameManager.IsNewGame = true;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void ContinueGame(int sceneIndex)
    {
        NoTouching.SetActive(true);
        GameManager.IsNewGame = false;
        Time.timeScale = 1f;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
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
    }

    public void GoToScreen(GameObject screen)
    {
        Mainmenu.SetActive(false);
        BGImage.SetActive(false);

        screen.SetActive(true);

        if (mewseumscript.gameObject.activeSelf)
        {
            TeamLogo.SetActive(false);
            mewseumscript.CheckUnlockedImages();
        }
    }

    public void BackToMain()
    {
        Credits.SetActive(false);
        Mewseum.SetActive(false);
        Mainmenu.SetActive(true);
        BGImage.SetActive(true);
        TeamLogo.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
