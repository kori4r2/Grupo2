using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : PlayerBehaviour {

	public float specialValue = 20f;

	public void Special(EnemyBehaviour enemy){
		state = STATE.ONSPECIAL;
		anim.SetInteger ("State", 2);
		float attack = enemy.Life - specialValue + enemy.Defense;
		enemy.Life = attack;
		print ("enemy life = " + enemy.Life);
	}

}
