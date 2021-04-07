using UnityEngine;
using GameEvents;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    private bool gamePaused = false;
    public GameObject UIPausegame;

    public GameObject[] UIToDeactivate ;
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!gamePaused)
            {
                PausedGame();
            }
            else 
            {
                ResumeGame();
            }
        }            
    }

    public void ResumeGame()
    {    
        UIPausegame.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        GameEventManager.Raise(new GamePausedEvent(gamePaused));

        for (int i = 0; i < UIToDeactivate.Length - 1; i++)
        {
            UIToDeactivate[i].SetActive(true);
        }

    }

    public void PausedGame()
    {
        UIPausegame.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GameEventManager.Raise(new GamePausedEvent(gamePaused));

        for (int i = 0; i < UIToDeactivate.Length - 1; i++)
        {
            UIToDeactivate[i].SetActive(false);
        }
    }   

    public void BackToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }
}
