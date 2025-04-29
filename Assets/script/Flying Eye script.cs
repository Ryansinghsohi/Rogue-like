using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyescript : Enemies
{
    private bool IsNearPlayer = false;

    // coin drop amount instanstate 
    public void Awake()
    {
        Coin_drop = 5;
    }

    // movement funktion
    public override void Movement()
    {
        // track player
        Vector3 move = (Player.transform.position - transform.position).normalized;
        

        // Move only if not near the player
        if (!IsNearPlayer)
        {
            transform.Translate(move * Enemy_Speed * Time.deltaTime);
            animation.SetBool("flying", true);
        }

        // change facing direction
        if (Player.transform.position.x > transform.position.x) 
        {
            sp.flipX = false;
        }

        // change facing direction
        if (Player.transform.position.x < transform.position.x)
        {
            sp.flipX = true;
        }

        // stop movement
        if (Vector3.Distance(transform.position, Player.transform.position) < attack_range) 
        {
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
