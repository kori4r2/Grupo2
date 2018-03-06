using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class GameManager {

	public enum STATE {BATTLE, EXPLORATION} // possiveis estados do jogo

	public static List<GameObject> players = new List<GameObject>();
	public static List<GameObject> enemies = new List<GameObject>();
    
	public static int potions = 0;
	public static bool isPotionSelected = false;

	public static GameObject prefabWarrior = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Warrior.prefab");
	public static GameObject prefabMage = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mage.prefab");
	public static GameObject prefabArcher = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Archer.prefab");
	public static GameObject prefabOgre = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ogre.prefab");

	public static STATE state = STATE.EXPLORATION;
    
    public static string saveFileLocation = "./SaveFiles/savefile.json";

	public static void OnClickPotion(){
		if (potions > 0) {
			if (!isPotionSelected)
				isPotionSelected = true;
			else
				isPotionSelected = false;
		}
	}

	// Instancia um warrior, com DontDestroyOnLoad
	public static void InstantiateWarrior(float x, float y, float z){
		GameObject warrior = GameObject.Instantiate (prefabWarrior, new Vector3 (x, y, z), Quaternion.identity);
		warrior.name = "Warrior";
		players.Add (warrior);
		GameObject.DontDestroyOnLoad (warrior);
	}

	// Instancia um mage, com DontDestroyOnLoad
	public static void InstantiateMage(float x, float y, float z){
		GameObject mage = GameObject.Instantiate (prefabMage, new Vector3 (x, y, z), Quaternion.identity);
		mage.name = "Mage";
		players.Add (mage);
        GameObject.DontDestroyOnLoad (mage);
	}

	// Instancia um archer, com DontDestroyOnLoad
	public static void InstantiateArcher(float x, float y, float z){
		GameObject archer = GameObject.Instantiate (prefabArcher, new Vector3 (x, y, z), Quaternion.identity);
		archer.name = "Archer";
		players.Add (archer);
        GameObject.DontDestroyOnLoad (archer);
	}

	public static void InstantiateOgre(float x, float y, float z, int attackType){
		GameObject ogre = GameObject.Instantiate (prefabOgre, new Vector3 (x, y, z), Quaternion.identity);
		ogre.name = "Ogre";
		ogre.GetComponent<EnemyBehaviour> ().AttackType = attackType;
		enemies.Add (ogre);
	}

	public static void RewardPotion(int n){
		potions += n;
		GameObject potionTxt = GameObject.Find ("TxtPotion");
		potionTxt.GetComponent<Text> ().text = "" + potions + "x";
	}

    public static void SaveGame() {
        // Save info to .json file
    }

    public static void LoadGame() {
        // Load info from .json file
    }
}