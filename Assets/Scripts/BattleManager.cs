using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

	public List<PlayerBehaviour> players;
	public List<EnemyBehaviour> enemies;

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
		float x = -2.23f, y = -3.0f, z = 0f;
		PlayerBehaviour playerBehaviour;
        // Check how many ogres will spawn
		if (Random.Range (1, 3) == 1)
            // In case of only one ogre, spawn a single one with random behaviour
			GameManager.InstantiateOgre (0f, 3.0f, 0f, (int)Random.Range(1,3));
		else {
            // In case of two ogres, spawn one of each behaviour
			GameManager.InstantiateOgre (-1f, 3.0f, 0f, 1);
			GameManager.InstantiateOgre (1f, 3.0f, 0f, 2);
		}
		// Para cada player no gameManager, instancia-o na cena de batalha, bem como seus elementos de UI.
		foreach (GameObject player in GameManager.players) {
            playerBehaviour = player.GetComponent<PlayerBehaviour>();
            players.Add(playerBehaviour);
			player.transform.position = new Vector3 (x, y, z);
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
        int i = 0;
		foreach (GameObject enemy in GameManager.enemies) {
			enemies.Add (enemy.GetComponent<EnemyBehaviour>());
			if (enemies[i].AttackType == 1) {
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
            i++;
		}

		turn = TURN.WAITTURN;
        
		battleUI.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		int counter = 0;

		if (allEnemiesFrozen)
			timerFrozen += Time.deltaTime;
		
		if (turn == TURN.CHECK)
			Check ();
		else if (turn == TURN.PLAYERTURN) {		// Sé é o turno do player e houve um toque na tela
            print("entrou no playerturn");
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				// Verify if a character was selected BEFORE, and not exactly now. 
				// We need to do it to obtain a good synchronization with the OnMouseDown() method from PlayerBehaviour.

				foreach (PlayerBehaviour player in players) {
					// MOVIMENTAÇÃO DO PLAYER
					if ((player.State == PlayerBehaviour.STATE.SELECTED) && // Se há um player selecionado e não tocou em um colisor
					    ((!Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, 
							Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0f))) &&
						(Camera.main.ScreenToWorldPoint (Input.mousePosition).y <= -0.26f && Camera.main.ScreenToWorldPoint (Input.mousePosition).y >= -3.75f )) {
						pointToGo = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
						player.FinalPosition = pointToGo;
						player.State = PlayerBehaviour.STATE.MOVING;
						//print ("State moving");
						player.anim.SetInteger ("State", 1);
						battleUI.SetActive (false);

						break;
					} else {
						initialPosition = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
						if (player.State == PlayerBehaviour.STATE.WAITATTACK) {		// Se o player vai atacar
							foreach (EnemyBehaviour enemy in enemies) {
								//ATAQUE DO PLAYER
								if (enemy.IsSelected) {		// Se um inimigo foi selecionado
									player.Attack (enemy.gameObject);
								}
							}
						} else if (player.State == PlayerBehaviour.STATE.SPECIAL && player is MageBehaviour) {	// Se o player está em special
							 //else if (playerBehaviour is MageBehaviour) {	// Se é o special do mage
							MageBehaviour mageBehaviour = (MageBehaviour) player;
							foreach (EnemyBehaviour enemy in enemies) {
								//SPECIAL DO MAGO
								if (enemy.IsSelected) {		// Se um inimigo foi selecionado
									List<GameObject> list = new List<GameObject>();
									list.Add (enemy.gameObject);
									mageBehaviour.SpecialCommand (list);
								}
							}
						}
					}
				}
			}
			// Verifica quantos enemies estão em espera para atacar
			foreach (EnemyBehaviour enemy in enemies) {
				if ((enemy.State != EnemyBehaviour.STATE.WAITATTACK && enemy.State != EnemyBehaviour.STATE.FROZEN)
					/*|| enemyBehaviour.State == EnemyBehaviour.STATE.FROZEN*/)
					break;
				counter++;
			}
			if (counter == enemies.Count || (allEnemiesFrozen && timerFrozen > enemies[0].GetComponent<EnemyBehaviour>().time)) { 		// Se todos os inimigos estão esperando, entra no turno de espera
				turn = TURN.WAITTURN;
				foreach (PlayerBehaviour player in players) {		// Verifica se há um player prestes a agir
					if (player.State == PlayerBehaviour.STATE.SELECTED || player.State == PlayerBehaviour.STATE.WAITATTACK ||
						player.State == PlayerBehaviour.STATE.FROZEN) {
						player.State = PlayerBehaviour.STATE.NOTSELECTED;
						battleUI.SetActive (false);
					}
				}
				foreach (EnemyBehaviour enemy in enemies)
					enemy.IsSelected = false;
			}
		} else if (turn == TURN.WAITTURN) { 		// Se estão no turno de espera
			//print ("Turno de espera");
			foreach (PlayerBehaviour player in players) {
				if (player.State != PlayerBehaviour.STATE.NOTSELECTED &&
                    (player.State != PlayerBehaviour.STATE.SPECIAL || player.Name != "Guerreiro") &&
				    player.State != PlayerBehaviour.STATE.FROZEN) {
                    print("Wait turn");
                    break;
				}
				counter++;
			}
			if (counter == players.Count) {		// Se todos os players pararam de agir, vai para o turno do inimigo
				print("player turn ended");
				turn = TURN.ENEMYTURN;

				foreach (EnemyBehaviour enemy in enemies) {
					if (enemy.State != EnemyBehaviour.STATE.FROZEN) {
						enemy.State = EnemyBehaviour.STATE.ATTACKING;
						if (enemy.AttackType == 1)
							enemy.anim.SetInteger ("State", 2);
						else
							enemy.anim.SetInteger ("State", 3);
					}
					//print ("Setou estado 2");
					//enemyBehaviour.Attack ();
				}
			}
		} else if (turn == TURN.ENEMYTURN) {
			// Checa se todos os inimigos ainda estão atacando
			foreach (EnemyBehaviour enemy in enemies) {
				//if (enemyBehaviour.State != EnemyBehaviour.STATE.NOTSELECTED && enemyBehaviour.State != EnemyBehaviour.STATE.FROZEN)
				if(enemy.State == EnemyBehaviour.STATE.ATTACKING || enemy.AttackObject != null)
					break;
				counter++;
			}
			if (counter == enemies.Count) {
                print("finished enemy turn");
                turn = TURN.CHECK;
			}
		} 
	}

	// Estado de checagem: pode terminar o jogo, chamar a nova sala, ou iniciar o PLAYERTURN
	void Check(){
		timerFrozen = 0f;
		int counterEnemiesFrozen = 0;
		if (players.Count > 0) {
			if (enemies.Count > 0) {
				turn = TURN.PLAYERTURN;

				foreach (EnemyBehaviour enemy in enemies) {
					// O inimigo inicia o turno se ele não estiver congelado ou se estiver congelado mas já tenha ficado 1 turno assim
					if ((enemy.State == EnemyBehaviour.STATE.FROZEN && enemy.TurnsFrozen == 1) ||
					    enemy.State != EnemyBehaviour.STATE.FROZEN) {
						print ("State no move = " + enemy.State);
						print ("Turns frozen = " + enemy.TurnsFrozen);
						enemy.Move ();
						enemy.TurnsFrozen = 0;
					} else if(enemy.State == EnemyBehaviour.STATE.FROZEN && enemy.TurnsFrozen == 0){
						counterEnemiesFrozen++;
						enemy.TurnsFrozen = 1;
						print ("Aqui turns frozen 11111");
						//print ("Setou 1");
					}
				}
				if (counterEnemiesFrozen == enemies.Count) {
					allEnemiesFrozen = true;
					print ("All enemies frozen");
				}else
					allEnemiesFrozen = false;

				foreach (PlayerBehaviour player in players) {
					player.State = PlayerBehaviour.STATE.NOTSELECTED;
					print ("Not selected state on check");
					player.IsColliding = false;
					player.anim.SetInteger ("State", 0);
				}
			}
			else
				NextRoom ();
		} else
			EndGame ();
	}

	void EndGame(){
        foreach(EnemyBehaviour enemy in enemies) {
            Destroy(enemy.gameObject);
        }
        foreach (PlayerBehaviour player in players) {
            Destroy(player.gameObject);
        }
        GameManager.enemies.Clear();
        enemies.Clear();
        players.Clear();
		GameManager.players.Clear();
		GameManager.potions = 0;
	    SceneManager.LoadScene ("MainMenu");
	}

	void NextRoom(){
        GameManager.enemies.Clear();
        SceneManager.LoadScene ("ExplorationScene");
	}
    
	public void EnemySelected(EnemyBehaviour enemyBehaviour){
		foreach (PlayerBehaviour player in players) {
			if (player.State == PlayerBehaviour.STATE.WAITATTACK || player.State == PlayerBehaviour.STATE.SPECIAL) {
				enemyBehaviour.IsSelected = true;
				return;
			}
		}
	}

	// Mostra a UI de batalha posicionada de acordo com o player passado como parâmetro
	public void ShowBattleUI(GameObject player){
		PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
		foreach (PlayerBehaviour otherPlayer in players) {
			if (!otherPlayer.Equals (playerBehaviour) && (otherPlayer.State == PlayerBehaviour.STATE.SELECTED) || 
				otherPlayer.State == PlayerBehaviour.STATE.SPECIAL)
				otherPlayer.State = PlayerBehaviour.STATE.NOTSELECTED;
		}
		battleUI.transform.position = player.transform.position;
		battleUI.SetActive (true);
	}

	// Evento de clique no btnAttack. Verifica qual player que tá selecionado e desencadeia seu ataque.
	public void OnClickAttack(){
		foreach (PlayerBehaviour player in players) {
			if (player.State == PlayerBehaviour.STATE.SELECTED) {
				player.State = PlayerBehaviour.STATE.WAITATTACK;
				battleUI.SetActive (false);
				return;
			}
		}
	}

	// Evento de clique no btnSpecial. Verifica qual player que tá selecionado e desencadeia seu special.
	public void OnClickSpecial(){
		foreach (PlayerBehaviour player in players) {
			if (player.State == PlayerBehaviour.STATE.SELECTED) {
				if (player is ArcherBehaviour) {		// Se é o special do archer
					ArcherBehaviour archerBehaviour = (ArcherBehaviour)player;
					archerBehaviour.SpecialCommand (GameManager.enemies);
				} else if (player is WarriorBehaviour) {
                    WarriorBehaviour warriorBehaviour = (WarriorBehaviour)player;
                    warriorBehaviour.SpecialCommand(new List<GameObject>());
                }else
					player.State = PlayerBehaviour.STATE.SPECIAL;
				battleUI.SetActive (false);
				return;
			}
		}
	}

	public void DestroyEnemy(EnemyBehaviour enemyBehaviour){
		GameManager.enemies.Remove (enemyBehaviour.gameObject);
		enemies.Remove (enemyBehaviour);
		Destroy(enemyBehaviour.gameObject);
	}

}
