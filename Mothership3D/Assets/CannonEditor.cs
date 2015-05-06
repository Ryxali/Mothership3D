﻿using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Cannon))]
public class CannonEditor : TurretEditor {

	public override void OnInspectorGUI() {
		Cannon t = (Cannon)target;
		base.OnInspectorGUI ();
		t.onEditorGUI ();
	}
}
