using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AimingTurret : Turret {
	[SerializeField]
	private Projectile bulletType;
	[SerializeField]
	private Transform spawnTransform;
	[SerializeField]
	private Transform head;

	public void onEditorGUI() {
		bulletType = (Projectile) EditorGUILayout.ObjectField ("Projectile ", bulletType, typeof(Projectile), false);
		spawnTransform = (Transform) EditorGUILayout.ObjectField ("Spawn Transform ", spawnTransform, typeof(Transform), true);
		head = (Transform)EditorGUILayout.ObjectField ("Head", head, typeof(Transform), true);
	}

	protected override void onUpdate(List<GameObject> targets) {
		foreach (GameObject target in targets) {
			head.LookAt(target.transform.position);
			fire (target);
			return;
		}
	}
	
	protected void fire(GameObject target) {
		Projectile projectile = (Projectile) Instantiate (bulletType, spawnTransform.position, spawnTransform.rotation);
		
	}
}
