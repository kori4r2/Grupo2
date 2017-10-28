/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que comanda as ações principais do jogo, como quando surgem os inimigos, os personagens, distribui a experiencia, etc.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private Vector2 pointToGo; // Ponto ate onde o personagem vai se mover
	public GameObject personagem; // Referencia ao personagem, o objeto que sera movido

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			if (personagem.GetComponent<PlayerBehaviour> ().State == PlayerBehaviour.STATE.SELECTED) {
				pointToGo = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
				personagem.GetComponent<PlayerBehaviour> ().FinalPosition = pointToGo;
				personagem.GetComponent<PlayerBehaviour> ().State = State.MOVING;
			}
		}

	}



}
