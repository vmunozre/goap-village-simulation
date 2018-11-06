using UnityEngine;

public class OrchardBuilding : BaseBuilding {
    public bool full = false;
    // Farmers counter
    public int farmers = 0;
    // Max workers
    public int limitWorkers = 3;
    // Effort and progress
    public float effort = 20f;
    public float farmProgress = 0f;
    // Food counters
    public int food = 0;
    // Harvest Sprite
    public Sprite spriteHarvest;
    private SpriteRenderer sr;

	void Start () {
        // Generate limitWorkers
        limitWorkers = Random.Range(3,8);
        sr = GetComponent<SpriteRenderer>();
    }

    // Check limit workers and add farmer
    public bool addFarmer()
    {
        if (farmers < limitWorkers)
        {
            farmers++;
            full = (farmers >= limitWorkers);
            return true;
        }
        return false;
    }

    // Farmed complete
    public void farmed()
    {
        food = Random.Range(100, (farmers * 100) + 1);
        toggleSpriteHarvest();
    }

    public void toggleSpriteHarvest()
    {
        sr.sprite = spriteHarvest;
    }

    public void toggleSpriteEmpty()
    {
        sr.sprite = normalSprite;
    }

}
