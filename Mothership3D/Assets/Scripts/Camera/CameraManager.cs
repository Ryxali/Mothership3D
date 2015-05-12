using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public GameObject[] cameras;
	private GameObject current;
	private GameObject next {
		get {
			if(n <= cameras.Length) {
				n = 0;
			}
			return cameras[n++];
		}
	}
	private int n = 0;
	// Use this for initialization
	void Start () {
		current = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey(KeyCode.Tab) && cameras.Length > 0) {
			PossessNextCamera();
		}
	}

	public void PossessNextCamera() {
		if(current != null)
			current.SetActive(false);
		current = next;
		current.SetActive(true);
		Camera.SetupCurrent (current.GetComponentInChildren<Camera>());
	}
}
