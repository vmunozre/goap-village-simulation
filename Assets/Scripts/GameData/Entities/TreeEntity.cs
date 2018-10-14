using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntity : MonoBehaviour
{
    public int age = 1;
    public int wood = 100;
    public int rare = 0;
    public bool clipped = false;
    // Timer
    float timer = 0f;
    float waitTime = 10f;

    public void checkChopped()
    {
        clipped = true;
    }
    // Use this for initialization
    void Start()
    {
        rare = Random.Range(10, 30);
        print("Rare: " + rare.ToString());
        waitTime += rare;
        float scale = calculateScale();
        transform.localScale = new Vector3(scale, scale, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!clipped && age < 20)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                wood += 50;
                age += 1;
                float scale = calculateScale();
                transform.localScale = new Vector3(scale, scale, 0f);
                print("Tree grew, age: " + age.ToString());
                timer = 0f;
            }
        }

    }

    private float calculateScale()
    {
        return Mathf.Min(1.7f, 1f + (age / 63f));
    }
}
