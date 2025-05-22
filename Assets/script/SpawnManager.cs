using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Enemies;
    protected HeroKnight Player;

    public float timeBetweenWaves = 5f; 
    private int enemiesToSpawn;
    private int aliveEnemies = 0;

    private bool isSpawning;


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
        // Starta n�sta level n�r alla fiender �r d�da
        if (aliveEnemies == 0 && !isSpawning)
        {
            PlayerData.level++;
            StartCoroutine(HandleWaveSpawning());
        }
    }

    // Hanterar v�ntetid + spawn av level
    IEnumerator HandleWaveSpawning()
    {
        isSpawning = true;

        Debug.Log($"level {PlayerData.level} startar om {timeBetweenWaves} sekunder");
        yield return new WaitForSeconds(timeBetweenWaves);

        enemiesToSpawn = 1 + (PlayerData.level - 1) * 2; // Antal fiender �kar 2 g�nger per level
        Debug.Log($"V�g {PlayerData.level} har {enemiesToSpawn} fiender.");

        for (int i = 1; i < enemiesToSpawn;)
        {
            i++;
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); // Delay mellan varje spawn
        }
        isSpawning = false;
    }

    // funktion f�r att spawna enemy p� ett random location
    public void SpawnEnemy()
    {
        int index = Random.Range(0, Enemies.Length);
        GameObject enemy = Instantiate(Enemies[index], RandomSpawnPoint(), Enemies[index].transform.rotation);

        aliveEnemies++; // H�ller koll p� levande fiender

        // N�r fienden d�r, minska r�knaren
        enemy.GetComponent<Enemies>().animation.SetBool("dead", false);
        StartCoroutine(WatchEnemyDeath(enemy.GetComponent<Enemies>()));
    }

    // skaffa en random spawn point
    Vector3 RandomSpawnPoint()
    {
        float x_right = Random.Range(Player.transform.position.x + 5f, Player.transform.position.x + 10f);
        float x_left = Random.Range(Player.transform.position.x - 10f, Player.transform.position.x - 5f);
        float y = Random.Range(Player.transform.position.y, Player.transform.position.y + 3f);

        int spawnSide = Random.Range(0, 2); // 0 = v�nster, 1 = h�ger
        return new Vector3(spawnSide == 0 ? x_left : x_right, y, 0f);
    }

    // V�ntar p� att fienden ska d�
    IEnumerator WatchEnemyDeath(Enemies enemy)
    {

        while (enemy.Enemy_Health > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f); // V�nta s� animationen hinner spelas upp
        aliveEnemies--;
    }

}
