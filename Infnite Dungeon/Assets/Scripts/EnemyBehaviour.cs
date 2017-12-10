using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public enum STATE {NOTSELECTED, FROZEN, MOVING, WAITATTACK, ATTACKING} // possiveis estados do personagem


	public float speed; // velocidade da movimentacao do personagem
	public float time;	// tempo esperado para se mover

	private Vector2 initialPosition; // posicao inicial do personagem
	private Vector2 finalPosition; // posicao para a qual o personagem vai se mover
	private STATE state; // determina o estado do personagem

	private float life = 100f;
	private float attackValue = 60f;
	private float defense = 5f;

	public GameObject prefabAttack;	//-3,23
	private GameObject attackObject;
	public BattleManager battleManager;

	private bool isSelected = false;

	public Animator anim;

	private int turnsFrozen = 0;

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
			life = value;
		}
	}

	public float Defense{
		get{
			return defense;
		}
		set{
			defense = value;
		}
	}

	public GameObject AttackObject{
		get{
			return attackObject;
		}
	}

	public float AttackValue{
		get{
			return attackValue;
		}
	}

	public bool IsSelected{
		get{
			return isSelected;
		}
		set{
			isSelected = value;
		}
	}

	public int TurnsFrozen{
		get{
			return turnsFrozen;
		}
		set{
			turnsFrozen = value;
		}
	}

	// Use this for initialization
	void Start () {
		//state = STATE.NOTSELECTED;
		initialPosition = transform.position;
		time = 5f;
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == STATE.MOVING) { 
			if (finalPosition.x != transform.position.x || finalPosition.y != transform.position.y)
				transform.position = Vector3.MoveTowards (transform.position, finalPosition, speed * Time.deltaTime);
			else {
				//print ("Wait attack");
				state = STATE.WAITATTACK;
				anim.SetInteger ("State", 0);
				//print ("Setou estado 0");
			}
		}
	}

	// Funcao que detecta a colisao entre um objeto com colisor e o mouse
	void OnMouseDown() {
		battleManager.EnemySelected (this);
	}

	// Gera o objeto de ataque
	public void Attack(){
		//Transform positionAttack = new GameObject ().transform;
		//positionAttack = (transform.position.x, transform.position.y - 3.23f);

		attackObject = Instantiate (prefabAttack, transform.position, transform.rotation);
		print ("Instanciou");
		anim.SetInteger ("State", 0);
		state = EnemyBehaviour.STATE.NOTSELECTED;

		/*
		float attack = character.GetComponent<PlayerBehaviour> ().Life - attackValue;
		character.GetComponent<PlayerBehaviour> ().Life = attack;
		state = STATE.NOTSELECTED;
		print ("Vida character = " + character.GetComponent<PlayerBehaviour> ().Life);
		*/
	}

	// Define o próximo destino do inimigo
	public void Move(){
		initialPosition = transform.position;
		finalPosition.x = Random.Range (-2.29f, 2.29f);
		finalPosition.y = Random.Range (1.21f, 4.28f);
		float distance = Mathf.Sqrt (Mathf.Pow (initialPosition.x - finalPosition.x, 2) + Mathf.Pow (initialPosition.y - finalPosition.y, 2));
		speed = distance / time;
		anim.SetInteger ("State", 1);
		state = STATE.MOVING;
	}
}
