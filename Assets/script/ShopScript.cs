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

    private SaveManager saveManager;

    private void Start()
    {
        saveManager = GameObject.FindAnyObjectByType<SaveManager>();
    }

    public void dmg() 
    {
        // check if you have the money to buy
        if (PlayerData.Coins >= dmg_cost) 
        {
            // increase dmg multiplier and decrease the amount of coins
            PlayerData.dmg_multiplier += 1;
            PlayerData.Coins -= dmg_cost;
        }
        // if you don't have the money
        else
        {
            return;
        }
    }

    // Increase max hp after button 
    public void Hp()
    {
        // check if you have the money to buy
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
    }

    void Update_text()
    {
        hp_text.text = "Hp:" + PlayerData.max_health;
        dmg_text.text = "dmg mult: " + PlayerData.dmg_multiplier;
        coins_text.text = "" + PlayerData.Coins;
    }

    private void Update()
    {
        Update_text();
    }
}
