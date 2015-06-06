using UnityEngine;
using System.Collections;

public class Frigate : Ship {
	private float gravVelocity = 0.0f;
	// Update is called once per frame
	void Update () {
		if (!inGarage) {
			float vert = Input.GetAxis("Vertical");
			float hor = Input.GetAxis("Horizontal");
			gravVelocity += ((transform.position.y < 0) ? 3.0f : -3.0f) * Time.deltaTime;
			Vector3 vel = transform.forward * vert * sailSpeed + Vector3.up * gravVelocity;
			rbody.velocity = vel;
			rbody.MoveRotation(Quaternion.Euler(0, hor * turnSpeed + rbody.rotation.eulerAngles.y, 0));


		}
	}
}
