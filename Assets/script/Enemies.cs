using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class Enemies : MonoBehaviour
{
    public string Enemy_Name;
    public int Enemy_Health;
    public float Enemy_Speed;
    public float attack_range;
    public int Enemy_damage;
    protected HeroKnight Player;
    public GameObject Coin;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public new Animator animation;
    public int current_attack;
    private float timeafterattack;
    public Transform Attack_Point;
    public LayerMask playerlayer;
    public int Coin_drop;


    private void Update()
    {
        Player = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        timeafterattack += Time.deltaTime;
        Movement();
        Attack();
    }

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
    }

    // attack method
    void Attack() 
    {
        // kollar om player �r inom enemy attack distance och om enemy attack �r p� cooldown
        if (attack_range + 1f > Vector3.Distance(transform.position, Player.transform.position) && timeafterattack > 3f)
        {
            // g�r till n�sta attack move
            current_attack++;
            
            // kollar om attack  point finns
            if (Attack_Point != null)
            {
                // kollar f�r layern player som overlapar enemys attack range distance
                Collider2D[] hitplayer = Physics2D.OverlapCircleAll(Attack_Point.position, attack_range + 1f, playerlayer);
                foreach (Collider2D Player in hitplayer)
                {   
                    // anv�nder player take damage funktion f�r att ge damage
                    Player.GetComponent<HeroKnight>().TakeDamage(Enemy_damage); 
                }
            }

            // loop the attack
            if (current_attack >= 3) 
            {
                // resetar till f�rsta attack move
                current_attack = 1;
            }

            // reset attack animation after awhile
            if (timeafterattack > 1.0f)
            {
                // resetar till f�rsta attack move
                current_attack = 1;
            }

            animation.SetTrigger($"attack{current_attack}");

            timeafterattack = 0f;
        }
    }

    // ger alla enemy en egen movement
    public abstract void Movement();

    // destroy enemy anv�nds f�r en invoke
    void Destroy_Enemy()
    {
        Destroy(gameObject);
    }

    // take damage 
    public void TakeDamage(int damage)
    {
        //subtrahera enemy health med en viss damage
        Enemy_Health -= damage;
        // damage animation
        animation.SetTrigger("take_hit");

        // death check
        if (Enemy_Health <= 0f) 
        {
            // run death animation and destroy game object and spawn coin
            animation.SetBool("dead", true);
            PlayerData.level += 1;
            SpawnCoin();
            Invoke("Destroy_Enemy", 1f);
            //SceneManager.LoadScene("");
        }
    }

    // spawn coin
    public void SpawnCoin() 
    {
        // spawna en coin och best�mmer dens v�rde beronde p� enemy
        GameObject newCoin = Instantiate(Coin, transform.position, transform.rotation);
        newCoin.GetComponent<CoinMovement>().value = Coin_drop;
    }
}
