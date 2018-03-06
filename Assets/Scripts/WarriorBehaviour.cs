using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : PlayerBehaviour {


	void Start () {
		stateAttack = 2;
        specialValue = 10f;
        attackValue = 20f;
		defense = 25f;
	}

	public float SpecialValue{
		get{
			return specialValue;
		}
		set{
			specialValue = value;
		}
	}

	public override void SpecialCommand (List<GameObject> enemies){
        state = STATE.SPECIAL;
        anim.SetInteger("State", 2);
        Special -= SpecialValue;
	}
			
	public override string Name{
		get{ return "Guerreiro";}
		set{}
	}

	public override void FinishSpecial (){
	}

	public void ShieldAnim(){
		GameObject defenseObject = Instantiate (prefabAttack, transform.position, Quaternion.identity);
		defenseObject.GetComponent<Animator> ().SetInteger ("State", 5);
	}
		

	public void FinishAttack(){
		GameObject attackObject;
		EnemyBehaviour enemy;
        // Instantiate attack effect on top of enemy
		attackObject = Instantiate (prefabAttack, enemiesAttacking [0].transform.position, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateAttack);
        enemy = enemiesAttacking[0].GetComponent<EnemyBehaviour>();
        // Deal damage
        enemy.TakeDamage(attackValue);
        enemy.IsSelected = false;
        enemiesAttacking.Clear();
    }
}
