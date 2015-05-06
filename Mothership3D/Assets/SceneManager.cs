using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SceneManager : MonoBehaviour {
	Stack<int> levels = new Stack<int> ();
	public void LoadLevel(string levelName) {
		levels.Push (Application.loadedLevel);
		Application.LoadLevel (levelName);
	}

	public void LoadPreviousScene() {
		Application.LoadLevel (levels.Pop());
	}
}
