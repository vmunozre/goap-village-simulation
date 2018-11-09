using UnityEngine;

public class BushEntity : MonoBehaviour
{
    public int age = 1;
    public int food = 100;
    public int rare = 0;
    // States
    public bool collected = false;
    public bool empty = false;
    public bool viewed = false;
    // Sprites
    public Sprite emptyBushSprite;

    // Timers
    private float timer = 0f;
    private float baseWaitTime = 10f;
    private float waitTime = 10f;
    
    void Start()
    {
        age = Random.Range(1, 4);
        food = 100 + (20 * age);
        rare = Random.Range(10, 20);
        baseWaitTime += rare;
    }

    void Update()
    {
        waitTime = baseWaitTime / GameManager.instance.actualMuti;
        if (!collected && age < 15)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                food += 20;
                age += 1;
                viewed = false;
                timer = 0f;
            }
        }
    }

    public void turnEmptySprite()
    {
        empty = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptyBushSprite;
    }
}
