using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
}
