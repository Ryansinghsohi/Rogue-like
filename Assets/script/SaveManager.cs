using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string SavePath;

    // awake �r kallad innan allt
    void Awake()
    {
        // hittar en path till Playerdata
        SavePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }

    // spara data
    public void SaveGame()
    {
        // skapa en container f�r playerdata f�r att den �r en static klass
        PlayerDataContainer container = new PlayerDataContainer();
        container.health = PlayerData.health;
        container.max_health = PlayerData.max_health;
        container.Coins = PlayerData.Coins;
        container.dmg = PlayerData.dmg_multiplier;
        container.level = PlayerData.level;
        container.high_score = PlayerData.high_score;

        // anv�nd en container f�r att skapa en json fill/string
        string datatosave = JsonUtility.ToJson(container, true);

        // spara allt datatosave till savepath
        File.WriteAllText(SavePath, datatosave);
    }

    // ladda sparat spel data
    public void LoadGame()
    {
        // kolla om en fill finns
        if (File.Exists(SavePath))
        {
            // l�ser av och h�mtar data fr�n json save filen
            string dataFromFile = File.ReadAllText(SavePath);
            PlayerDataContainer container = JsonUtility.FromJson<PlayerDataContainer>(dataFromFile);

            // Kopiera tillbaka in i statiska klass PlayerData
            PlayerData.health = container.health;
            PlayerData.max_health = container.max_health;
            PlayerData.Coins = container.Coins;
            PlayerData.dmg_multiplier = container.dmg;
            PlayerData.level = container.level;
            PlayerData.high_score = container.level;

            // s�ger till dig om spelet har laddats
        }
        else
        {
            Debug.LogWarning("Ingen sparfil hittades.");
        }
    }

    [System.Serializable]
    public class PlayerDataContainer
    {
        public int health;
        public int max_health;
        public int Coins;
        public int dmg;
        public int level;
        public int high_score;
    }
}
