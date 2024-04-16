using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    CollisionManager collisionManager;

    Gameplay gameplay;

    [SerializeField]
    GameObject scrap;

    [SerializeField]
    GameObject cog;

    [SerializeField]
    int rewardPoints = 100;

    // Start is called before the first frame update
    void Start()
    {
        collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManager>();
        gameplay = GameObject.Find("GameplayManager").GetComponent<Gameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {

        collisionManager.Enemies.Remove(gameObject.GetComponent<SpriteInfo>());

        gameplay.AddPoints(rewardPoints);
        
        

        float num = Random.value;

        if (num < 0.4)
        {
            GameObject g = Instantiate(cog, gameObject.transform.position, new Quaternion());
            collisionManager.EnemyCollidables.Add(g.GetComponent<SpriteInfo>());
        }
        else if (num < 0.6)
        {
            GameObject g = Instantiate(scrap, gameObject.transform.position, new Quaternion());
            collisionManager.EnemyCollidables.Add(g.GetComponent<SpriteInfo>());
        }

        Destroy(gameObject);
    }



}
