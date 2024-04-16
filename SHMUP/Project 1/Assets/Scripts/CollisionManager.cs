using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;


public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    SpriteInfo Player;

   
    List<SpriteInfo> playerCollidables = new List<SpriteInfo>();

    List<SpriteInfo> enemyCollidables = new List<SpriteInfo>();

   
    List<SpriteInfo> enemies = new List<SpriteInfo>();

  

    Camera cam;
    float height;
    float width;


    // Start is called before the first frame update
    void Start()
    {

        List<EnemyMovement> enemyCore = new List<EnemyMovement>(GameObject.FindObjectsOfType<EnemyMovement>());
        foreach (EnemyMovement enemy in enemyCore)
        {
            enemies.Add(enemy.GetComponent<SpriteInfo>());
        }


        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

    public List<SpriteInfo> PlayerCollidables
    {
        get { return playerCollidables; }
        set { playerCollidables = value; }
    }

    public List<SpriteInfo> EnemyCollidables
    {
        get { return enemyCollidables; }
        set { enemyCollidables = value; }
    }

    public List<SpriteInfo> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }


    // Update is called once per frame
    void Update()
    {

        // player proj vs enemies
        if (playerCollidables.Count > 0 && enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < playerCollidables.Count; j++)
                {

                    Projectile projComponent = playerCollidables[j].GetComponent<Projectile>();

                    if (AABBCheck(enemies[i], playerCollidables[j]))
                    {

                        if (!projComponent.HasCollided)
                        {
                            enemies[i].IsColliding = true;
                            playerCollidables[j].IsColliding = true;
                            projComponent.HasCollided = true;

                            Health health = enemies[i].GetComponent<Health>();
                            health.TakeDamage(playerCollidables[j].gameObject.GetComponent<Projectile>().Damage);
                        }


                    }
                    else if (enemies[i].IsColliding && playerCollidables[j].IsColliding)
                    {
                        enemies[i].IsColliding = false;
                        playerCollidables[j].IsColliding = false;

                        projComponent.HasCollided = false;

                    }
                }
            }



            for (int i = 0; i < PlayerCollidables.Count; i++)
            {
                if (playerCollidables.Count > 0 && playerCollidables[i].transform.position.x > (width / 2) + 2)
                {
                    Destroy(playerCollidables[i].gameObject);
                    playerCollidables.Remove(playerCollidables[i]);
                    i--;
                }
            }
        }



        

        
        // enemy proj vs player
        if (enemyCollidables.Count > 0 && Player != null)
        {
            for (int i = 0; i < enemyCollidables.Count; i++)
            {
                Projectile projComponent = enemyCollidables[i].GetComponent<Projectile>();

                if (AABBCheck(enemyCollidables[i], Player))
                {
                    enemyCollidables[i].IsColliding = true;
                    Player.IsColliding = true;

                    if (!projComponent.HasCollided)
                    {
                        enemyCollidables[i].IsColliding = true;
                        Player.IsColliding = true;
                        projComponent.HasCollided = true;


                        if (enemyCollidables[i].gameObject.GetComponent<Projectile>().Type == "cog")
                        {
                            Health health = Player.GetComponent<Health>();
                            health.Heal();

                            GameObject.Find("GameplayManager").GetComponent<Gameplay>().AddPoints(50);

                            Destroy(enemyCollidables[i].gameObject);
                            enemyCollidables.Remove(enemyCollidables[i]);
                            i--;

                            Player.IsColliding = false;
                        }
                        else if (enemyCollidables[i].gameObject.GetComponent<Projectile>().Type == "scrap")
                        {
                            GameObject gameplay = GameObject.Find("GameplayManager");
                            gameplay.GetComponent<Gameplay>().Scrap += 1;

                            GameObject.Find("GameplayManager").GetComponent<Gameplay>().AddPoints(75);

                            Destroy(enemyCollidables[i].gameObject);
                            enemyCollidables.Remove(enemyCollidables[i]);
                            i--;

                            Player.IsColliding = false;
                        }
                        else
                        {
                            Health health = Player.GetComponent<Health>();
                            health.TakeDamage(enemyCollidables[i].gameObject.GetComponent<Projectile>().Damage);
                        }
                        

                        Player.GetComponent<HealthUI>().UpdateUI();
                    }

                }
                else if (enemyCollidables[i].IsColliding && Player.IsColliding)
                {
                    enemyCollidables[i].IsColliding = false;
                    Player.IsColliding = false;
                }

 
            }

            for (int i = 0; i < EnemyCollidables.Count; i++)
            {
                if (i < enemyCollidables.Count && enemyCollidables[i].transform.position.x < -1 * (width / 2) - 2)
                {
                    Destroy(enemyCollidables[i].gameObject);
                    enemyCollidables.Remove(enemyCollidables[i]);
                    i--;
                }
            }

        }
        


        
  
    }

    bool AABBCheck(SpriteInfo spriteA, SpriteInfo spriteB)
    {
        return (spriteB.RectMin.x < spriteA.RectMax.x) &&
            (spriteB.RectMax.x > spriteA.RectMin.x) &&
            (spriteB.RectMax.y > spriteA.RectMin.y) &&
            (spriteB.RectMin.y < spriteA.RectMax.y);
    }


}
