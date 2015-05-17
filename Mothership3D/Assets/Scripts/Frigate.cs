using UnityEngine;
using System.Collections;

public class Frigate : Ship {
	
	// Update is called once per frame
	void Update () {
		if (!inGarage) {
			float vert = Input.GetAxis("Vertical");
			float hor = Input.GetAxis("Horizontal");

			Vector3 vel = transform.position - transform.right * vert * sailSpeed;
			rbody.MovePosition(vel);
			rbody.MoveRotation(Quaternion.Euler(0, hor * turnSpeed + rbody.rotation.eulerAngles.y, 0));
		}
	}
}
