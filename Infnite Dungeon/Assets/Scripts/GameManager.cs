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

	public static int potions;
	public static bool isPotionSelected = false;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		potions = 2;	//teste
	}

	public void OnClickPotion(){
		if (potions > 0) {
			if (!isPotionSelected)
				isPotionSelected = true;
			else
				isPotionSelected = false;
		}
	}

}
