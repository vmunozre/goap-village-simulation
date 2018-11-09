using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour {

    public float actualSpeed = 1;

	void Start () {
		
	}
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            actualSpeed = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            actualSpeed = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            actualSpeed = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            actualSpeed = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            actualSpeed = 0;
        }
    }
}
