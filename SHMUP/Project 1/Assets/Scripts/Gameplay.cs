using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    GameObject bigEnemyPrefab;

    CollisionManager collisionManager;

    [SerializeField]
    int points = 0;
    [SerializeField]
    int scrap = 0;



    int round = 0;

    int timesToSpawn = 5;
    int spawnPerLoop;

    bool gameOver = false;
    bool inRound = false;
    bool coroutineRunning = false;

    [SerializeField]
    bool shopOpen = false;


    public bool ShopOpen
    {
        get { return shopOpen; }
        set { shopOpen = value; }
    }
   
    public int Points
    {
        get { return points; }
        set { points = value; }
    }


    public int Scrap
    {
        get { return scrap; }
        set { scrap = value; }
 
    }



    // Start is called before the first frame update
    void Start()
    {
        collisionManager = GameObject.FindAnyObjectByType<CollisionManager>();
        spawnPerLoop = (1 + (round / 3));
    }

    // Update is called once per frame
    void Update()
    {
        if (!inRound)
        {
            Debug.Log("Round start");
            StartCoroutine(SpawnEnemies());
            coroutineRunning = true;
            inRound = true;
        }

        if (inRound && !coroutineRunning && collisionManager.Enemies.Count == 0)
        {
            if (!shopOpen)
            {
                gameObject.GetComponent<ShopHandler>().OpenShop();
            } 
            else
            {
                round++;
                // increases every 3 rounds
                spawnPerLoop = (1 + (round / 3));
                inRound = false;
                shopOpen = false;
            }

            
  
            
            
        }

        //if (!shopOpen)
        //{
        //    round++;
        //    // increases every 3 rounds
        //    spawnPerLoop = (1 + (round / 3));
        //    inRound = false;
        //}
    }

    IEnumerator SpawnEnemies()
    {
        for (int x = 0; x < timesToSpawn; x++) 
        {
           

            for (int i = 0; i < spawnPerLoop; i++)
            {
                if (gameOver)
                {
                    break;
                }

                float rand = Random.value;

                // big boy spawn
                if (rand < 0.5)
                {
                    GameObject b = Instantiate(bigEnemyPrefab, new Vector3(13, Random.Range(3f, -3f), 0), Quaternion.Euler(0, 0, 90));

                    b.GetComponent<Health>().MaxHealthValue = 80 + (10 * round);
                    b.GetComponent<Health>().HealthValue = 80 + (10 * round);

                    collisionManager.Enemies.Add(b.GetComponent<SpriteInfo>());
                }
                else
                {
                    GameObject e = Instantiate(enemyPrefab, new Vector3(13, Random.Range(3.5f, -3.5f), 0), Quaternion.Euler(0, 0, 90));

                    e.GetComponent<Health>().MaxHealthValue = 50 + (5 * round);
                    e.GetComponent<Health>().HealthValue = 50 + (5 * round);

                    e.GetComponent<EnemyProjectile>().MaxFired = 5 + (round / 2);

                    collisionManager.Enemies.Add(e.GetComponent<SpriteInfo>());
                }

               

            }

            if (gameOver)
            {
                Debug.Log("break");
                yield break;
                
            }

            yield return new WaitForSeconds(spawnPerLoop * ((50 + (5 * round)) / 4));

        }

        coroutineRunning = false;

        
    }

    public void AddPoints(int add)
    {
        points += add;

        player.GetComponent<HealthUI>().UpdateUI();
    }

    public void EndGame()
    {
        gameOver = true;

        Destroy(player);

        for (int i = collisionManager.Enemies.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            Destroy(collisionManager.Enemies[i].gameObject);
            collisionManager.Enemies.Remove(collisionManager.Enemies[i]);
            
        }

        gameObject.GetComponent<GameOver>().ShowOver();
    }
}
