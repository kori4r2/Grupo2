using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : PlayerBehaviour {
		
    
	void Start () {
        specialValue = 40f;
        specialCost = 15f;
        stateAttack = 3;
		stateSpecial = 1;
		attackValue = 18f;
		defense = 0f;
	}

	public override void SpecialCommand(List<GameObject> enemies){
		enemiesAttacking = enemies;
		state = STATE.ONSPECIAL;
		anim.SetInteger ("State", 2);
	}

	public override string Name{
		get{ return "Mago";}
		set{}
	}

    public void StartSpecial() {
        // Instantiate magic cast on top of enemy
        GameObject attackObject = Instantiate(prefabAttack, enemiesAttacking[0].transform.position, Quaternion.identity);
        attackObject.GetComponent<Animator>().SetInteger("State", stateSpecial);
    }

	public override void FinishSpecial(){
        EnemyBehaviour enemy = enemiesAttacking [0].GetComponent<EnemyBehaviour> ();
        // Deal damage
        enemy.TakeDamage(specialValue);
		enemy.IsSelected = false;
        // Decrement special bar
		Special -= specialCost;
        enemiesAttacking.Clear();
    }

	public void FinishAttack(){
		GameObject attackObject;
		EnemyBehaviour enemy;
        // Instantiate attack effect on top of enemy
		attackObject = Instantiate (prefabAttack, enemiesAttacking [0].transform.position, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateAttack);
		enemy = enemiesAttacking [0].GetComponent<EnemyBehaviour> ();
        // Deal damage to enemy
        enemy.TakeDamage(attackValue);
		enemy.IsSelected = false;
		enemiesAttacking.Clear ();
	}
}
