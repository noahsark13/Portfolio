using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 objectPosition = Vector3.zero;
    [SerializeField]
    Vector3 direction = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    Vector3 windDirection = Vector3.left;

    Camera cam;
    float height;
    float width;

    float baseSpeed = 6.0f;
    float speed = 6.0f;


    void Start()
    {
        objectPosition = transform.position;

        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        Debug.Log(width / 2);

        
    }

    // Update is called once per frame
    void Update()
    {
    
        
        if (direction == (Vector3.left*2) || direction == Vector3.zero)
        {
            speed = baseSpeed;
        }
        else
        {
            speed = baseSpeed * (Mathf.Pow(((-1 * transform.position.x)) / 5, 2));
        }



        velocity = (windDirection + direction) * speed * Time.deltaTime;

        objectPosition += velocity;

        if (objectPosition.x > (-1 * width/2) && objectPosition.y > (-1 * height / 2) && objectPosition.y < (height / 2))
        {
            transform.position = objectPosition;
        }

      

           

       
        

       
    }

    public void MoveDirection(Vector3 newDirection)
    {
        objectPosition = transform.position;

        if (newDirection.normalized == Vector3.right)
        {
            direction = newDirection.normalized * 2;

        }
        else
        {
            direction = newDirection.normalized;
        }
     

        
    }
}