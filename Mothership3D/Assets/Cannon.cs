using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class Cannon : Turret {
	[SerializeField]
	private Projectile bulletType;
	[SerializeField]
	private Transform spawnTransform;

	protected override void onUpdate(List<GameObject> targets) {
		foreach (GameObject target in targets) {
			fire (target);
		}
	}

	protected void fire(GameObject target) {
		Projectile projectile = (Projectile) Instantiate (bulletType, spawnTransform.position, spawnTransform.rotation);
		
	}


	public void onEditorGUI() {
		bulletType = (Projectile) EditorGUILayout.ObjectField ("Projectile ", bulletType, typeof(Projectile), false);
		spawnTransform = (Transform) EditorGUILayout.ObjectField ("Spawn Transform ", spawnTransform, typeof(Transform), true);
	}
}
