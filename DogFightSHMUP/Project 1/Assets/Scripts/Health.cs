using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{



    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int health;

    public int HealthValue
    { get { return health; } set { health = value; } }

    public int MaxHealthValue
    { get { return maxHealth; } set { maxHealth = value; } }


    // Start is called before the first frame update
    void Start()
    {
      
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal()
    {
        if (health + 20 >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += 20;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (gameObject == GameObject.Find("Plane"))
            {
                GameObject.Find("GameplayManager").GetComponent<Gameplay>().EndGame();
            }
            else
            {
                GetComponent<Destroy>().Explode();
            }
            
        }

    }


}
