using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.SceneManagement;

public class SaveFileHandler : MonoBehaviour
{
    public InventoryObject playerinventory;
    public InventoryObject playerequipment;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            OnLoad();
        }
        if (GameManager.IsNewGame == false)
        {
            OnLoad();
        }
        GameManager.IsNewGame = false;
    }
    public void OnSave()
    {
        SerializationManager.Save("savefile", SaveData.current);
    }

    public void OnLoad()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/savefile.save");
        GameEventManager.Raise(new LoadEvent());
    }
}
