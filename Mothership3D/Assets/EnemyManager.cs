using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	public static List<GameObject> enemies { get { return instance._enemies; } } 
	private static EnemyManager instance;

	private List<GameObject> _enemies;

	// Use this for initialization
	void Awake () {
		instance = this;
		_enemies = new List<GameObject> ();
		for(int i = 0; i < transform.childCount; ++i) {
			_enemies.Add(transform.GetChild(i).gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
