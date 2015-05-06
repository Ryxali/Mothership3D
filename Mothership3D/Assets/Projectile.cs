using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
	private new Rigidbody rigidbody;
	public float initialVelocity;
	// Use this for initialization
	void Awake () {
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.AddForce (transform.forward * initialVelocity, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
