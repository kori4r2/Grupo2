using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public enum STATE {NOTSELECTED, FROZEN, MOVING, WAITATTACK, ATTACKING} // possiveis estados do personagem


	public float speed; // velocidade da movimentacao do personagem
	public float time = 5f;	// tempo esperado para se mover

	private Vector2 initialPosition; // posicao inicial do personagem
	private Vector2 finalPosition; // posicao para a qual o personagem vai se mover
	private STATE state; // determina o estado do personagem

	private float life = 100f;
	private float attackValue = 30f;
	private float defense = 0f;

	public GameObject prefabAttack1;	//-3,23
	public GameObject prefabAttack2;
	private GameObject attackObject;
    public GameObject UIElement;
	public BattleManager battleManager;

	private bool isSelected = false;

	public Animator anim;

	public int attackType;

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
        set {
            attackObject = value;
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

	public int AttackType{
		get{
			return attackType;
		}
		set{
			attackType = value;
		}
	}

	// Use this for initialization
	void Start () {
		turnsFrozen = 0;
		initialPosition = transform.position;
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == STATE.MOVING) {
            if (finalPosition.x != transform.position.x || finalPosition.y != transform.position.y) {
                print("moving towards destination");
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, speed * Time.deltaTime);
            } else {
                print ("Wait attack");
                state = STATE.WAITATTACK;
                anim.SetInteger("State", 0);
                //print ("Setou estado 0");
            }
		}
	}

	// Funcao que detecta a colisao entre um objeto com colisor e o mouse
	void OnMouseDown() {
		battleManager.EnemySelected (this);
	}

    // Gera o objeto de ataque
    public void Attack() {
        if (attackType == 1) {
            attackObject = Instantiate(prefabAttack1, new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z), transform.rotation);
            attackObject.GetComponent<LineAttack>().owner = this;
        } else {
            attackObject = Instantiate(prefabAttack2, new Vector3(transform.position.x, transform.position.y - 4f, transform.position.z), transform.rotation);
            attackObject.GetComponent<ThrowAttack>().owner = this;
        }
        attackObject.name = "StraightLineAttack";
        anim.SetInteger("State", 0);
        //DEBUG
        print("State = " + anim.GetInteger("State"));
        print("Instanciou ataque");
    }

    public void TakeDamage(float attack) {
        life -= attack - defense;
        if (life <= 0) {
            Destroy(UIElement);
            battleManager.DestroyEnemy(this);
        }else {
            print("Vida enemy = " + life);
        }
    }

	// Define o próximo destino do inimigo
	public void Move(){
		initialPosition = transform.position;
        float distance = 0f;
        while (distance <= 0.3) {
            finalPosition.x = Random.Range(-2.29f, 2.29f);
            finalPosition.y = Random.Range(1.21f, 2.17f);
            distance = Mathf.Sqrt(Mathf.Pow(initialPosition.x - finalPosition.x, 2) + Mathf.Pow(initialPosition.y - finalPosition.y, 2));
        }
		speed = distance / time;
		anim.SetInteger ("State", 1);
        print("found new target position, speed = " + speed + ", time = " + time + ", distance = " + distance);
        state = STATE.MOVING;
	}

    public void SetInactive() {
        state = STATE.NOTSELECTED;
    }
}
