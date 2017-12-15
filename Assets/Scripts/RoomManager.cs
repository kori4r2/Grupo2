/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que gera as salas, seus detalhes e a porta.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {

	public Text heroText; // nome do heroi
	private int dieHero; // variavel randomica que determina de que classe sera o heroi encontrado
	private int dieRoom; // variavel randomica que determina se a proxima sala sera de batalha, pocao ou heroi
	private Vector2 initialPosition; // posicao inicial do pegar e arrastar
	private Vector2 finalPosition; // posicao final do clicar e arrastar
	private float distanceX; // distancia entre dois pontos no eixo X
	private float distanceY; // distancia entre dois pontos no eixo Y
	private float totalDistanceX; // distancia percorrida total em x
	private float totalDistanceY; // distancia percorrida total em Y

	public List<Sprite> rooms;
	public List<GameObject> heroes;
	public GameManager gameManager;

	public Text potionTxt;

	// Use this for initialization
	void Start () {
		initialPosition = new Vector2 (0f, 0f);
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = rooms[0];
		dieRoom = (int) Random.Range (0f, 2.9f);
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		GameManager.RewardPotion (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDrag() {
		if (initialPosition == new Vector2(0f, 0f)) {
			initialPosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		}

		finalPosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
	}

	void OnMouseUp() {
		int i, counter = 0;
		distanceX = finalPosition.x - initialPosition.x;
		distanceY = finalPosition.y - initialPosition.y;
		totalDistanceX = Mathf.Sqrt (Mathf.Pow (initialPosition.x - finalPosition.x, 2));
		totalDistanceY = Mathf.Sqrt (Mathf.Pow (initialPosition.y - finalPosition.y, 2));
		//print ("TOTAL DISTANCE EM X: " + totalDistanceX);
		//print ("TOTAL DISTANCE EM Y: " + totalDistanceY);

		// Se a distancia percorrida horizontalmente e' maior que a distancia percorrida na vertical
		if (totalDistanceX > totalDistanceY) {
			// Se a distancia em x for positiva o movimento e' para a direita
			if (distanceX > 0) {
				print ("DIREITA");
			} 
			// Se a distancia em x for negativa o movimento e' para a esquerda
			else {
				print ("ESQUERDA");
			}
		}

		// Se a distancia percorrida verticalmente e' maior que a distancia percorrida na horizontal
		else {
			// Se a distancia em y for positiva o movimento e' para cima
			if (distanceY > 0) {
				print ("CIMA");
			} 
			// Se a distancia em y for negativa o movimento e' para baixo
			else {
				print ("BAIXO");
			}
				
		}
			
		dieRoom = (int) Random.Range (0.8f, 1.9f);
		this.GetComponent<SpriteRenderer> ().sprite = rooms[dieRoom];

		if (dieRoom == 1) { // cena de batalha
			heroText.text = "";
			//gameObject.GetComponent<Collider> ().enabled = !gameObject.GetComponent<Collider> ().enabled;
			Application.LoadLevel ("BattleScene");
			GameManager.state = GameManager.STATE.BATTLE;
		} else if (dieRoom == 0) { // cena de encontrar aventureiro
			GameManager.state = GameManager.STATE.EXPLORATION;
			dieHero = Random.Range (0, 3);
			string heroName = heroes [dieHero].GetComponent<PlayerBehaviour> ().Name;

			if (heroName == "Arqueiro") {
				for (i = 0; i < gameManager.players.Count; i++) {
					if (gameManager.players [i].GetComponent<PlayerBehaviour> () is ArcherBehaviour)
						break;
					counter++;
				}
				if (counter == gameManager.players.Count) {
					gameManager.InstantiateArcher (-1f, 0f, 0f);
					heroText.text = "Parabéns! Você encontrou um novo aventureiro para a sua equipe da classe " + heroName;
				}
			} else if (heroName == "Mago") {
				for (i = 0; i < gameManager.players.Count; i++) {
					if (gameManager.players [i].GetComponent<PlayerBehaviour> () is MageBehaviour)
						break;
					counter++;
				}
				if (counter == gameManager.players.Count) {
					gameManager.InstantiateMage (1f, 0f, 0f);
					heroText.text = "Parabéns! Você encontrou um novo aventureiro para a sua equipe da classe " + heroName;
				}
			} else {
				for (i = 0; i < gameManager.players.Count; i++) {
					if (gameManager.players [i].GetComponent<PlayerBehaviour> () is WarriorBehaviour)
						break;
					counter++;
				}
				if (counter == gameManager.players.Count) {
					gameManager.InstantiateWarrior (0f, 0f, 0f);
					heroText.text = "Parabéns! Você encontrou um novo aventureiro para a sua equipe da classe " + heroName;
				}
			}
		} else { // cena de exploração
			heroText.text = "Você ganhou mais uma poção!";
			GameManager.RewardPotion (1);
		}

		initialPosition = new Vector2 (0f, 0f);
	}

	public void OnClickPotion(){
		gameManager.OnClickPotion ();
	}

}
