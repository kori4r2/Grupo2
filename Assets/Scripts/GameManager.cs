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
	public GameObject prefabOgre;

	public static STATE state;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		potions = 2;	//teste
		state = STATE.EXPLORATION;
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
	public void InstantiateWarrior(float x, float y, float z){
		//float x = x1, y = y1, z = z1;
		GameObject warrior = Instantiate (prefabWarrior, new Vector3 (x, y, z), Quaternion.identity);
		warrior.name = "Warrior";
		players.Add (warrior);
		DontDestroyOnLoad (warrior);
	}

	// Instancia um mage, com DontDestroyOnLoad
	public void InstantiateMage(float x, float y, float z){
		//float x = -2.23f, y = -2.64f, z = 0f;
		GameObject mage = Instantiate (prefabMage, new Vector3 (x, y, z), Quaternion.identity);
		mage.name = "Mage";
		players.Add (mage);
		DontDestroyOnLoad (mage);
	}

	// Instancia um archer, com DontDestroyOnLoad
	public void InstantiateArcher(float x, float y, float z){
		//float x = -2.23f, y = -2.64f, z = 0f;
		GameObject archer = Instantiate (prefabArcher, new Vector3 (x, y, z), Quaternion.identity);
		archer.name = "Archer";
		players.Add (archer);
		DontDestroyOnLoad (archer);
	}

	public void InstantiateOgre(float x, float y, float z, int attackType){
		GameObject ogre = Instantiate (prefabOgre, new Vector3 (x, y, z), Quaternion.identity);
		ogre.name = "Ogre";
		ogre.GetComponent<EnemyBehaviour> ().AttackType = attackType;
		enemies.Add (ogre);
		DontDestroyOnLoad (ogre);
	}

}