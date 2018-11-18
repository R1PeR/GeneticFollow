using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByMouse : MonoBehaviour {

	// Use this for initialization

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }		
    }
}
