using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteInfo : MonoBehaviour
{


    [SerializeField]
    Vector2 rectSize = Vector2.one;

    [SerializeField]
    SpriteRenderer sRenderer;

    [SerializeField]
    bool useSprite = false;

    bool isColliding = false;

    public Vector2 RectMin
    {
        get { return (Vector2)transform.position - (rectSize / 2); }
    }

    public Vector2 RectMax
    {
        get { return (Vector2)transform.position + (rectSize / 2); }
    }

    public float X
    {
        get { return transform.position.x; }
    }

    public float Y
    {
        get { return transform.position.y; }
    }

    public bool IsColliding { get { return isColliding; } set { isColliding = value; } }


    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        if (useSprite) { rectSize = sRenderer.bounds.extents * 2; }
        
 
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            sRenderer.color = Color.gray;

        }
        else
        {
            sRenderer.color = Color.white;
        }
    }

    private void OnDrawGizmosSelected()
    {

        if (isColliding)
        {
            Gizmos.color = Color.red;

        }
        else
        {
            Gizmos.color = Color.green;
        }


        Gizmos.DrawWireCube(transform.position, rectSize);





    }
}
