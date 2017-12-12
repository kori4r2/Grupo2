using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject newGameMenu;
	public Button btnNewGame;
	public Button btnWarrior;
	public Button btnMage;
	public Button btnArcher;

	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		mainMenu.SetActive (true);
		newGameMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickNewGame(){
		mainMenu.SetActive (false);
		newGameMenu.SetActive (true);
	}


	// Instancia o personagem selecionado e chama a cena de batalha

	public void OnClickWarrior(){
		gameManager.InstantiateWarrior (0f, 0f, 0f);
		Application.LoadLevel ("ExplorationScene");
	}

	public void OnClickMage(){
		gameManager.InstantiateMage (1f, 0f, 0f);
		Application.LoadLevel ("ExplorationScene");
	}

	public void OnClickArcher(){
		gameManager.InstantiateArcher (-1f, 0f, 0f);
		Application.LoadLevel ("ExplorationScene");
	}
}
