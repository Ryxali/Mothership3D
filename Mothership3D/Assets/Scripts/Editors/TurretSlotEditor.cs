using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(TurretSlot))]
public class TurretSlotEditor : Editor {

	public override void OnInspectorGUI() {
		TurretSlot t = (TurretSlot)target;

		t.maxAllowedSize = (TurretSize) EditorGUILayout.EnumPopup ("Max Allowed Size", t.maxAllowedSize);
		t.typesAllowed = (TurretMType) EditorGUILayout.EnumPopup ("Types Allowed", t.typesAllowed);
		t.indicator = (GameObject) EditorGUILayout.ObjectField ("Indicator", t.indicator, typeof(GameObject), true);
		if (t.reach == null)
			t.reach = new Reach ();
		t.reach.onEditorGUI ();

		EditorUtility.SetDirty (target);
	}
}
