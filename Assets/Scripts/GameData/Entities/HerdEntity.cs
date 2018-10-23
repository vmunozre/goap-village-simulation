using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdEntity : MonoBehaviour {
    public int maxDeer = 5;
    public int deerCount = 0;
    public GameObject deerPrefab;

    private bool control = false;
    //Timer
    private float startTime = 0;
    private float reproductionTime = 10f;
    public float minReproductionTime = 10f;
    public float maxReproductionTime = 30f;
    
    void Start () {
        maxDeer = Random.Range(3, 6);
        calculateReproductionTime();
    }
	
	void Update () {
        if(deerCount < maxDeer)
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > reproductionTime)
            {
                resetTimer();
                GameObject newDeer = Instantiate(deerPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                newDeer.transform.parent = transform;
                deerCount++;
            }
        }
	}

    public void hunted()
    {
        if (!control)
        {
            minReproductionTime *= 1.5f;
            maxReproductionTime *= 1.5f;
            control = true;
        }
        deerCount--;
        resetTimer();
    }

    private void resetTimer()
    {
        startTime = 0;
        calculateReproductionTime();
    }

    private void calculateReproductionTime()
    {
        reproductionTime = Random.Range(minReproductionTime, maxReproductionTime);
    }
}
