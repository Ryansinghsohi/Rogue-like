using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class mainmenuscript : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    public SaveManager saveManager;

    void UpdateText()
    {
        saveManager.LoadGame();
        HighScoreText.text = "Highest level: " + PlayerData.high_score;
    }

    // load new scene and save data
    public void LoadGame(string scene_name)
    {
        saveManager.LoadGame();
        SceneManager.LoadScene(scene_name);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
}
