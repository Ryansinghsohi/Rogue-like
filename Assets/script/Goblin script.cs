using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblinscript : Enemies 
{
    // coins drop amount
    public void Awake()
    {
        // coin amount
        Coin_drop = 3;
    }


    public override void Movement()
    {

        // Track player
        if (Player.transform.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-Enemy_Speed, rb.velocity.y);
            sp.flipX = true;
            animation.SetBool("running", true);
        }

        // Track player
        if (Player.transform.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(Enemy_Speed, rb.velocity.y);
            sp.flipX = false;
            animation.SetBool("running", true);
        }

        // stop close to player
        if (Mathf.Abs(Player.transform.position.x - transform.position.x) <= attack_range)
        {
            rb.velocity = new Vector2(0f, 0f);
            animation.SetBool("running", false);
        }


    }
}
