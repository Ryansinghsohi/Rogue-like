using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    protected HeroKnight Player;
    public float timer;

    public bool MoveToPlayer;
    public float speed;
    private Rigidbody2D rb;
    public CircleCollider2D cc;

    public int value;

    // Start is called before the first frame update
    void Start()
    {
        // get player and rigidbody
        Player = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        cc = gameObject.GetComponent<CircleCollider2D>();
    }

    // update per frame
    private void FixedUpdate()
    {
        // move to player after one second 
        if (MoveToPlayer == false) 
        {
            if(timer < 1) 
            {
                timer += Time.fixedDeltaTime;
            }
            else 
            {
                MoveToPlayer = true;
                cc.enabled = false;
                rb.gravityScale = 0;
            }
        }

        // coin to player path
        if(MoveToPlayer == true) 
        {

            Vector3 movement = (Player.transform.position - transform.position).normalized;
            rb.velocity = movement * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") 
        {
            PlayerData.Coins += value;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
