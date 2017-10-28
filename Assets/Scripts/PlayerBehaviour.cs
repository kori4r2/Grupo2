/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que descreve o comportamento do personagem*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE {NOTSELECTED, SELECTED, MOVING} // possiveis estados do personagem

public class PlayerBehaviour : MonoBehaviour {

	public float teste;
 
	public float speed; // velocidade da movimentacao do personagem
	private Vector2 initialPosition; // posicao inicial do personagem
	private Vector2 finalPosition; // posicao para a qual o personagem vai se mover
	private STATE state; // determina o estado do personagem

	public Vector2 FinalPosition {
		get {
			return finalPosition;
		}
		set { 
			finalPosition = value; 
		}
	}

	public int State {
		get {
			return state;
		}
		set { 
			state = value; 
		}
	}

	// Use this for initialization
	void Start () {
		initialPosition = gameObject.transform.position; // pega a posicao inicial do personagem
		state = STATE.NOTSELECTED;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == STATE.MOVING) {
			speed = 1f;
			teste += Time.deltaTime / 1f;
			transform.position = Vector2.Lerp (initialPosition, finalPosition, teste);
		}

	}

	// Funcao que detecta a colisao entre um objeto com colisor e o mouse
	void OnMouseDown() {
		state = STATE.SELECTED; // DEVERIA ABRIR O MENU MAS ESTAMOS PULANDO ESTA ETAPA POR ENQUANTO! MODIFICAR FUTURAMENTE	
	}

}
