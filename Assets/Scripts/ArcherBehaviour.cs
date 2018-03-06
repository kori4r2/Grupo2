using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : PlayerBehaviour {

	void Start () {
        specialValue = 24f;
        stateAttack = 4;
		stateSpecial = 4;
		attackValue = 24f;
		defense = 17f;
	}

    // Attack all enemies
	public override void SpecialCommand(List<GameObject> enemies){
		enemiesAttacking = enemies;
		State = STATE.ONSPECIAL;
		Special -= specialValue;
		battleManager.archerSpecialSlider.value = Special;
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
		for (i = 0; i < enemiesAttacking.Count; i++) {
			enemy = enemiesAttacking [i].GetComponent<EnemyBehaviour> ();
            // Instantiate arrow on the enemy being hit
			Vector3 vec = new Vector3 (enemiesAttacking [i].transform.position.x, enemiesAttacking [i].transform.position.y + 0.5f, enemiesAttacking [i].transform.position.z - 1f);
			attackObject = Instantiate (prefabAttack, vec, Quaternion.identity);
			attackObject.GetComponent<Animator> ().SetInteger ("State", stateSpecial);
            // Deal damage
            enemy.TakeDamage(specialValue);
            enemy.IsSelected = false;
		}
        enemiesAttacking.Clear();
	}
}
