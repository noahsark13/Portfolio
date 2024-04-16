using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField]
    GameObject enemyBullet;

 
    CollisionManager collisionManager;

    float maxBulletTime = 2;
    float bulletTime = 0;

    int fireAmount = 0;
    [SerializeField]
    int maxFired = 5;

    bool bulletActive = false;
    float fireDelay = 0;

  



    public int MaxFired
    {
        get { return maxFired; }
        set { maxFired = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime += Time.deltaTime;

        // time to begin firing
        if (bulletTime >= maxBulletTime)
        {
            bulletActive = true;
            bulletTime = 0;
        }

        if (bulletActive)
        {

            fireDelay += Time.deltaTime;


            if (fireDelay >= 0.1)
            {
                Attack();
                fireAmount++;
                fireDelay = 0;
            }

            // bullet delay

            if (fireAmount >= maxFired)
            {
                bulletActive = false;
                fireAmount = 0;
            }



        }
    }

    public void Attack()
    {

        GameObject b = Instantiate(enemyBullet, transform.position, new Quaternion(0, 0, 180, 0));
        b.GetComponent<Projectile>().Direction = Vector3.left;
        collisionManager.EnemyCollidables.Add(b.GetComponent<SpriteInfo>());



    }
}
