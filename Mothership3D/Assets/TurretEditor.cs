using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Turret))]
public class TurretEditor : Editor {

	public override void OnInspectorGUI() {
		Turret t = (Turret)target;
		EditorGUILayout.ObjectField ("Script", t, typeof(Turret));
		t.onEditorGUI ();
		EditorUtility.SetDirty (target);
	}
}

