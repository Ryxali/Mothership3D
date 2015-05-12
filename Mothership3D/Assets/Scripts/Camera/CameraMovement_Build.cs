using UnityEngine;
using System.Collections;

public class CameraMovement_Build : MonoBehaviour {
	// Use this for initialization
	void Awake () {
	}
	[Range(0.0f, 100.0f)]
	public float panningSpeed = 1.0f;
	[Range(0.0f, 100.0f)]
	public float rotationSpeed = 1.0f;

	private Vector3 lastMousePosition;
	private bool panning = false;
	// Update is called once per frame
	void Update () {
		float vert = Input.GetAxis ("Vertical");
		float hor = Input.GetAxis ("Horizontal");
		float alt = Input.GetAxis("Altitude");
		Vector3 nPos = transform.position;
		nPos += transform.forward * (vert * panningSpeed);
		nPos += transform.right * (hor * panningSpeed);
		nPos += transform.up * (alt * panningSpeed);
		transform.position = nPos;


		if (panning) {
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

		if (Input.GetMouseButtonDown (1)) {
			startPanning ();
		} 
		if (Input.GetMouseButtonUp(1)) {
			stopPanning ();
		}
	}

	private void startPanning() {
		panning = true;
		lastMousePosition = Input.mousePosition;
	}

	private void stopPanning() {
		panning = false;

	}
}
