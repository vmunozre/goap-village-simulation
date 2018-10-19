using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerEntity : MonoBehaviour {
    public float moveSpeed = 0.5f;
    public Vector3 randWander;
    private bool resting = false;

    //Timer
    private float startTime = 0;
    private float restingTime = 3;
    void Start () {
        randWander = getRandomWander(-0.5f, 0.5f);
        restingTime = getRandomTime(1.5f, 5f);
    }
	
	// Update is called once per frame
	void Update () {
        if (!resting)
        {
            float step = moveSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, randWander, step);

            if (gameObject.transform.position.Equals(randWander))
            {
                startTime = 0;
                restingTime = getRandomTime(1.5f, 5f);
                resting = true;
            }
        }
        else
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }
            if (Time.time - startTime > restingTime)
            {
                randWander = getRandomWander(-0.5f,0.5f);
                resting = false;
            }
        }
    }

    private float getRandomTime(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }
    private Vector3 getRandomWander(float _min, float _max)
    {
        return new Vector3(transform.position.x + Random.Range(_min, _max), transform.position.y + Random.Range(_min, _max), transform.position.z); ;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collector")
        {
            Debug.Log("HUMANO!!!!");
            // Debug.DrawLine(collision.transform.position, transform.position, Color.magenta, 4, false);
            randWander = getRunAwayPosition(collision);
            resting = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Collector")
        {
            Debug.DrawLine(collision.transform.position, transform.position, Color.magenta);
            randWander = getRunAwayPosition(collision);
        }
    }

    private Vector3 getRunAwayPosition(Collider2D collision)
    {
        float posX = (transform.position.x - collision.transform.position.x);
        float posY = (transform.position.y - collision.transform.position.y);
        return new Vector3(transform.position.x + posX, transform.position.y + posY, transform.position.z); 
    }
}
