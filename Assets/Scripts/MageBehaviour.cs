using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : PlayerBehaviour {
		
	public float specialValue = 40f;


	void Start () {
		stateAttack = 3;
		stateSpecial = 1;
		attackValue = 18f;
		defense = 0f;
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

    public void StartSpecial() {
        GameObject attackObject = Instantiate(prefabAttack, enemiesAttacking[0].transform.position, Quaternion.identity);
        attackObject.GetComponent<Animator>().SetInteger("State", stateSpecial);
    }

	public override void FinishSpecial(){
        EnemyBehaviour enemy = enemiesAttacking [0].GetComponent<EnemyBehaviour> ();
        enemy.TakeDamage(specialValue);
		enemy.IsSelected = false;
		Special = Special - specialValue;
		battleManager.mageSpecialSlider.value = Special;
        enemiesAttacking.Clear();
    }

	public void FinishAttack(){
		GameObject attackObject;
		int i;
		EnemyBehaviour enemy;
		attackObject = Instantiate (prefabAttack, enemiesAttacking [0].transform.position, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateAttack);
		enemy = enemiesAttacking [0].GetComponent<EnemyBehaviour> ();
        enemy.TakeDamage(attackValue);
		enemy.IsSelected = false;
		enemiesAttacking.Clear ();
	}
}
