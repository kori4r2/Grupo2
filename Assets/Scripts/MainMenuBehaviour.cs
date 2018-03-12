using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject newGameMenu;
    public GameObject loadGameButton;
    /*
	public Button btnNewGame;
	public Button btnWarrior;
	public Button btnMage;
	public Button btnArcher;
    */

	// Use this for initialization
	void Start () {
		mainMenu.SetActive (true);
		newGameMenu.SetActive (false);
        loadGameButton.SetActive(GameManager.hasSaveFile());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickNewGame(){
		mainMenu.SetActive (false);
		newGameMenu.SetActive (true);
	}

    public void OnClickLoadGame() {
        GameManager.LoadGame();
    }


	// Instancia o personagem selecionado e chama a cena de batalha

	public void OnClickWarrior(){
		GameManager.InstantiateWarrior (0f, 0f, 0f);
		SceneManager.LoadScene("ExplorationScene");
	}

	public void OnClickMage(){
		GameManager.InstantiateMage (1f, 0f, 0f);
		SceneManager.LoadScene("ExplorationScene");
	}

	public void OnClickArcher(){
		GameManager.InstantiateArcher (-1f, 0f, 0f);
		SceneManager.LoadScene("ExplorationScene");
	}
}
