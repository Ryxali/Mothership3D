using UnityEngine;
using System.Collections;

public class Battleship : Ship {
	public float maxSpeed = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!inGarage) {
			float vert = Input.GetAxis("Vertical");
			float hor = Input.GetAxis("Horizontal");
			float pitch = Input.GetAxis("Pitch");
			float roll = Input.GetAxis("Roll");
			Vector3 vel = -transform.right * vert * sailSpeed;
			rbody.velocity = Vector3.ClampMagnitude( rbody.velocity + vel * Time.deltaTime, maxSpeed);


			Vector3 eulers = rbody.rotation.eulerAngles;

			//rbody.MoveRotation(Quaternion.Euler(eulers.x + roll * turnSpeed, eulers.y + hor * turnSpeed, eulers.z + pitch * turnSpeed));
			rbody.MoveRotation(rbody.rotation * Quaternion.Euler(roll * turnSpeed, hor * turnSpeed, pitch * turnSpeed));
		}
	}
}
