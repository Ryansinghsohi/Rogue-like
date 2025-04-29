using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Enemies;
    protected HeroKnight Player;


    // Start is called before the first frame update
    void Start()
    {
        // Find HeroKnight/player
        Player = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        InvokeRepeating("SpawnEnemy", 0.5f, 10f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy() 
    {
        // which enemy to spawn from enemy list 
        int index = Random.Range(0, Enemies.Length);
        Instantiate(Enemies[index], RandomSpawnPoint(), Enemies[index].transform.rotation);
    }

    Vector3 RandomSpawnPoint() 
    {
        // x-value depending on if it on the left or right 
        float x_right = Random.Range((Player.transform.position.x + 5f), (Player.transform.position.x + 10f));
        float x_left = Random.Range((Player.transform.position.x -5f), (Player.transform.position.x -10f));

        // y-value 
        float y = Random.Range(Player.transform.position.y ,Player.transform.position.y + 3f);

        int SpawnSide = Random.Range(1, 2);

        // spawn enemy on the left
        if (SpawnSide == 1)
        {
            return new Vector3(x_left, y, 0f);
        }

        //spawn enemy on the right 
        if (SpawnSide == 2)
        {
            return new Vector3(x_right, y, 0f);
        }

        // defult spawn
        else
            return new Vector3(10f, -2f, 0f);
    }
}
