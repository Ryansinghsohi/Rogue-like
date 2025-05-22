using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_button_script : MonoBehaviour
{
    public SaveManager saveManager;

    // load new scene and save data
    public void LoadGame(string scene_name) 
    {
        saveManager.LoadGame();
        SceneManager.LoadScene(scene_name);
    }
}
