using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public enum STATE {NOTSELECTED, SELECTED, MOVING, ATTACKING} // possiveis estados do personagem

	public GameObject character;

	public float speed; // velocidade da movimentacao do personagem
	private Vector2 currentPosition; // posicao inicial do personagem
	private Vector2 finalPosition; // posicao para a qual o personagem vai se mover
	private STATE state; // determina o estado do personagem
	private float life = 100f;

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


	// Use this for initialization
	void Start () {
		state = STATE.NOTSELECTED;
		currentPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Funcao que detecta a colisao entre um objeto com colisor e o mouse
	void OnMouseDown() {
		state = STATE.SELECTED;
	}
}
