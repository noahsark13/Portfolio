using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{

    Health health;



    [SerializeField]
    GameObject barUI;

    [SerializeField]
    Gameplay gameplay;

    RectTransform barFill;
    TextMesh counter;

    [SerializeField]
    TextMesh points;

    [SerializeField]
    TextMesh scrap;

    // Start is called before the first frame update
    void Start()
    {

        health = GetComponent<Health>();

        barFill = barUI.transform.GetChild(0).GetComponent<RectTransform>();
        barFill.sizeDelta = new Vector2(100, 100);

        counter = barUI.transform.GetChild(1).GetComponent<TextMesh>();

        counter.text = health.MaxHealthValue + " / " + health.MaxHealthValue;

        // -----------

        points.text = "Points: " + gameplay.Points.ToString();
        scrap.text = "Scrap: " + gameplay.Scrap.ToString();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {

        //Debug.Log(((float)health.HealthValue / health.MaxHealthValue) * 100);
        barFill.sizeDelta = new Vector2((float)health.HealthValue / health.MaxHealthValue*100, 100);

        counter.text = health.HealthValue + " / " + health.MaxHealthValue;

        points.text = "Points: " + gameplay.Points.ToString();
        scrap.text = "Scrap: " + gameplay.Scrap.ToString();
    }
}
