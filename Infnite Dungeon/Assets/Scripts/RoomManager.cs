/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que gera as salas, seus detalhes e a porta.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

	public List<Sprite> rooms;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = rooms[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
