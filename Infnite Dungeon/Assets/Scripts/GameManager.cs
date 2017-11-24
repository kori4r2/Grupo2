/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que comanda as ações principais do jogo, como quando surgem os inimigos, os personagens, distribui a experiencia, etc.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum TURN {CHECK, PLAYERTURN, WAITTURN, ENEMYTURN} // possiveis estados do jogo

	private Vector2 pointToGo; // Ponto ate onde o character vai se mover
	private Vector2 initialPosition; // Posicao inicial do clique do mouse fora do player
	private Vector2 offset;

	public GameObject battleUI;

	private TURN turn; // determina o estado do turno

	public List<GameObject> players;
	public List<GameObject> enemies;

	private float timerFrozen = 0.0f;
	private bool allEnemiesFrozen = false;


	public TURN Turn {
		get {
			return turn;
		}
		set { 
			turn = value; 
		}
	}

	// Use this for initialization
	void Start () {
		EnemyBehaviour enemyBehaviour;
		int i;
		turn = TURN.ENEMYTURN;
		for (i = 0; i < enemies.Count; i++) {
			enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			enemyBehaviour.State = EnemyBehaviour.STATE.ATTACKING;
			enemyBehaviour.Attack ();
		}

		battleUI.SetActive(false);
		//offset = new Vector2 (1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		int i, j, counter = 0;
		PlayerBehaviour playerBehaviour;
		EnemyBehaviour enemyBehaviour;

		if (allEnemiesFrozen)
			timerFrozen += Time.deltaTime;
		
		if (turn == TURN.CHECK)
			Check ();
		else if (turn == TURN.PLAYERTURN || (allEnemiesFrozen && timerFrozen < enemies[0].GetComponent<EnemyBehaviour>().time)) {		// Sé é o turno do player e houve um toque na tela
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				// Verify if a character was selected BEFORE, and not exactly now. 
				// We need to do it to obtain a good synchronization with the OnMouseDown() method from PlayerBehaviour.

				for (i = 0; i < players.Count; i++) {
					playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
					// MOVIMENTAÇÃO DO PLAYER
					if ((playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) && // Se há um player selecionado e não tocou em um colisor
					    ((!Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
						    Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0f)))) {
						pointToGo = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
						playerBehaviour.FinalPosition = pointToGo;
						playerBehaviour.State = PlayerBehaviour.STATE.MOVING;
						print ("State moving");
						playerBehaviour.anim.SetInteger ("State", 1);
						battleUI.SetActive (false);

						break;
					} else {
						initialPosition = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
						if (playerBehaviour.State == PlayerBehaviour.STATE.WAITATTACK) {		// Se o player vai atacar
							for (j = 0; j < enemies.Count; j++) {
								enemyBehaviour = enemies [j].GetComponent<EnemyBehaviour> ();
								//ATAQUE DO PLAYER
								if (enemyBehaviour.IsSelected) {		// Se um inimigo foi selecionado
									playerBehaviour.Attack (enemyBehaviour);
								}
							}
						}
					}
				}
			}
			// Verifica quantos enemies estão em espera para atacar
			for (i = 0; i < enemies.Count; i++) {
				enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
				if (enemyBehaviour.State != EnemyBehaviour.STATE.WAITATTACK && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
					break;
				counter++;
			}
			if (counter == enemies.Count) { 		// Se todos os inimigos estão esperando, entra no turno de espera
				turn = TURN.WAITTURN;
				for (i = 0; i < players.Count; i++) {		// Verifica se há um player prestes a agir
					playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
					if (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED || playerBehaviour.State == PlayerBehaviour.STATE.WAITATTACK ||
						playerBehaviour.State == PlayerBehaviour.STATE.FROZEN) {
						playerBehaviour.State = PlayerBehaviour.STATE.NOTSELECTED;
						battleUI.SetActive (false);
					}
				}
				for (i = 0; i < enemies.Count; i++)
					enemies [i].GetComponent<EnemyBehaviour> ().IsSelected = false;
			}
		} else if (turn == TURN.WAITTURN) { 		// Se estão no turno de espera
			print ("Turno de espera");
			for (i = 0; i < players.Count; i++) {
				playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
				if (playerBehaviour.State != PlayerBehaviour.STATE.NOTSELECTED && playerBehaviour.State != PlayerBehaviour.STATE.SPECIAL)
					break;
				counter++;
			}
			if (counter == players.Count) {		// Se todos os players pararam de agir, vai para o turno do inimigo
				turn = TURN.ENEMYTURN;
				for (i = 0; i < enemies.Count; i++) {
					enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
					enemyBehaviour.State = EnemyBehaviour.STATE.ATTACKING;
					enemyBehaviour.anim.SetInteger ("State", 2);
					print ("Setou estado 2");
					//enemyBehaviour.Attack ();
				}
				for (i = 0; i < players.Count; i++) {
					playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
					if (playerBehaviour is WarriorBehaviour && playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL)
						playerBehaviour.anim.SetInteger ("State", 2);
				}
			}
		} else if (turn == TURN.ENEMYTURN) {
			// Checa se todos os inimigos ainda estão atacando
			for (i = 0; i < enemies.Count; i++) {
				enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
				if (enemyBehaviour.State != EnemyBehaviour.STATE.NOTSELECTED && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
					break;
				counter++;
			}
			if (counter == enemies.Count) {
				CheckCollisions ();
			}
		} else if (turn == TURN.CHECK)
			Check ();
	}

	// Estado de checagem: pode terminar o jogo, chamar a nova sala, ou iniciar o PLAYERTURN
	void Check(){
		timerFrozen = 0f;
		int counterEnemiesFrozen = 0;
		EnemyBehaviour enemyBehaviour;
		if (players.Count > 0) {
			if (enemies.Count > 0) {
				int i;
				turn = TURN.PLAYERTURN;
				for (i = 0; i < enemies.Count; i++) {
					enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
					// O inimigo inicia o turno se ele não estiver congelado ou se estiver congelado mas já tenha ficado 1 turno assim
					if ((enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN && enemyBehaviour.TurnsFrozen == 1) ||
					    enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN) {
						enemyBehaviour.Move ();
						enemyBehaviour.TurnsFrozen = 0;
					} else {
						counterEnemiesFrozen++;
						enemyBehaviour.TurnsFrozen = 1;
						print ("Setou 1");
					}
				}
				if (counterEnemiesFrozen == enemies.Count) {
					allEnemiesFrozen = true;
					print ("All enemies frozen");
				}
				else
					allEnemiesFrozen = false;
				for (i = 0; i < players.Count; i++) {
					players [i].GetComponent<PlayerBehaviour> ().State = PlayerBehaviour.STATE.NOTSELECTED;
				}
			}
			else
				NextRoom ();
		} else
			EndGame ();
	}

	void EndGame(){

	}

	void NextRoom(){

	}

	/*
	 * 
				print ("Colide");
				SetLifePlayer (playerBehaviour.Collider, playerBehaviour);
				playerBehaviour.IsColliding = false;
		*/

	// Checa se, nesse turno, algum player foi atacado
	public void CheckCollisions(){
		List<PlayerBehaviour> playersColliding = new List<PlayerBehaviour>();
		int i;
		// Verifica quais os players que estão colidindo
		for (i = 0; i < players.Count; i++) {
			PlayerBehaviour playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.IsColliding) 
				playersColliding.Add (playerBehaviour);			
		}

		if (playersColliding.Count > 0) {
			// Ordena pelo maior y
			playersColliding.Sort (delegate(PlayerBehaviour x, PlayerBehaviour y) {
				return(x.transform.position.y).CompareTo (y.transform.position.y);
			});

			// Verifica se há um warrior no meio do caminho usando special
			for(i = 0; i < playersColliding.Count; i++){
				if (playersColliding [i] is WarriorBehaviour && playersColliding [i].State == PlayerBehaviour.STATE.SPECIAL) {
					FreezeOgre (playersColliding [i].Collider);
					break;
				}
				// Se não é um warrior usando special, recebe dano
				print ("Colide");
				SetLifePlayer (playersColliding[i].Collider, playersColliding[i]);
				playersColliding[i].IsColliding = false;
			}

		}			

		// Destrói todos os objetos de ataque
		for (i = 0; i < enemies.Count; i++) {
			EnemyBehaviour enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			if(enemyBehaviour.AttackObject != null)
				Destroy (enemyBehaviour.AttackObject);
			print ("Destruiu");
		}
		turn = TURN.CHECK;	// Vai para o estado de checagem
	}

	// Diminui a vida do player baseada nos atributos de ataque do inimigo que gerou o ataque com Collider coll
	void SetLifePlayer(Collider2D coll, PlayerBehaviour player){
		EnemyBehaviour enemyBehaviour;
		for (int i = 0; i < enemies.Count; i++) {
			enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			if (enemyBehaviour.AttackObject == coll.gameObject) { 
				player.Life -= enemyBehaviour.AttackValue - player.Defense;
				print ("player life = " + player.Life);
			}
		}
	}

	void FreezeOgre(Collider2D coll){
		EnemyBehaviour enemyBehaviour;
		for (int i = 0; i < enemies.Count; i++) {
			enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			if (enemyBehaviour.AttackObject == coll.gameObject) {
				enemyBehaviour.State = EnemyBehaviour.STATE.FROZEN;
				print ("Congelou");
			}
		}
	}

	public void EnemySelected(EnemyBehaviour enemyBehaviour){
		for (int i = 0; i < players.Count; i++) {
			if (players [i].GetComponent<PlayerBehaviour> ().State == PlayerBehaviour.STATE.WAITATTACK) {
				enemyBehaviour.IsSelected = true;
				return;
			}
		}
	}

	/*
	void OnMouseDrag() {
		Vector2 cursorPoint = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 cursorPosition = (Vector2)Camera.main.ScreenToWorldPoint (cursorPoint.x) + offset;
		Vector2 heading = cursorPosition - initialPosition;
		Vector2 direction = heading / heading.magnitude;
		print (heading);

	}
	*/

}
