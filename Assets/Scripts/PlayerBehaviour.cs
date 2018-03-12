/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que descreve o comportamento do personagem*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBehaviour : MonoBehaviour {

	public enum STATE {NOTSELECTED, SELECTED, MOVING, WAITATTACK, ATTACKING, SPECIAL, ONSPECIAL, FROZEN} // possiveis estados do personagem

	private Vector2 currentPosition; // posicao inicial do personagem
	private Vector2 finalPosition; // posicao para a qual o personagem vai se mover
	public STATE state; // determina o estado do personagem

	public abstract void SpecialCommand(List<GameObject> enemies);
	//public abstract void FinishAttack ();
	public abstract void FinishSpecial();
	public abstract string Name{ get; set;} // Declarado como abstract para retornar o nome de acordo com a classe

	private float speed = 2f; // velocidade da movimentacao do personagem
	protected float attackValue;
    protected float specialValue;
    protected float specialCost;
	private float life = 100f;
	private float special = 100f;
	protected float defense;
	protected int level;
	protected int actionsOnTurn = 0;
    private Vector2 upperLeftLimit = new Vector2(-2.5f, -0.2f);
    private Vector2 lowerRightLimit = new Vector2(2.50f, -3.35f);

    protected int stateAttack;
	protected int stateSpecial;

	/*
	public GameObject battleUI;		// GameObject that contains all UI Battle components
	public GameObject attackButton;		
	public GameObject specialButton;
*/
	public BattleManager battleManager;

	private bool isColliding = false;
	private Collider2D col2D;

	public Animator anim;

	public List<GameObject> enemiesAttacking;

	public GameObject prefabAttack;

	public Vector2 FinalPosition {
		get {
			return finalPosition;
		}
		set {
            float x = value.x;
            float y = value.y;

            print("Moving towards: (" + x + ", " + y + ")");

            x = (x < upperLeftLimit.x) ? upperLeftLimit.x : (x > lowerRightLimit.x) ? lowerRightLimit.x : x;
            y = (y > upperLeftLimit.y) ? upperLeftLimit.y : (y < lowerRightLimit.y) ? lowerRightLimit.y : y;

			finalPosition.x = x;
            finalPosition.y = y;
		}
	}

	public STATE State {
		get {
			return state;
		}
		set {
			state = value; 
		}
	}

	public float Life{
		get{
			return life;
		}
		set{
            life = (value < 0)? 0 : (value > 100)? 100 : value;
            if (battleManager != null) {
                switch (Name) {
                    case "Guerreiro":
                        battleManager.warriorLifeSlider.value = Life;
                        break;
                    case "Mago":
                        battleManager.mageLifeSlider.value = Life;
                        break;
                    case "Arqueiro":
                        battleManager.archerLifeSlider.value = Life;
                        break;
                    default:
                        throw new System.Exception("Life.set(): This class doesn't exist");
                }
            }
            if (Life <= 0) {
                switch (Name) {
                    case "Guerreiro":
                        Destroy(battleManager.warriorUI);
                        break;
                    case "Mago":
                        Destroy(battleManager.mageUI);
                        break;
                    case "Arqueiro":
                        Destroy(battleManager.archerUI);
                        break;
                }
                GameManager.players.Remove(gameObject);
                if (battleManager != null)
                    battleManager.players.Remove(this);
                Destroy(gameObject);
            }
        }
	}

	public float Special{
		get{
			return special;
		}
		set {
            special = (value < 0) ? 0 : (value > 100) ? 100 : value;
            if (battleManager != null) {
                switch (Name) {
                    case "Guerreiro":
                        battleManager.warriorSpecialSlider.value = Special;
                        break;
                    case "Mago":
                        battleManager.mageSpecialSlider.value = Special;
                        break;
                    case "Arqueiro":
                        battleManager.archerSpecialSlider.value = Special;
                        break;
                    default:
                        throw new System.Exception("Special.set(): This class doesn't exist");
                }
            }
        }
	}

	public float Defense{
		get{
			return defense;
		}
        protected set {
			defense = value;
		}
	}

	public bool IsColliding{
		get{
			return isColliding;
		}
        set {
			isColliding = value;
		}
	}

	public Collider2D Collider{
		get{
			return col2D;
		}
	}

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		currentPosition = gameObject.transform.position; // pega a posicao inicial do personagem
		state = STATE.FROZEN;
	}

	void Update () {
		if (state == STATE.MOVING) {
			if (finalPosition.x != transform.position.x || finalPosition.y != transform.position.y) {
				transform.position = Vector3.MoveTowards (transform.position, finalPosition, speed * Time.deltaTime);
			} else {
				state = STATE.FROZEN;
				anim.SetInteger ("State", 0);
			}
		}

	}
		
	// Funcao que detecta a colisao entre um objeto com colisor e o mouse
	void OnMouseDown() {
		if (GameManager.isPotionSelected) {
			life += 50;
			print("Life player after potion= " + life);
			GameManager.isPotionSelected = false;
			GameManager.RewardPotion (-1);
		}else if (battleManager != null && battleManager.Turn == BattleManager.TURN.PLAYERTURN && state == STATE.NOTSELECTED) {
			battleManager.ShowBattleUI (this.gameObject);
            if (Special <= 0)
                battleManager.specialButton.SetActive(false);
            else
                battleManager.specialButton.SetActive(true);
			state = STATE.SELECTED;
			currentPosition = transform.position;
		}
	}

	public void Attack(GameObject enemy){
		enemiesAttacking.Add (enemy);
		state = STATE.ATTACKING;
		anim.SetInteger ("State", 3);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "EnemyAttack") {
			isColliding = true;
			col2D = coll;
		}
	}

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "EnemyAttack") {
            isColliding = false;
            col2D = null;
        }
    }

    public void SetFrozen() {
        state = STATE.FROZEN;
        anim.SetInteger("State", 0);
    }

    public void SetNotSelected(){
		state = STATE.NOTSELECTED;
		anim.SetInteger ("State", 0);
	}

    public void SetStateIdle() {
        anim.SetInteger("State", 0);
    }

    public void TakeDamage(float enemyAttack) {
        Life -= (enemyAttack - Defense);
    }
}