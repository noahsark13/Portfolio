using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField]
    GameObject scrap;

    Vector3 objectPosition;
    Vector3 direction;
    Vector3 velocity = Vector3.zero;

    float speed = 4.0f;

    float maxTime = 5;
    float currentTime = 0;

    bool enteredRegion = false;
    

    Camera cam;
    float height;
    float width;

    




    // Start is called before the first frame update
    void Start()
    {
        

        objectPosition = transform.position;
        direction = Vector3.left;
        //direction = Random.insideUnitCircle.normalized;

        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

    }

    // Update is called once per frame
    void Update()
    {

        if (enteredRegion)
        {
            currentTime += Time.deltaTime;

            // time to change directions
            if (currentTime >= maxTime)
            {
                direction = Random.insideUnitCircle.normalized;
                currentTime = 0;
            }




            // keep in bounds
            // in futureu update fix this so plane can get semi out of bounds without getting deleted xd
            if ((transform.position.x > width / 2) || (transform.position.x < (0)))
            {
                direction = new Vector3(-1 * direction.x, direction.y, direction.z);

            }
            if (transform.position.y < -1 * (height / 2) + 0.5)
            {
                direction = new Vector3(direction.x, -1 * direction.y, direction.z);
            }
            if (transform.position.y > (height / 2) - 0.5)
            {
                direction = new Vector3(-1 * direction.x, -1 * direction.y, direction.z);
            }
        }

        
        


        velocity = direction * speed * Time.deltaTime;

        objectPosition += velocity;

        transform.position = objectPosition;

        if (!enteredRegion)
        {
            if (transform.position.x < (width / 2))
            {
                enteredRegion = true;
                direction = Random.insideUnitCircle.normalized;
            }

        }


    }
    



}
