using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblinscript : Enemies 
{
    // coins drop amount
    public void Awake()
    {
        // coin amount
        Coin_drop = 5 + 2 * PlayerData.level;
        Enemy_Health = 50 + 10 * PlayerData.level;
        Enemy_damage = 10 + 5 * PlayerData.level;
    }

    // Goblins movement
    public override void Movement()
    {
        // Track player to the left 
        if (Player.transform.position.x < transform.position.x)
        {
            // Goblin to player vector
            rb.velocity = new Vector2(-Enemy_Speed, rb.velocity.y);
            animation.SetBool("running", true);
        }

        // Track player to the right
        if (Player.transform.position.x > transform.position.x)
        {
            // Goblin to player vector 
            rb.velocity = new Vector2(Enemy_Speed, rb.velocity.y);
            animation.SetBool("running", true);
        }

        // stop close to player if its in Goblins attack range
        if (Mathf.Abs(Player.transform.position.x - transform.position.x) <= attack_range)
        {
            // stop Goblin movement and running animation
            rb.velocity = new Vector2(0f, 0f);
            animation.SetBool("running", false);
        }
    }
}
