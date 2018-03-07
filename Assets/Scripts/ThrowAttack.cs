using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : MonoBehaviour {
    
    public EnemyBehaviour owner = null;
    public float fallTime = 1.0f;
    private List<PlayerBehaviour> targets;
    private BattleManager battleManager;
    private float time = 0f;
    private int animStage = 0;

    private void Start() {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        GetComponent<Collider2D>().enabled = false;
        targets = new List<PlayerBehaviour>();
        time = 0f;
        animStage = 0;
    }

    private void Update() {
        if (owner != null && time < fallTime) {
            time += Time.deltaTime;
            switch (animStage) {
                case 0:
                    if (time >= 0.5 * fallTime) {
                        EnableCollider();
                        animStage = 1;
                    }
                    break;
                case 1:
                    if (time >= 0.9 * fallTime) {
                        time = fallTime;
                        DealDamage();
                    }
                    break;
            }
        }
    }

    public void EnableCollider() {
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player" && !targets.Contains(coll.gameObject.GetComponent<PlayerBehaviour>())) {
            targets.Add(coll.GetComponent<PlayerBehaviour>());
        }
    }

    public void DealDamage() {
        bool success = true;
        foreach (PlayerBehaviour player in targets) {
            if(player.Name == "Guerreiro" && player.State == PlayerBehaviour.STATE.SPECIAL) {
                WarriorBehaviour warrior = (WarriorBehaviour)player;
                warrior.FinishSpecial();
                // Stunning ogres is currently disabled
                //owner.State = EnemyBehaviour.STATE.FROZEN;
                success = false;
                break;
            }
        }
        if (success) {
            foreach (PlayerBehaviour player in targets) {
                player.TakeDamage(owner.AttackValue);
            }
        }
        owner.AttackObject = null;
        GameObject.Destroy(gameObject);
    }
}
