using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour {

	public float speed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		/// get raw input values
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 movement  = new Vector3(h, v, 0);
		
		/// prevents diagonals from being faster.
		if (movement.magnitude > 1)
		{ movement.Normalize(); }

		/// scale movement to duration of frame
		movement *= (speed * Time.deltaTime);

		/// update the position
		transform.position = transform.position + movement;

	}
}
