using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
[ExecuteInEditMode]
public class Turret : MonoBehaviour {


	/// <summary>
	/// Gets the size of the turret.
	/// </summary>
	/// <value>The size of the turret.</value>
	public TurretSize turretSize { get { return tSize; } }
	[SerializeField]
	private TurretSize tSize = TurretSize.MEDIUM;

	/// <summary>
	/// Gets the type of the turret mount.
	/// </summary>
	/// <value>The type of the turret mount.</value>
	public TurretMType turretMountType { get { return tMType; } }
	[SerializeField]
	private TurretMType tMType = TurretMType.INTERNAL;

	[SerializeField]
	private Reach maxReach = new Reach();
	/// <summary>
	/// Get the reach of this turret. The value may be lower as it's 
	/// constrained by the TurretSlot it resides in. Defaults to the
	/// maximum reach if not attached.
	/// </summary>
	/// <value>The reach.</value>
	public Reach reach { 
		get { return (_reach != null) ? _reach : maxReach; } 
		private set { _reach = value; } 
	}
	private Reach _reach;
	
	public TurretSlot turretSlot { get; private set; }

	[SerializeField]
	protected float fireRate = 1.0f;
	private float lastFire = 0.0f;

	void Awake () {
	}

	void Update () {
		if (Application.isEditor && !Application.isPlaying) {
			maxReach.DrawDebug (transform);
		} else if (!Ship.instance.inGarage) {
			if(lastFire < Time.time) {
				lastFire = Time.time + fireRate;
				List<GameObject> currentTargets = new List<GameObject> ();
				foreach (GameObject obj in EnemyManager.enemies) {
					if(Vector3.Distance(obj.transform.position, transform.position) < 100.0f &&
					   reach.inReach(transform, obj.transform)) {
						currentTargets.Add(obj);
					}
				}
				onUpdate (currentTargets);
			}
		}

	}

	protected virtual void onUpdate (List<GameObject> targets) {

	}

	public void delete() {
		Destroy (gameObject);
	}

	public void setSlot(TurretSlot slot) {
		turretSlot = slot;
		reach = Reach.CreateConstrainedReach (slot.reach, maxReach);
	}

	public void onEditorGUI() {
		EditorGUILayout.LabelField ("Turret");
		tSize = (TurretSize) EditorGUILayout.EnumPopup ("Turret Size", tSize);
		tMType = (TurretMType) EditorGUILayout.EnumPopup ("Turret Type", tMType);
		fireRate = EditorGUILayout.FloatField ("Fire Rate", fireRate);
		if(turretMountType == TurretMType.ANY) {
			Debug.LogWarning("A turret's type shouldn't be 'ANY'!");
		}
		maxReach.onEditorGUI ();
	}
}
