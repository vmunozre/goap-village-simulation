using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerEntity : MonoBehaviour {
    public float moveSpeed = 0;
    public int food = 150;
    public bool isAdult = false;
    public bool isDead = false;
    public Vector3 randWander;
    public Sprite[] sprites;

    //Private var's
    private bool resting = false;
    private SpriteRenderer sr;

    //Timer
    private float startTime = 0;
    private float restingTime = 3;
    private float bornDuration = 5f;
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        randWander = getRandomWander(-0.5f, 0.5f);
        restingTime = getRandomTime(1.5f, 5f);
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead)
        {
            return;
        }
        if (!isAdult)
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > bornDuration)
            {
                isAdult = true;
                transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 0.5f;
                startTime = 0;
            }
            else
            {
                float scale = Mathf.Min(1f, transform.localScale.x + 0.003f);
                transform.localScale = new Vector3(scale, scale, 1f);
            }
        } else
        {
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
                    sr.sprite = sprites[1];
                    startTime = Time.time;
                }
                if (Time.time - startTime > restingTime)
                {
                    sr.sprite = sprites[0];
                    randWander = getRandomWander(-0.5f, 0.65f);                    
                    resting = false;
                    if(Random.Range(0,10) < 3)
                    {
                        food = Mathf.Min(300, food + 5);
                    }
                }
            }
        }
    }

    public void killDeer()
    {
        isDead = true;
        sr.sprite = sprites[2];
        moveSpeed = 0;
    }

    public void turnEmpty()
    {
        food = 0;
        sr.sprite = sprites[3];

        HerdEntity herd = GetComponentInParent<HerdEntity>();
        herd.hunted();
    }
    private float getRandomTime(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }
    private Vector3 getRandomWander(float _min, float _max)
    {
        float posX = transform.position.x - Random.Range(_min, _max);
        if (transform.parent.position.x > transform.position.x)
        {
            posX = transform.position.x + Random.Range(_min, _max);
        }

        float posY = transform.position.y - Random.Range(_min, _max);
        if (transform.parent.position.y > transform.position.y)
        {
            posY = transform.position.y + Random.Range(_min, _max);
        }

        return new Vector3(posX, posY, transform.position.z); ;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDead && collision.tag == "Hunter")
        {
            Debug.Log("HUMANO!!!!");
            // Debug.DrawLine(collision.transform.position, transform.position, Color.magenta, 4, false);
            randWander = getRunAwayPosition(collision);
            resting = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDead && collision.tag == "Hunter")
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
