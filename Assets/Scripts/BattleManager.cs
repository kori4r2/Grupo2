/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que comanda as ações principais do jogo, como quando surgem os inimigos, os personagens, distribui a experiencia, etc.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour {

	//TODO
	//	Checar o campo de batalha, ou seja, não deixar os personagens andarem pra onde não devem
	//

	public enum TURN {CHECK, PLAYERTURN, WAITTURN, ENEMYTURN} // possiveis estados do jogo

	private Vector2 pointToGo; // Ponto ate onde o character vai se mover
	private Vector2 initialPosition; // Posicao inicial do clique do mouse fora do player
	private Vector2 offset;

	public GameObject battleUI;			// GameObject that contains all UI Battle components
	public GameObject specialButton;
	public Text potionTxt;
	public GameObject playerUI;

	private TURN turn; // determina o estado do turno

	public List<GameObject> players;
	public List<GameObject> enemies;

	private float timerShockwave = 0.0f;
	private float timerFrozen = 0.0f;
	private bool allEnemiesFrozen = false;

	public Slider warriorLifeSlider;
	public Slider warriorSpecialSlider;
	public Slider mageLifeSlider;
	public Slider mageSpecialSlider;
	public Slider archerLifeSlider;
	public Slider archerSpecialSlider;

	public GameObject warriorUI;
	public GameObject mageUI;
	public GameObject archerUI;

	public GameObject prefabWarriorUI;
	public GameObject prefabMageUI;
	public GameObject prefabArcherUI;

	private GameManager gameManager;

	public TURN Turn {
		get {
			return turn;
		}
		set { 
			turn = value; 
		}
	}

	public bool AllEnemiesFrozen {
		get {
			return allEnemiesFrozen;
		}
		set { 
			allEnemiesFrozen = value; 
		}
	}

	public GameObject BattleUI{
		get{
			return battleUI;
		}
	}

	// Use this for initialization
	void Start () {
		int i;
		float x = -2.23f, y = -2.64f, z = 0f;
		EnemyBehaviour enemyBehaviour;
		PlayerBehaviour playerBehaviour;

		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();


		// Para cada player no gameManager, instancia-o na cena de batalha, bem como seus elementos de UI.
		for (i = 0; i < gameManager.players.Count; i++) {
			players.Add(gameManager.players[i]);
			players [i].transform.position = new Vector3 (x, y, z);
			playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			playerBehaviour.battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
			if(playerBehaviour is WarriorBehaviour){
				warriorUI = Instantiate (prefabWarriorUI, transform.position, Quaternion.identity);
				warriorUI.transform.parent = playerUI.transform;
				warriorLifeSlider = GameObject.Find ("WarriorLifeSlider").GetComponent<Slider> ();
				warriorSpecialSlider = GameObject.Find ("WarriorSpecialSlider").GetComponent<Slider> ();
			}
			else if(playerBehaviour is MageBehaviour){
				mageUI = Instantiate (prefabMageUI, transform.position, Quaternion.identity);
				mageUI.transform.parent = playerUI.transform;
				mageLifeSlider = GameObject.Find ("MageLifeSlider").GetComponent<Slider> ();
				mageSpecialSlider = GameObject.Find ("MageSpecialSlider").GetComponent<Slider> ();
			}
			else if(playerBehaviour is ArcherBehaviour){
				archerUI = Instantiate (prefabArcherUI, transform.position, Quaternion.identity);
				archerUI.transform.parent = playerUI.transform;
				archerLifeSlider = GameObject.Find ("ArcherLifeSlider").GetComponent<Slider> ();
				archerSpecialSlider = GameObject.Find ("ArcherSpecialSlider").GetComponent<Slider> ();
			}
			x += 2.23f;
		}

		// Provavelmente vai virar lixo
		GameObject[] playersScene = GameObject.FindGameObjectsWithTag ("Player");
		GameObject[] enemiesScene = GameObject.FindGameObjectsWithTag ("Enemy");
		//potionTxt = GameObject.Find("TxtPotion").GetComponent<Text>();
		//potionTxt.text = GameManager.potions + "x";

		/*
		if(GameObject.Find("Warrior") != null){
			warriorLifeSlider = GameObject.Find ("WarriorLifeSlider").GetComponent<Slider> ();
			warriorSpecialSlider = GameObject.Find ("WarriorSpecialSlider").GetComponent<Slider> ();
			warriorUI = GameObject.Find ("WarriorUI");
		}

		if(GameObject.Find("Mage") != null){
			mageLifeSlider = GameObject.Find ("MageLifeSlider").GetComponent<Slider> ();
			mageSpecialSlider = GameObject.Find ("MageSpecialSlider").GetComponent<Slider> ();
			mageUI = GameObject.Find ("MageUI");
		}

		if(GameObject.Find("Archer") != null){
			archerLifeSlider = GameObject.Find ("ArcherLifeSlider").GetComponent<Slider> ();
			archerSpecialSlider = GameObject.Find ("ArcherSpecialSlider").GetComponent<Slider> ();
			archerUI = GameObject.Find ("ArcherUI");
		}


		for (i = 0; i < playersScene.Length; i++) {
			//playerBehaviour = playersScene [i].GetComponent<PlayerBehaviour> ();
			players.Add (playersScene[i]);
		}*/
		for (i = 0; i < enemiesScene.Length; i++) {
			enemyBehaviour = enemiesScene [i].GetComponent<EnemyBehaviour> ();
			enemies.Add (enemiesScene[i]);
		}
		
		turn = TURN.WAITTURN;

		

			
		/*
		for (i = 0; i < enemies.Count; i++) {
			enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			enemyBehaviour.State = EnemyBehaviour.STATE.ATTACKING;
			enemyBehaviour.Attack ();
		}*/

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
		else if (turn == TURN.PLAYERTURN) {		// Sé é o turno do player e houve um toque na tela
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
							Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0f))) &&
						(Input.mousePosition.y <= -0.26f && Input.mousePosition.y >= 3.75f )) {
						pointToGo = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
						playerBehaviour.FinalPosition = pointToGo;
						playerBehaviour.State = PlayerBehaviour.STATE.MOVING;
						//print ("State moving");
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
						} else if (playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL) {	// Se o player está em special
							if (playerBehaviour is ArcherBehaviour) {		// Se é o special do archer
								ArcherBehaviour archerBehaviour = (ArcherBehaviour) playerBehaviour;
								archerBehaviour.SpecialCommand (enemies);
							} else if (playerBehaviour is MageBehaviour) {	// Se é o special do mage
								MageBehaviour mageBehaviour = (MageBehaviour) playerBehaviour;
								for (j = 0; j < enemies.Count; j++) {
									enemyBehaviour = enemies [j].GetComponent<EnemyBehaviour> ();
									//SPECIAL DO MAGO
									if (enemyBehaviour.IsSelected) {		// Se um inimigo foi selecionado
										List<GameObject> list = new List<GameObject>();
										list.Add (enemyBehaviour.gameObject);
										mageBehaviour.SpecialCommand (list);
									}
								}
							}
						}
					}
				}
			}
			// Verifica quantos enemies estão em espera para atacar
			for (i = 0; i < enemies.Count; i++) {
				enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
				if ((enemyBehaviour.State != EnemyBehaviour.STATE.WAITATTACK && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
					|| enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN)
					break;
				counter++;
			}
			if (counter == enemies.Count || (allEnemiesFrozen && timerFrozen > enemies[0].GetComponent<EnemyBehaviour>().time)) { 		// Se todos os inimigos estão esperando, entra no turno de espera
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
			//print ("Turno de espera");
			for (i = 0; i < players.Count; i++) {
				playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
				if (playerBehaviour.State != PlayerBehaviour.STATE.NOTSELECTED && playerBehaviour.State != PlayerBehaviour.STATE.SPECIAL &&
				    playerBehaviour.State != PlayerBehaviour.STATE.FROZEN) {
					break;
					print ("Wait turn");
				}
				counter++;
			}
			if (counter == players.Count) {		// Se todos os players pararam de agir, vai para o turno do inimigo
				print("counter");
				turn = TURN.ENEMYTURN;
				for (i = 0; i < enemies.Count; i++) {
					print ("Loop");
					enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
					enemyBehaviour.State = EnemyBehaviour.STATE.ATTACKING;
					enemyBehaviour.anim.SetInteger ("State", 2);
					//print ("Setou estado 2");
					//enemyBehaviour.Attack ();
				}
				for (i = 0; i < players.Count; i++) {
					playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
					if (playerBehaviour is WarriorBehaviour && playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL) {
						WarriorBehaviour warrior = (WarriorBehaviour)playerBehaviour;
						warrior.anim.SetInteger ("State", 2);
						warrior.Special = warrior.Special - warrior.SpecialValue;
						warriorSpecialSlider.value = warrior.Special;
					}
				}
			}
		} else if (turn == TURN.ENEMYTURN) {
			timerShockwave += Time.deltaTime;
			// Checa se todos os inimigos ainda estão atacando
			for (i = 0; i < enemies.Count; i++) {
				enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
				if (enemyBehaviour.State != EnemyBehaviour.STATE.NOTSELECTED && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
					break;
				counter++;
			}
			if (counter == enemies.Count && timerShockwave >= 1f) {
				CheckCollisions ();
				timerShockwave = 0f;
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
						//print ("Setou 1");
					}
				}
				if (counterEnemiesFrozen == enemies.Count) {
					allEnemiesFrozen = true;
					print ("All enemies frozen");
				}
				else
					allEnemiesFrozen = false;
				for (i = 0; i < players.Count; i++) {
					PlayerBehaviour playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
					playerBehaviour.State = PlayerBehaviour.STATE.NOTSELECTED;
					print ("Not selected state on check");
					playerBehaviour.IsColliding = false;
					playerBehaviour.anim.SetInteger ("State", 0);
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
		
	// Checa se, nesse turno, algum player foi atacado
	public void CheckCollisions(){
		List<PlayerBehaviour> playersColliding = new List<PlayerBehaviour>();
		int i;
		// Verifica quais os players que estão colidindo
		for (i = 0; i < players.Count; i++) {
			PlayerBehaviour playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.IsColliding) {
				playersColliding.Add (playerBehaviour);
				print ("Colision checked");
			}
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
		/*
		GameObject attackObject;
		while ((attackObject = GameObject.Find ("StraightLineAttack")) != null)
			Destroy (attackObject);
		*/
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
				if (player.Life > 0) {
					if (player is WarriorBehaviour)
						warriorLifeSlider.value = player.Life;
					else if (player is MageBehaviour)
						mageLifeSlider.value = player.Life;
					else if (player is ArcherBehaviour)
						archerLifeSlider.value = player.Life;
				} else {		// MATA O PLAYER
					if (player is WarriorBehaviour)
						Destroy (warriorUI);
					else if (player is MageBehaviour)
						Destroy (mageUI);
					else if (player is ArcherBehaviour)
						Destroy (archerUI);
					// GameManager.players.Remove (player.gameObject);	TIRA O COMENTÁRIO QUANDO TIVER AS REFERÊNCIAS PRONTAS
					players.Remove (player.gameObject);
					Destroy(player.gameObject);
				}
			}
		}
	}

	void FreezeOgre(Collider2D coll){
		EnemyBehaviour enemyBehaviour;
		for (int i = 0; i < enemies.Count; i++) {
			enemyBehaviour = enemies [i].GetComponent<EnemyBehaviour> ();
			if (enemyBehaviour.AttackObject == coll.gameObject) {
				enemyBehaviour.State = EnemyBehaviour.STATE.FROZEN;
				//print ("Congelou");
			}
		}
	}

	public void EnemySelected(EnemyBehaviour enemyBehaviour){
		PlayerBehaviour playerBehaviour;
		for (int i = 0; i < players.Count; i++) {
			playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.State == PlayerBehaviour.STATE.WAITATTACK || playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL) {
				enemyBehaviour.IsSelected = true;
				return;
			}
		}
	}

	// Mostra a UI de batalha posicionada de acordo com o player passado como parâmetro
	public void ShowBattleUI(GameObject player){
		int i;
		PlayerBehaviour playerBehaviour;
		for (i = 0; i < players.Count; i++) {
			playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (!players [i].Equals (player) && (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) || 
				playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL)
				players [i].GetComponent<PlayerBehaviour> ().State = PlayerBehaviour.STATE.NOTSELECTED;
		}
		battleUI.transform.position = player.transform.position;
		battleUI.SetActive (true);
	}

	// Evento de clique no btnAttack. Verifica qual player que tá selecionado e desencadeia seu ataque.
	public void OnClickAttack(){
		int i;
		PlayerBehaviour playerBehaviour;
		for (i = 0; i < players.Count; i++) {
			playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) {
				playerBehaviour.State = PlayerBehaviour.STATE.WAITATTACK;
				battleUI.SetActive (false);
				return;
			}
		}
	}

	// Evento de clique no btnSpecial. Verifica qual player que tá selecionado e desencadeia seu special.
	public void OnClickSpecial(){
		int i;
		PlayerBehaviour playerBehaviour;
		for (i = 0; i < players.Count; i++) {
			playerBehaviour = players [i].GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) {
				playerBehaviour.State = PlayerBehaviour.STATE.SPECIAL;
				battleUI.SetActive (false);
				return;
			}
		}
	}

	public void DestroyEnemy(EnemyBehaviour enemyBehaviour){
		// GameManager.enemies.Remove (enemyBehaviour.gameObject);	TIRA O COMENTÁRIO QUANDO TIVER AS REFERÊNCIAS PRONTAS
		enemies.Remove (enemyBehaviour.gameObject);
		Destroy(enemyBehaviour.gameObject);
	}

}
