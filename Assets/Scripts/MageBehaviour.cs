using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : PlayerBehaviour {
		
	public float specialValue = 20f;

	void Start () {
		stateAttack = 3;
		stateSpecial = 1;
	}

	public override void SpecialCommand(List<GameObject> enemies){
		EnemyBehaviour enemy = enemies [0].GetComponent<EnemyBehaviour> ();
		enemiesAttacking = enemies;
		state = STATE.ONSPECIAL;
		anim.SetInteger ("State", 2);
	}

	public override string Name{
		get{ return "Mago";}
		set{}
	}

	public override void FinishSpecial(){
		EnemyBehaviour enemy = enemiesAttacking [0].GetComponent<EnemyBehaviour> ();
		GameObject attackObject = Instantiate (prefabAttack, enemiesAttacking [0].transform.position, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateSpecial);
		float attack = enemy.Life - specialValue + enemy.Defense;
		enemy.Life = attack;
		enemy.IsSelected = false;
		Special = Special - specialValue;
		battleManager.mageSpecialSlider.value = Special;
		if (enemy.Life <= 0)
			battleManager.DestroyEnemy (enemy);
		else
			print ("enemy life mage special= " + enemy.Life);
	}
}
