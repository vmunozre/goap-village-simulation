using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrchardBuilding : BaseBuilding {
    public bool full = false;
    public int limitWorkers = 3;
    public int farmers = 0;
    public int food = 0;
    public float effort = 10f;
    public float farmProgress = 0f;

    public Sprite spriteHarvest;
    private SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        limitWorkers = Random.Range(2,4);
        sr = GetComponent<SpriteRenderer>();
    }

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

    public void farmed()
    {
        food = Random.Range(150, 300);
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
