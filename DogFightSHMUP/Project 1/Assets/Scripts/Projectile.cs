using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 objectPosition;
    [SerializeField]
    Vector3 direction;
    Vector3 velocity = Vector3.zero;

    [SerializeField]
    CollisionManager collisionManager;

    [SerializeField]
    float speed = 8.0f;

    [SerializeField]
    int damage = 5;

    Camera cam;
    float height;
    float width;

    [SerializeField]
    bool hasCollided = false;

    [SerializeField]
    string type;


    public string Type
    { get { return type; } }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public bool HasCollided
    {
        get { return hasCollided; }
        set { hasCollided = value; }
    }

    public Vector3 Direction
    {
        set { direction = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManager>();

        objectPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        velocity = direction * speed * Time.deltaTime;

        objectPosition += velocity;

        transform.position = objectPosition;


    }


}
