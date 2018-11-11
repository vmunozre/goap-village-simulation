using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour {

    public bool activeMovement = false;

    private int boundary = 50;
    private int speed = 5;
    private int screenWidth = 0;
    private int screenHeight = 0;

    public GameObject target = null;

    public
	void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
	
	void Update () {

        // Unselect target
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            target = null;
        }
        // Turn on/off activeMovement
        if (Input.GetKeyDown(KeyCode.M))
        {
            activeMovement = !activeMovement;
        }

        if (target != null)
        {
            // Follow target
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        } else
        {
            moveCam();
        }
    }

    void moveCam()
    {
        if (activeMovement)
        {
            Vector3 camPos = transform.position;
            float movement = 0f;
            if (Input.mousePosition.x > screenWidth - boundary)
            {
                movement = camPos.x + speed * Time.deltaTime;
                camPos.x = Mathf.Min(movement, 15f);
            }
            else if (Input.mousePosition.x < boundary)
            {
                movement = camPos.x - speed * Time.deltaTime;
                camPos.x = Mathf.Max(movement, -15f);
            }

            else if (Input.mousePosition.y > screenHeight - boundary)
            {
                movement = camPos.y + speed * Time.deltaTime;
                camPos.y = Mathf.Min(movement, 10f);
            }
            else if (Input.mousePosition.y < boundary)
            {
                movement = camPos.y - speed * Time.deltaTime;
                camPos.y = Mathf.Max(movement, -10f);
            }

            transform.position = camPos;
        }
    }
}
