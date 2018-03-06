using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : PlayerBehaviour {

	void Start () {
        specialValue = 24f;
        specialCost = 10f;
        stateAttack = 4;
		stateSpecial = 4;
		attackValue = 24f;
		defense = 17f;
	}

    // Attack all enemies
	public override void SpecialCommand(List<GameObject> enemies){
		enemiesAttacking = enemies;
		State = STATE.ONSPECIAL;
		anim.SetInteger ("State", 2);
	}

    // Get class name
	public override string Name{
		get{ return "Arqueiro";}
		set{}
	}

    // 
	public void FinishAttack(){
		GameObject attackObject;
		EnemyBehaviour enemy;
        // Instantiate arrow on the enemy being hit
		Vector3 vec = new Vector3 (enemiesAttacking [0].transform.position.x, enemiesAttacking [0].transform.position.y + 0.5f, enemiesAttacking [0].transform.position.z - 1f);
		attackObject = Instantiate (prefabAttack, vec, Quaternion.identity);
		attackObject.GetComponent<Animator> ().SetInteger ("State", stateAttack);
        enemy = enemiesAttacking[0].GetComponent<EnemyBehaviour>();
        // Deal damage
        enemy.TakeDamage(attackValue);
        enemy.IsSelected = false;
        enemiesAttacking.Clear();
    }

	public override void FinishSpecial(){
		int i;
		EnemyBehaviour enemy;
		GameObject attackObject;
        // For every enemy on the field
		foreach (GameObject enemyObject in enemiesAttacking) {
			enemy = enemyObject.GetComponent<EnemyBehaviour> ();
            // Instantiate arrow on the enemy being hit
			Vector3 vec = new Vector3 (enemyObject.transform.position.x, enemyObject.transform.position.y + 0.4f, enemyObject.transform.position.z - 1f);
			attackObject = Instantiate (prefabAttack, vec, Quaternion.identity);
			attackObject.GetComponent<Animator> ().SetInteger ("State", stateSpecial);
            // Deal damage
            enemy.TakeDamage(specialValue);
            enemy.IsSelected = false;
        }
        Special -= specialCost;
        enemiesAttacking.Clear();
	}
}
