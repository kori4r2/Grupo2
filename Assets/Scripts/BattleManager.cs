using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour {

	public enum TURN {CHECK, PLAYERTURN, WAITTURN, ENEMYTURN} // possiveis estados do jogo

	private Vector2 pointToGo; // Ponto ate onde o character vai se mover
	private Vector2 initialPosition; // Posicao inicial do clique do mouse fora do player
	private Vector2 offset;

	public GameObject battleUI;			// GameObject that contains all UI Battle components
	public GameObject specialButton;
	public Text potionTxt;
	public GameObject playerUI;
	public GameObject enemyUI;

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
	public Slider ogreLifeSlider1;
	public Slider ogreLifeSlider2;

	public GameObject warriorUI;
	public GameObject mageUI;
	public GameObject archerUI;
	public GameObject ogreUI1;
	public GameObject ogreUI2;

	public GameObject prefabWarriorUI;
	public GameObject prefabMageUI;
	public GameObject prefabArcherUI;
	public GameObject prefabOgreUI1;
	public GameObject prefabOgreUI2;

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
		float x = -2.23f, y = -3.0f, z = 0f;
		EnemyBehaviour enemyBehaviour;
		PlayerBehaviour playerBehaviour;

		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		if (Random.Range (1, 3) == 1)
			gameManager.InstantiateOgre (0f, 3.0f, 0f, (int)Random.Range(1,3));
		else {
			gameManager.InstantiateOgre (-1f, 3.0f, 0f, 1);
			gameManager.InstantiateOgre (1f, 3.0f, 0f, 2);
		}
		// Para cada player no gameManager, instancia-o na cena de batalha, bem como seus elementos de UI.
		foreach (GameObject player in gameManager.players) {
			players.Add(player);
			player.transform.position = new Vector3 (x, y, z);
			playerBehaviour = player.GetComponent<PlayerBehaviour> ();
			playerBehaviour.battleManager = this;
			if(playerBehaviour is WarriorBehaviour){
				warriorUI = Instantiate (prefabWarriorUI, transform.position, Quaternion.identity);
				warriorUI.transform.SetParent (playerUI.transform, false);
				warriorLifeSlider = GameObject.Find ("WarriorLifeSlider").GetComponent<Slider> ();
				warriorSpecialSlider = GameObject.Find ("WarriorSpecialSlider").GetComponent<Slider> ();
			}
			else if(playerBehaviour is MageBehaviour){
				mageUI = Instantiate (prefabMageUI, transform.position, Quaternion.identity);
                mageUI.transform.SetParent(playerUI.transform, false);
                mageLifeSlider = GameObject.Find ("MageLifeSlider").GetComponent<Slider> ();
				mageSpecialSlider = GameObject.Find ("MageSpecialSlider").GetComponent<Slider> ();
			}
			else if(playerBehaviour is ArcherBehaviour){
				archerUI = Instantiate (prefabArcherUI, transform.position, Quaternion.identity);
                archerUI.transform.SetParent(playerUI.transform, false);
                archerLifeSlider = GameObject.Find ("ArcherLifeSlider").GetComponent<Slider> ();
				archerSpecialSlider = GameObject.Find ("ArcherSpecialSlider").GetComponent<Slider> ();
			}
			x += 2.23f;
		}

		foreach (GameObject enemy in gameManager.enemies) {
			enemies.Add (enemy);
			if (enemy.GetComponent<EnemyBehaviour> ().AttackType == 1) {
				ogreUI1 = Instantiate (prefabOgreUI1, transform.position, Quaternion.identity);
				ogreUI1.transform.parent = enemyUI.transform;
                enemy.GetComponent<EnemyBehaviour>().UIElement = ogreUI1;
				ogreLifeSlider1 = GameObject.Find ("OgreLifeSlider1").GetComponent<Slider> ();
			}else{
				ogreUI2 = Instantiate (prefabOgreUI2, transform.position, Quaternion.identity);
				ogreUI2.transform.parent = enemyUI.transform;
                enemy.GetComponent<EnemyBehaviour>().UIElement = ogreUI2;
                ogreLifeSlider2 = GameObject.Find ("OgreLifeSlider2").GetComponent<Slider> ();
			}

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

		foreach (GameObject enemy in enemies) {
			enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
			if (enemyBehaviour.AttackType == 1)
				ogreLifeSlider1.value = enemyBehaviour.Life;
			else
				ogreLifeSlider2.value = enemyBehaviour.Life;
		}

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

				foreach (GameObject player in players) {
					playerBehaviour = player.GetComponent<PlayerBehaviour> ();
					// MOVIMENTAÇÃO DO PLAYER
					if ((playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) && // Se há um player selecionado e não tocou em um colisor
					    ((!Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
							Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0f))) &&
						(Camera.main.ScreenToWorldPoint (Input.mousePosition).y <= -0.26f && Camera.main.ScreenToWorldPoint (Input.mousePosition).y >= -3.75f )) {
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
							foreach (GameObject enemy in enemies) {
								enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
								//ATAQUE DO PLAYER
								if (enemyBehaviour.IsSelected) {		// Se um inimigo foi selecionado
									playerBehaviour.Attack (enemy);
								}
							}
						} else if (playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL && playerBehaviour is MageBehaviour) {	// Se o player está em special
							 //else if (playerBehaviour is MageBehaviour) {	// Se é o special do mage
							MageBehaviour mageBehaviour = (MageBehaviour) playerBehaviour;
							foreach (GameObject enemy in enemies) {
								enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
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
			// Verifica quantos enemies estão em espera para atacar
			foreach (GameObject enemy in enemies) {
				print ("Preso aqui");
				enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
				if ((enemyBehaviour.State != EnemyBehaviour.STATE.WAITATTACK && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
					/*|| enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN*/)
					break;
				counter++;
			}
			if (counter == enemies.Count || (allEnemiesFrozen && timerFrozen > enemies[0].GetComponent<EnemyBehaviour>().time)) { 		// Se todos os inimigos estão esperando, entra no turno de espera
				turn = TURN.WAITTURN;
				foreach (GameObject player in players) {		// Verifica se há um player prestes a agir
					playerBehaviour = player.GetComponent<PlayerBehaviour> ();
					if (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED || playerBehaviour.State == PlayerBehaviour.STATE.WAITATTACK ||
						playerBehaviour.State == PlayerBehaviour.STATE.FROZEN) {
						playerBehaviour.State = PlayerBehaviour.STATE.NOTSELECTED;
						battleUI.SetActive (false);
					}
				}
				foreach (GameObject enemy in enemies)
					enemy.GetComponent<EnemyBehaviour> ().IsSelected = false;
			}
		} else if (turn == TURN.WAITTURN) { 		// Se estão no turno de espera
			//print ("Turno de espera");
			foreach (GameObject player in players) {
				playerBehaviour = player.GetComponent<PlayerBehaviour> ();
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
				foreach (GameObject player in players) {
					playerBehaviour = player.GetComponent<PlayerBehaviour> ();
					if (playerBehaviour is WarriorBehaviour && playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL) {
						WarriorBehaviour warrior = (WarriorBehaviour)playerBehaviour;
						warrior.anim.SetInteger ("State", 2);
						warrior.Special = warrior.Special - warrior.SpecialValue;
						warriorSpecialSlider.value = warrior.Special;
					}
				}
				foreach (GameObject enemy in enemies) {
					print ("Loop");
					enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
					if (enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN) {
						enemyBehaviour.State = EnemyBehaviour.STATE.ATTACKING;
						if (enemyBehaviour.AttackType == 1)
							enemyBehaviour.anim.SetInteger ("State", 2);
						else
							enemyBehaviour.anim.SetInteger ("State", 3);
					}
					//print ("Setou estado 2");
					//enemyBehaviour.Attack ();
				}
			}
		} else if (turn == TURN.ENEMYTURN) {
			timerShockwave += Time.deltaTime;
			// Checa se todos os inimigos ainda estão atacando
			foreach (GameObject enemy in enemies) {
				enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
				//if (enemyBehaviour.State != EnemyBehaviour.STATE.NOTSELECTED && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
				if(enemyBehaviour.State == EnemyBehaviour.STATE.ATTACKING)
					break;
				counter++;
			}
			if (counter == enemies.Count && timerShockwave >= 1f) {
				CheckCollisions ();
				timerShockwave = 0f;
			}
		} 
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

				foreach (GameObject enemy in enemies) {
					enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
					// O inimigo inicia o turno se ele não estiver congelado ou se estiver congelado mas já tenha ficado 1 turno assim
					if ((enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN && enemyBehaviour.TurnsFrozen == 1) ||
					    enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN) {
						print ("State no move = " + enemyBehaviour.State);
						print ("Turns frozen = " + enemyBehaviour.TurnsFrozen);
						enemyBehaviour.Move ();
						enemyBehaviour.TurnsFrozen = 0;
					} else if(enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN && enemyBehaviour.TurnsFrozen == 0){
						counterEnemiesFrozen++;
						enemyBehaviour.TurnsFrozen = 1;
						print ("Aqui turns frozen 11111");
						//print ("Setou 1");
					}
				}
				if (counterEnemiesFrozen == enemies.Count) {
					allEnemiesFrozen = true;
					print ("All enemies frozen");
				}
				else
					allEnemiesFrozen = false;
				foreach (GameObject player in players) {
					PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour> ();
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
        foreach(GameObject go in enemies) {
            Destroy(go);
        }
        foreach (GameObject go in players) {
            Destroy(go);
        }
        gameManager.enemies.Clear();
        enemies.Clear();
        players.Clear();
		gameManager.players.Clear();
		GameManager.potions = 0;
		Application.LoadLevel ("MainMenu");
	}

	void NextRoom(){
        gameManager.enemies.Clear();
        Application.LoadLevel ("ExplorationScene");
	}
		
	// Checa se, nesse turno, algum player foi atacado
	public void CheckCollisions(){
		List<PlayerBehaviour> playersColliding = new List<PlayerBehaviour>();
		int i;
		// Verifica quais os players que estão colidindo
		foreach (GameObject player in players) {
			PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.IsColliding) {
				playersColliding.Add (playerBehaviour);
				print ("Colision checked");
			}
		}

		if (playersColliding.Count > 0) {
			// Ordena pelo menor y
			playersColliding.Sort (delegate(PlayerBehaviour x, PlayerBehaviour y) {
				return(x.transform.position.y).CompareTo (y.transform.position.y);
			});


			// Verifica se há um warrior no meio do caminho usando special
			foreach(PlayerBehaviour player in playersColliding){
				if (player is WarriorBehaviour && player.State == PlayerBehaviour.STATE.SPECIAL) {
					WarriorBehaviour warrior = (WarriorBehaviour)player;
					warrior.ShieldAnim ();
					FreezeOgre (player.Collider);
					break;
				}
				// Se não é um warrior usando special, recebe dano
				print ("Colide");
				SetLifePlayer (player.Collider, player);
				player.IsColliding = false;
			}

		}			

		// Destrói todos os objetos de ataque

		foreach (GameObject enemy in enemies) {
			EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
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
		foreach (GameObject enemy in enemies) {
			enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
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
		foreach (GameObject enemy in enemies) {
			enemyBehaviour = enemy.GetComponent<EnemyBehaviour> ();
			if (enemyBehaviour.AttackObject == coll.gameObject) {
				enemyBehaviour.State = EnemyBehaviour.STATE.FROZEN;
				print ("Congelou");
			}
		}
	}

	public void EnemySelected(EnemyBehaviour enemyBehaviour){
		PlayerBehaviour playerBehaviour;
		foreach (GameObject player in players) {
			playerBehaviour = player.GetComponent<PlayerBehaviour> ();
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
		foreach (GameObject otherPlayer in players) {
			playerBehaviour = otherPlayer.GetComponent<PlayerBehaviour> ();
			if (!otherPlayer.Equals (player) && (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) || 
				playerBehaviour.State == PlayerBehaviour.STATE.SPECIAL)
				otherPlayer.GetComponent<PlayerBehaviour> ().State = PlayerBehaviour.STATE.NOTSELECTED;
		}
		battleUI.transform.position = player.transform.position;
		battleUI.SetActive (true);
	}

	// Evento de clique no btnAttack. Verifica qual player que tá selecionado e desencadeia seu ataque.
	public void OnClickAttack(){
		int i;
		PlayerBehaviour playerBehaviour;
		foreach (GameObject player in players) {
			playerBehaviour = player.GetComponent<PlayerBehaviour> ();
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
		foreach (GameObject player in players) {
			playerBehaviour = player.GetComponent<PlayerBehaviour> ();
			if (playerBehaviour.State == PlayerBehaviour.STATE.SELECTED) {
				if (playerBehaviour is ArcherBehaviour) {		// Se é o special do archer
					ArcherBehaviour archerBehaviour = (ArcherBehaviour)playerBehaviour;
					archerBehaviour.SpecialCommand (enemies);
				} else 
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
