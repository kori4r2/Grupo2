using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : PlayerBehaviour {

	private float specialValue = 5f;

	void Start () {
	}

	public override void SpecialCommand(List<GameObject> enemies){
		int i;
		EnemyBehaviour enemy;
		State = STATE.ONSPECIAL;
		Special = Special - specialValue;
		battleManager.archerSpecialSlider.value = Special;
		for (i = 0; i < enemies.Count; i++) {
			enemy = enemies [i].GetComponent<EnemyBehaviour> ();
			float attack = enemy.Life - attackValue + enemy.Defense - specialValue;
			enemy.Life = attack;
			if (enemy.Life <= 0)
				battleManager.DestroyEnemy (enemy);
			else
				print ("enemy life = " + enemy.Life);
		}
	}

	public override string Name{
		get{ return "Arqueiro";}
		set{}
	}
}
