using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static List<GameObject> players;
	public static List<GameObject> enemies;

	public static GameObject warriorPrefab;
	public static GameObject archerPrefab;
	public static GameObject magePrefab;
	public static GameObject ogrePrefab;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

}
