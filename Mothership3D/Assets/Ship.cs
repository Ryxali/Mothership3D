using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour {
	public static Ship instance { get; private set; }
	public bool inGarage = true;
	private new Rigidbody rbody;
	public float sailSpeed = 1.0f;
	public float turnSpeed = 1.0f;
	void Awake() {
		instance = this;
		rbody = GetComponent<Rigidbody> ();
	}

	public void setInTheGarage(bool val) {
		inGarage = val;	 
	}

	void Update() {
		if (!inGarage) {
			float vert = Input.GetAxis("Vertical");
			float hor = Input.GetAxis("Horizontal");
			Vector3 vel = transform.position - transform.right * vert * sailSpeed;
			rbody.MovePosition(vel);
			rbody.MoveRotation(Quaternion.Euler(0, hor * turnSpeed + rbody.rotation.eulerAngles.y, 0));
		}
	}
}
