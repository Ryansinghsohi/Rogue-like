using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;
using Dan.Models;
using UnityEditor.PackageManager;

public class HighscoreManager : MonoBehaviour
{
    public TMP_InputField userInput;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHighscoreText();
    }

    public void SubmitScore()
    {
        Leaderboards.Myleaderboard.UploadNewEntry(userInput.text, PlayerData.high_score);
        Invoke("UpdateHighscoreText", 0.5f);
    }

    void UpdateHighscoreText()
    {
        Leaderboards.Myleaderboard.GetEntries(OnEntriesLoaded);
    }

    private void OnEntriesLoaded(Entry[] entries)
    {
        string newText = "High level: \n";
        foreach (Entry entry in entries)
        {
            newText += $"{entry.Rank} {entry.Username} - {entry.Score}\n";
        }

        scoreText.text = newText;
    }
}