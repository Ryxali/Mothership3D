using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour {
	public static Ship instance { get; private set; }
	public bool inGarage = true;
	protected Rigidbody rbody { get; private set; }
	public float sailSpeed = 1.0f;
	public float turnSpeed = 1.0f;
	void Awake() {
		instance = this;
		rbody = GetComponent<Rigidbody> ();
	}

	public void setInTheGarage(bool val) {
		inGarage = val;	 
	}
}
