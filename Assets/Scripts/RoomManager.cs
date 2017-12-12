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
	public List<GameObject> heros;

	// Use this for initialization
	void Start () {
		initialPosition = new Vector2 (0f, 0f);
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = rooms[0];
		dieRoom = (int) Random.Range (0f, 2.9f);

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
			
		dieRoom = (int) Random.Range (0f, 2.9f);
		this.GetComponent<SpriteRenderer> ().sprite = rooms[dieRoom];

		if (dieRoom == 1) {
			//Application.LoadLevel ("BattleScene");
		} else if (dieRoom == 0) {
			dieHero = (int)Random.Range (0f, 2.9f);
			string heroName = heros[dieHero].GetComponent<PlayerBehaviour> ().Name;
			heroText.text = "Qualquer coisa " + heroName;
		}

		initialPosition = new Vector2 (0f, 0f);
	}

}
