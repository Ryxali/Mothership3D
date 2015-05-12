using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class TurretSlot : MonoBehaviour {
	public TurretSize maxAllowedSize;
	public TurretMType typesAllowed;
	public GameObject indicator;

	private Turret turret;
	public Reach reach;

	void Update() {
		reach.DrawDebug(transform);

		/*
		Vector3 dr = (D_radius + R_radius).normalized;//Quaternion.Euler (dradius, rradius, 0) * transform.forward;
		Vector3 ur = (U_radius + R_radius).normalized;//Quaternion.Euler (-uradius, rradius, 0) * transform.forward;
		Vector3 dl = (D_radius + L_radius).normalized;//Quaternion.Euler (dradius, -lradius, 0) * transform.forward;
		Vector3 ul = (U_radius + L_radius).normalized;//Quaternion.Euler (-uradius, -lradius, 0) * transform.forward;
		Debug.DrawRay (transform.position, dr, Color.yellow);
		Debug.DrawRay (transform.position, ur, Color.yellow);
		Debug.DrawRay (transform.position, dl, Color.yellow);
		Debug.DrawRay (transform.position, ul, Color.yellow);

		Debug.DrawLine (transform.position + dr, transform.position + dl, Color.white);
		Debug.DrawLine (transform.position + dl, transform.position + ul, Color.white);
		Debug.DrawLine (transform.position + ul, transform.position + ur, Color.white);
		Debug.DrawLine (transform.position + ur, transform.position + dr, Color.white);*/
	}

	public void setTurret(Turret t) {
		removeTurret();
		turret = (Turret) Instantiate(t);
		turret.transform.position = transform.position;
		turret.transform.parent = transform;
		turret.transform.rotation = transform.rotation;
		turret.setSlot (this);
	}

	private void removeTurret() {
		if (turret == null)
			return;
		turret.delete ();
		turret = null;
	}

	public bool fits(Turret t)  {
		return fits (t.turretSize) && fits (t.turretMountType);
	}

	public bool fits(TurretSize tSize) {
		return (tSize & maxAllowedSize) == tSize;
	}

	public bool fits(TurretMType tMType) {
		return (tMType & typesAllowed) == tMType;
	}
}
