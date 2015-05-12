using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(AimingTurret))]
public class AimingTurretEditor : TurretEditor {


	public override void OnInspectorGUI() {
		AimingTurret t = (AimingTurret)target;
		t.onEditorGUI ();
		base.OnInspectorGUI ();
	}
}
