using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum STATE {BATTLE, EXPLORATION} // possiveis estados do jogo

	public List<GameObject> players;
	public List<GameObject> enemies;

	public static GameObject warriorPrefab;
	public static GameObject archerPrefab;
	public static GameObject magePrefab;
	public static GameObject ogrePrefab;

	public static int potions;
	public static bool isPotionSelected = false;

	public GameObject prefabWarrior;
	public GameObject prefabMage;
	public GameObject prefabArcher;

	public static STATE state;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		potions = 2;	//teste
		state = STATE.BATTLE;
	}

	public void OnClickPotion(){
		if (potions > 0) {
			if (!isPotionSelected)
				isPotionSelected = true;
			else
				isPotionSelected = false;
		}
	}

	// Instancia um warrior, com DontDestroyOnLoad
	public void InstantiateWarrior(){
		float x = -2.23f, y = -2.64f, z = 0f;
		GameObject warrior = Instantiate (prefabWarrior, new Vector3 (x, y, z), Quaternion.identity);
		warrior.name = "Warrior";
		players.Add (warrior);
		DontDestroyOnLoad (warrior);
	}

	// Instancia um mage, com DontDestroyOnLoad
	public void InstantiateMage(){
		float x = -2.23f, y = -2.64f, z = 0f;
		GameObject mage = Instantiate (prefabMage, new Vector3 (x, y, z), Quaternion.identity);
		mage.name = "Mage";
		players.Add (mage);
		DontDestroyOnLoad (mage);
	}

	// Instancia um archer, com DontDestroyOnLoad
	public void InstantiateArcher(){
		float x = -2.23f, y = -2.64f, z = 0f;
		GameObject archer = Instantiate (prefabArcher, new Vector3 (x, y, z), Quaternion.identity);
		archer.name = "Archer";
		players.Add (archer);
		DontDestroyOnLoad (archer);
	}

}
