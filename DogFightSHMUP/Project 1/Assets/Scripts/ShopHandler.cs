using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{

    [SerializeField]
    GameObject shopUI;

    [SerializeField]
    GameObject player;

    Health health;
    Gameplay gameplay;
    ProjectileSpawner projectileSpawner;
    HealthUI healthUI;



    // Start is called before the first frame update
    void Start()
    {
        health = player.GetComponent<Health>();

        gameplay = gameObject.GetComponent<Gameplay>();

        projectileSpawner = player.GetComponent<ProjectileSpawner>();

        healthUI = player.GetComponent<HealthUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
    }

    public void IncreaseHealth()
    {
        if (gameplay.Scrap >= 2)
        {
            health.MaxHealthValue += 20;
            health.HealthValue += 20;
            player.GetComponent<HealthUI>().UpdateUI();

            gameplay.Scrap -= 2;

            healthUI.UpdateUI();

            CloseShop();
        }
        
    }

    public void IncreaseDamage() 
    {
        if (gameplay.Scrap >= 3)
        {
            projectileSpawner.Damage += 10;

            gameplay.Scrap -= 3;

            healthUI.UpdateUI();

            CloseShop();
        }
    }

    public void IncreaseFire()
    {
        if (gameplay.Scrap >= 1)
        {
            projectileSpawner.CooldownMax -= 0.1f;

            gameplay.Scrap -= 1;

            healthUI.UpdateUI();

            CloseShop();
        }
    }

    public void CloseShop()
    {
        Debug.Log("trying to closde");
        gameplay.ShopOpen = true;
        shopUI.SetActive(false);
    }

}
