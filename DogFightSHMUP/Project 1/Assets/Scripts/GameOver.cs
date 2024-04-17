using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    GameObject gameOver;

    [SerializeField]
    GameObject healthUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOver()
    {

        healthUI.SetActive(false);

        int s = gameObject.GetComponent<Gameplay>().Points;
    
        gameOver.transform.GetChild(1).GetComponent<TextMesh>().text = "SCORE: " + s;
        gameOver.SetActive(true);
    }
}
