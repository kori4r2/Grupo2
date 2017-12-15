using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : PlayerBehaviour {

	private float specialValue = 5f;

	void Start () {
		stateAttack = 4;
		stateSpecial = 4;
	}

	public override void SpecialCommand(List<GameObject> enemies){
		int i;
		enemiesAttacking = enemies;
		State = STATE.ONSPECIAL;
		Special = Special - specialValue;
		battleManager.archerSpecialSlider.value = Special;
		anim.SetInteger ("State", 2);
	}

	public override string Name{
		get{ return "Arqueiro";}
		set{}
	}

	public override void FinishSpecial(){
		int i;
		EnemyBehaviour enemy;
		GameObject attackObject;
		for (i = 0; i < enemiesAttacking.Count; i++) {
			enemy = enemiesAttacking [i].GetComponent<EnemyBehaviour> ();
			attackObject = Instantiate (prefabAttack, enemiesAttacking [i].transform.position, Quaternion.identity);
			attackObject.GetComponent<Animator> ().SetInteger ("State", stateSpecial);
			float attack = enemy.Life - attackValue + enemy.Defense - specialValue;
			enemy.Life = attack;
			if (enemy.Life <= 0)
				battleManager.DestroyEnemy (enemy);
			else
				print ("enemy life special archer = " + enemy.Life);
		}
	}
}
