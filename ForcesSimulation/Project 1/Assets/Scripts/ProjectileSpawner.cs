using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{


    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    CollisionManager collisionManager;

    float cooldown = 0f;
    bool canFire = true;

    [SerializeField]
    int playerDamage = 15;
    [SerializeField]
    float cooldownMax = 1f;

    public int Damage
    {
        get { return playerDamage; }
        set { playerDamage = value; }
    }

    public float CooldownMax
    {
        get { return cooldownMax; }
        set { cooldownMax = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            cooldown += Time.deltaTime;
        }

        if (cooldown > cooldownMax) { canFire = true; cooldown = 0; }

        
    }

    public void Fire()
    {
        if (canFire)
        {
            GameObject b = Instantiate(BulletPrefab, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            b.GetComponent<Projectile>().Direction = Vector3.right;
            collisionManager.PlayerCollidables.Add(b.GetComponent<SpriteInfo>());

            b.GetComponent<Projectile>().Damage = playerDamage;

            canFire = false;
        }
        
        
    }
}
