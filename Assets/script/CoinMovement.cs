using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    protected HeroKnight Player;
    public float timer;

    public bool MoveToPlayer;
    public float speed;
    public Rigidbody2D rb;

    private bool collectable = false;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // update per frame
    private void FixedUpdate()
    {
        if (MoveToPlayer == false) 
        {
            if(timer < 1) 
            {
                timer += Time.fixedDeltaTime;
            }
            else 
            {
                collectable = true;
                MoveToPlayer = true;
                rb.gravityScale = 0;
            }
        }

        if(MoveToPlayer == true) 
        {

            Vector3 movement = (Player.transform.position - transform.position).normalized;
            rb.velocity = movement * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && collectable) 
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
