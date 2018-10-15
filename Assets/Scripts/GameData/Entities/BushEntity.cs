using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushEntity : MonoBehaviour
{
    public int age = 1;
    public int food = 100;
    public int rare = 0;
    public bool collected = false;
    public bool empty = false;

    //Images
    public Sprite emptyBushSprite;

    // Timer
    float timer = 0f;
    float waitTime = 10f;
    
    void Start()
    {
        rare = Random.Range(10, 20);
        waitTime += rare;
    }

    // Update is called once per frame
    void Update()
    {
        if (!collected && age < 15)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                food += 20;
                age += 1;
                // print("Bush grew, age: " + age.ToString());
                timer = 0f;
            }
        }
    }

    public void turnEmptySprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptyBushSprite;
    }
}
