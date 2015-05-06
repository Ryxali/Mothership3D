using UnityEngine;
using System.Collections;

public class CameraRotationComponent : MonoBehaviour {
	private Vector3 lastMousePosition;
	public float rotationSpeed = 1.0f;
	// Use this for initialization
	void Awake () {
		lastMousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 diff = Input.mousePosition - lastMousePosition;
		//transform.Rotate(Vector3.up, diff.x * rotationSpeed);
		//transform.Rotate(Vector3.right, diff.y * rotationSpeed);
		Vector3 eulers = transform.rotation.eulerAngles;
		eulers.x -= diff.y * rotationSpeed;
		eulers.y += diff.x * rotationSpeed;
		transform.rotation = Quaternion.Euler(eulers);
		//eulers.z = 0;
		lastMousePosition = Input.mousePosition;
	}
}
