using UnityEngine;
using System.Collections;

public class ToolManager : MonoBehaviour {

	public Turret current;
	public Camera editCam;
	// Use this for initialization
	void Awake () {
		if (editCam == null) {
			Debug.LogError("editCam is null!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (editCam == null)
			return;
		if (Input.GetMouseButtonUp (0)) {
			RaycastHit hit;
			if (Physics.Raycast (editCam.ScreenPointToRay (Input.mousePosition), out hit)) {
				TurretSlot slot;
				if((slot = hit.transform.GetComponent<TurretSlot>()) != null) {
					slot.setTurret(current);
				}
			}
		}

		
	}

	public void setCurrent(Turret turret) {
		current = turret;
	}
}
