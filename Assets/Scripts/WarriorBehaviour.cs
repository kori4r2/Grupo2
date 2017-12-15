using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : PlayerBehaviour {

	private float specialValue = 10f;

	void Start () {
		stateAttack = 2;

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
	// Use this for initialization

	public override void SpecialCommand (List<GameObject> enemies){
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
		int i;
		EnemyBehaviour enemy;
		attackObject = Instantiate (prefabAttack, enemiesAttacking [0].transform.position, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateAttack);
        enemy = enemiesAttacking[0].GetComponent<EnemyBehaviour>();
        enemy.TakeDamage(attackValue);
        enemy.IsSelected = false;
        enemiesAttacking.Clear();
    }
}
