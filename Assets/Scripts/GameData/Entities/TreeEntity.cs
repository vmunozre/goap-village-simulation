﻿using UnityEngine;

public class TreeEntity : MonoBehaviour
{
    public int age = 1;
    public int wood = 100;
    public int rare = 0;
    public bool chopped = false;
    public bool empty = false;
    public bool viewed = false;

    // Timer
    float timer = 0f;
    float waitTime = 10f;

    //Images
    public Sprite emptyTreeSprite;
    public Sprite clippedTreeSprite;

    void Start()
    {
        // Rare system
        rare = Random.Range(10, 30);
        waitTime += rare;
        float scale = calculateScale();
        transform.localScale = new Vector3(scale, scale, 0f);
    }

    void Update()
    {
        // Evolution tree system
        if (!empty && !chopped && age < 20)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                wood += 50;
                age += 1;
                float scale = calculateScale();
                transform.localScale = new Vector3(scale, scale, 0f);
                timer = 0f;
            }
        }

    }

    public void turnEmptySprite()
    {
        empty = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptyTreeSprite;
    }

    public void turnChoppedSprite()
    {
        empty = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = clippedTreeSprite;
    }

    public void checkChopped()
    {
        chopped = true;
    }

    private float calculateScale()
    {
        return Mathf.Min(1.7f, 1f + (age / 63f));
    }
}
