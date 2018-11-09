using UnityEngine;

public class HerdEntity : MonoBehaviour {
    // Deer reference
    public GameObject deerPrefab;
    // Counters
    public int maxDeer = 5;
    public int deerCount = 0;
    // Control hunted 
    private bool control = false;
    //Timers
    private float startTime = 0;
    private float baseReproductionTime = 10f;
    private float reproductionTime = 10f;
    public float minReproductionTime = 10f;
    public float maxReproductionTime = 30f;
    
    void Start () {
        maxDeer = Random.Range(3, 6);
        calculateReproductionTime();
    }
	
	void Update () {
        reproductionTime = baseReproductionTime / GameManager.instance.actualMuti;
        // Deer counters
        if (deerCount < maxDeer)
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
    // Deer hunted
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
        baseReproductionTime = Random.Range(minReproductionTime, maxReproductionTime);
    }
}
