using UnityEngine;
using System.Collections;
using UnityEditor;


/*
 * 
 */
/// <summary>
/// Class detailing the reach in angles of a turret or turret slot.
/// Requires onEditorGUI() call to be editable (make custom editor for objects)
///	
/// </summary>
/// Author: Niklas L
[System.Serializable]
public class Reach {
	public Reach() {

	}
	private Reach(Reach a, Reach b) {
		lradius = (a.lradius < b.lradius) ? a.lradius : b.lradius;
		rradius = (a.rradius < b.rradius) ? a.rradius : b.rradius;
		dradius = (a.dradius < b.dradius) ? a.dradius : b.dradius;
		uradius = (a.uradius < b.uradius) ? a.uradius : b.uradius;
	}

	/// <summary>
	/// Creates a Reach, constrained by the smallest radiuses
	/// between the two provided reaches.
	/// </summary>
	/// <returns>The constrained reach.</returns>
	/// <param name="a">The first Reach.</param>
	/// <param name="b">The second Reach.</param>
	public static Reach CreateConstrainedReach(Reach a, Reach b) {
		if (a != null && b != null) {
			return new Reach (a, b);
		} else {
			Debug.LogError ("One or more parameters are null!");
		}
		return null;
	}

	[SerializeField]
	private float lr;
	/// <summary>
	/// Gets the lradius in degrees.
	/// </summary>
	/// <value>The lradius in degrees.</value>
	public float lradius { 
		get { return lr; }
		private set {
			lr = value;
			lr_Rad = lr * Mathf.Deg2Rad;
		} 
	}
	/// <summary>
	/// Get the leftwards radius in radians.
	/// </summary>
	/// <value>The lradius in radians.</value>
	public float lradius_Rad { get { return lr_Rad; } }
	private float lr_Rad;

	[SerializeField]
	private float rr;
	/// <summary>
	/// Get the rightwards radius in degrees.
	/// </summary>
	/// <value>The rradius in degrees.</value>
	public float rradius { 
		get { return rr; }
		private set {
			rr = value;
			rr_Rad = rr * Mathf.Deg2Rad;
		} 
	}
	/// <summary>
	/// Get the rightwards radius in radians.
	/// </summary>
	/// <value>The rradius in radians.</value>
	public float rradius_Rad { get { return rr_Rad; } }
	[SerializeField]
	private float rr_Rad;

	[SerializeField]
	private float dr;
	/// <summary>
	/// Get the downwards radius in degrees.
	/// </summary>
	/// <value>The dradius in degrees.</value>
	public float dradius { 
		get { return dr; }
		private set {
			dr = value;
			dr_Rad = dr * Mathf.Deg2Rad;
		} 
	}
	/// <summary>
	/// Get the downwards radius in radians.
	/// </summary>
	/// <value>The dradius in radians.</value>
	public float dradius_Rad { get { return dr_Rad; } }
	[SerializeField]
	private float dr_Rad;

	[SerializeField]
	private float ur;
	/// <summary>
	/// Get the upwards radius in degrees.
	/// </summary>
	/// <value>The uradius in degrees.</value>
	public float uradius { 
		get { return ur; }
		private set {
			ur = value;
			ur_Rad = ur * Mathf.Deg2Rad;
		}
	}
	/// Get the upwards radius in radians.
	/// </summary>
	/// <value>The uradius in radians.</value>
	public float uradius_Rad { get { return ur_Rad; } }
	[SerializeField]
	private float ur_Rad;

	/// <summary>
	/// Call this function from custom editor to automatically
	/// render the sliders in editor.
	/// </summary>
	public void onEditorGUI() {
		lradius = EditorGUILayout.Slider ("Left Radius", lradius, 0, 360 - rradius);
		rradius = EditorGUILayout.Slider ("Right Radius", rradius, 0, 360 - lradius);
		dradius = EditorGUILayout.Slider ("Down Radius", dradius, 0, 180);
		uradius = EditorGUILayout.Slider ("Up Radius", uradius, 0, 180);

	}

	public void DrawDebug(Transform origin) {
		Debug.DrawRay (origin.position, Quaternion.Euler(0, -lradius, 0) * origin.forward, Color.yellow);
		Debug.DrawRay (origin.position, Quaternion.Euler(0, rradius, 0) * origin.forward, Color.yellow);
		Debug.DrawRay (origin.position, Quaternion.Euler(dradius, 0, 0) * origin.forward, Color.yellow);
		Debug.DrawRay (origin.position, Quaternion.Euler(-uradius, 0, 0) * origin.forward, Color.yellow);
	}

	private float radius { get { return (dradius + uradius + rradius + lradius) / 4; } }

	public bool inReach(Transform self, Transform target) {
		Vector3 pointOnPlaneU = Vector3.ProjectOnPlane (target.position, self.up);
		Vector3 pointOnPlaneR = Vector3.ProjectOnPlane (target.position, self.right);
		return Vector3.Angle(self.forward, (target.position - self.position).normalized) < radius;
	}
}
