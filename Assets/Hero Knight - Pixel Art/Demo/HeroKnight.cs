﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] GameObject m_slideDust;


    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_isDead = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    private float mouseWorldPos;
    public Transform Attack_Point;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;
    public int attackDamage = 10;
    private bool isBlocking;
    public Image Healthbar;
    public TextMeshProUGUI Coins_ui;
    public TextMeshProUGUI level_ui;
    public SaveManager saveManager;
    private Vector3 spawnPosition = new Vector3(0f, -3f, 0f);


    // Used this for initialization
    void Start()
    {
        PlayerData.level = 1;
        saveManager.LoadGame();
        PlayerData.health = PlayerData.max_health;
        PlayerData.level = 1;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // used for the invoke to play the death scene
    public void PlayDeathScene()
    {
        saveManager.SaveGame();
        SceneManager.LoadScene("death");
    }

    // player takes damage
    public void TakeDamage(int damage)
    {
        //Check if the player is blocking when attacking
        if (isBlocking == false)
        {
            PlayerData.health -= damage;
            m_animator.SetTrigger("Hurt");
        }
        // ignore damage if player is blocking
        else
            return;
    }

    // update the display count for coins
    void UpdateUIText()
    {
        // change the amount of coins displayed
        Coins_ui.text = "Coins:" + PlayerData.Coins.ToString();
        // update level text
        level_ui.text = "level: " + PlayerData.level;

    }

    //  upadate health bar fill amount
    void UpdateHealthBar()
    {
        // change the fill amount depending on the players health
        Healthbar.fillAmount = (float)PlayerData.health / (float)PlayerData.max_health;  
    }

    // Update is called once per frame
    void Update ()
    {
        // upadate health bar fill amount and coins
        UpdateHealthBar();
        UpdateUIText();


        // reset player position to middle
        if(gameObject.transform.position.x < -70  || gameObject.transform.position.x > 70f || gameObject.transform.position.y < -5f)
        {
            gameObject.transform.position = spawnPosition;
        }


        // checking if the level is higher than high score 
        if (PlayerData.level > PlayerData.high_score)
        {
            // if yes, then change high score to the level and reset level
            PlayerData.high_score = PlayerData.level;
        }

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");


        // mouse position 
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        // swap sprite directrion depending on mouse
        if (transform.position.x < mouseWorldPos)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (transform.position.x > mouseWorldPos)
        { 
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }


        // is dead 
        if (m_isDead == true) 
        {
            Invoke("PlayDeathScene", 1f);
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (PlayerData.health <= 0 && !m_rolling)
        {
            m_isDead = true;
            m_animator.SetTrigger("Death");
        }



        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 1f && !m_rolling)
        {
            m_currentAttack++;

            //give damage to enemy
            if (Attack_Point != null)
            {
                Collider2D[] hitenemy = Physics2D.OverlapCircleAll(Attack_Point.position, attackrange, enemylayer);
                foreach (Collider2D enemy in hitenemy)
                {
                    enemy.GetComponent<Enemies>().TakeDamage(PlayerData.dmg_multiplier * attackDamage);
                }
            }


            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 2.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            isBlocking = true;
        }

        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
            isBlocking = false;
        }


        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}