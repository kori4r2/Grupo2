using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : PlayerBehaviour {

	private float specialValue = 5f;

	public void Special(List<GameObject> enemies){
		int i;
		EnemyBehaviour enemy;
		State = STATE.ONSPECIAL;
		for (i = 0; i < enemies.Count; i++) {
			enemy = enemies [i].GetComponent<EnemyBehaviour> ();
			float attack = enemy.Life - attackValue + enemy.Defense - specialValue;
			enemy.Life = attack;
			print ("Vida enemy = " + enemy.Life);
		}
	}
}
