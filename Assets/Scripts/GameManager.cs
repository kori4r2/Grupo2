/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que comanda as ações principais do jogo, como quando surgem os inimigos, os personagens, distribui a experiencia, etc.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private Vector2 pointToGo; // Ponto ate onde o character vai se mover
	private Vector2 initialPosition; // Posicao inicial do clique do mouse fora do player
	private Vector2 offset;
	public GameObject character; // Referencia ao character, o objeto que sera movido
	public GameObject enemy;
	public GameObject battleUI;

	private Component playerBehaviour;
	private Component enemyBehaviour;

	// Use this for initialization
	void Start () {
		battleUI.SetActive(false);
		offset = new Vector2 (1f, 1f);
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			// Verify if a character was selected BEFORE, and not exactly now. 
			// We need to do it to obtain a good synchronization with the OnMouseDown() method from PlayerBehaviour. 
			if ((character.GetComponent<PlayerBehaviour> ().State == PlayerBehaviour.STATE.SELECTED) &&
				(!Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
					Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0f))) {
				pointToGo = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
				character.GetComponent<PlayerBehaviour> ().FinalPosition = pointToGo;
				character.GetComponent<PlayerBehaviour> ().State = PlayerBehaviour.STATE.MOVING;
				print ("Entrou");
				battleUI.SetActive (false);
			} else {
				Vector2 initialPosition = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
			}

			if (enemy.GetComponent<EnemyBehaviour> ().State == EnemyBehaviour.STATE.SELECTED) {		// Se um inimigo foi selecionado
				if (character.GetComponent<PlayerBehaviour> ().State == PlayerBehaviour.STATE.ATTACKING)  // Se um character está atacando
					character.GetComponent<PlayerBehaviour> ().Attack();
				else 
					enemy.GetComponent<EnemyBehaviour> ().State = EnemyBehaviour.STATE.NOTSELECTED;
			}
		}

	}

	void OnMouseDrag() {
		Vector2 cursorPoint = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 cursorPosition = (Vector2)Camera.main.ScreenToWorldPoint (cursorPoint.x) + offset;
		Vector2 heading = cursorPosition - initialPosition;
		Vector2 direction = heading / heading.magnitude;
		print (heading);

	}

}
