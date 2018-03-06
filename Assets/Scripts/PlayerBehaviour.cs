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
	protected STATE state; // determina o estado do personagem

	public abstract void SpecialCommand(List<GameObject> enemies);
	//public abstract void FinishAttack ();
	public abstract void FinishSpecial();
	public abstract string Name{ get; set;} // Declarado como abstract para retornar o nome de acordo com a classe

	private float speed = 2f; // velocidade da movimentacao do personagem
	protected float attackValue;
    protected float specialValue;
	private float life = 100f;
	private float special = 100f;
	protected float defense;
	private int level;
	private float secondary = 100f;
	private int actionsOnTurn = 0;

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
            // TODO: Check if position is within boundaries maybe?
			finalPosition = value; 
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
            // TODO: Truncate to maximum and minimum levels
			life = value;
		}
	}

	public float Special{
		get{
			return special;
		}
		protected set {
            // TODO: Truncate to maximum and minimum levels
            special = value;
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
			if (special <= 0)
				battleManager.specialButton.SetActive (false);
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