using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public int dmg_cost = 5;
    public int hp_cost = 10;

    public TextMeshProUGUI dmg_text;
    public TextMeshProUGUI hp_text;
    public TextMeshProUGUI coins_text;

    public SaveManager saveManager;

    private void Start()
    {
        //saveManager = GameObject.FindAnyObjectByType<SaveManager>();
    }

    public void dmg() 
    {
        // check if you have the money to buy 100
        if (PlayerData.Coins >= dmg_cost * 100) 
        {
            // increase dmg multiplier and decrease the amount of coins
            PlayerData.dmg_multiplier += 100;
            PlayerData.Coins -= dmg_cost*100;
        }

        // check if you can buy 10 
        if (PlayerData.Coins >= dmg_cost * 10)
        {
            // increase dmg multiplier and decrease the amount of coins
            PlayerData.dmg_multiplier += 10;
            PlayerData.Coins -= dmg_cost * 10;
        }

        // check if you can buy 1
        if (PlayerData.Coins >= dmg_cost * 1)
        {
            // increase dmg multiplier and decrease the amount of coins
            PlayerData.dmg_multiplier += 1;
            PlayerData.Coins -= dmg_cost * 1;
        }

        // if you don't have the money
        else
        {
            return;
        }
        saveManager.SaveGame();
    }

    // Increase max hp after button 
    public void Hp()
    {
        // check if you have the money to buy 100
        if (PlayerData.Coins >= hp_cost * 100)
        {
            // increase the max hp and decrease the amount of coins
            PlayerData.max_health += 1000;
            PlayerData.Coins -= hp_cost * 100;
        }

        // check if you have the money to buy 10 
        if (PlayerData.Coins >= hp_cost*10)
        {
            // increase the max hp and decrease the amount of coins
            PlayerData.max_health += 100;
            PlayerData.Coins -= hp_cost * 10;
        }

        // check if you have the money to buy 1
        if (PlayerData.Coins >= hp_cost)
        {
            // increase the max hp and decrease the amount of coins
            PlayerData.max_health += 10;
            PlayerData.Coins -= hp_cost;
        }
        // if you don't have the money
        else
        {
            return;
        }
        saveManager.SaveGame();
    }

    // funktion att updatera alla text som hp, dmg och coins texten
    void Update_text()
    {
        hp_text.text = "Hp:" + PlayerData.max_health;
        dmg_text.text = "dmg mult: " + PlayerData.dmg_multiplier;
        coins_text.text = "Coins:" + PlayerData.Coins;
    }


    //updatera varje frame
    private void Update()
    {
        Update_text();
    }
}
