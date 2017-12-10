using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum STATE {BATTLE, EXPLORATION} // possiveis estados do jogo

	//TODO
	// Mudar pra lista de PlayerBehaviour e EnemyBehaviour. Quando for instanciar em uma cena, instancia primeiro o prefab sem 
	// o componente, e depois adiciona o componente
	public static List<GameObject> players;
	public static List<GameObject> enemies;

	public static GameObject warriorPrefab;
	public static GameObject archerPrefab;
	public static GameObject magePrefab;
	public static GameObject ogrePrefab;

	public static int potions;
	public static bool isPotionSelected = false;

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

}
