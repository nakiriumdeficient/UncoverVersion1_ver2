using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{

    public void LoadLastSave()
    {
        if (GameManager.Instance.SaveExists())
        {
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogError("No save file found! Cannot load game.");
        }
    }
}
