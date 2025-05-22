using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyescript : Enemies
{
    private bool IsNearPlayer = false;

    // coin drop amount instanstate 
    public void Awake()
    {
        Coin_drop = 10 + 3 * PlayerData.level;
        Enemy_Health = 100 + 20 * PlayerData.level;
        Enemy_damage = 20 + 10 * PlayerData.level;
    }

    // movement funktion
    public override void Movement()
    {
        // track player
        Vector3 move = (Player.transform.position - transform.position).normalized;
        

        // Move only if not near the player
        if (!IsNearPlayer)
        {
            // move flying eye towards player
            transform.Translate(move * Enemy_Speed * Time.deltaTime);
            animation.SetBool("flying", true);
        }


        // stop movement if player in flying eyes attack range
        if (Vector3.Distance(transform.position, Player.transform.position) < attack_range) 
        {
            // stop movement 
            IsNearPlayer = true;
            animation.SetBool("flying", false);
        }

        // Reset movement when player moves away
        else
        {
            IsNearPlayer = false;
        }
    }
}
