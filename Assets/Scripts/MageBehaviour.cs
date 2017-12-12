using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : PlayerBehaviour {
		
	public float specialValue = 20f;

	void Start () {
	}

	public override void SpecialCommand(List<GameObject> enemies){
		EnemyBehaviour enemy = enemies [0].GetComponent<EnemyBehaviour> ();
		state = STATE.ONSPECIAL;
		anim.SetInteger ("State", 2);
		float attack = enemy.Life - specialValue + enemy.Defense;
		enemy.Life = attack;
		enemy.IsSelected = false;
		Special = Special - specialValue;
		battleManager.mageSpecialSlider.value = Special;
		if (enemy.Life <= 0)
			battleManager.DestroyEnemy (enemy);
		else
			print ("enemy life = " + enemy.Life);
	}

	public override string Name{
		get{ return "Mago";}
		set{}
	}
}
