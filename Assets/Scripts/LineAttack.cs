﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttack : MonoBehaviour {

    public bool piercing = false;
    public float speed = 5.0f;
    public float range = 2.0f;
    public EnemyBehaviour owner = null;
    private Vector2 startingPosition;
    private Rigidbody2D rigid;
    private BattleManager battleManager;

    private void Start() {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0f, 0f);
        startingPosition = gameObject.transform.position;
    }

    private void Update() {
        if (owner != null) {
            if(rigid.velocity.y == 0)
                rigid.velocity = new Vector2(0f, (-1) * speed);
            if (gameObject.transform.position.y <= (startingPosition.y - range)) {
                owner.AttackObject = null;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            PlayerBehaviour player = coll.GetComponent<PlayerBehaviour>();
            if (player.Name == "Guerreiro" && player.State == PlayerBehaviour.STATE.SPECIAL) {
                WarriorBehaviour warrior = (WarriorBehaviour)player;
                warrior.FinishSpecial();
                // Stunning ogres is currently disabled
                //owner.State = EnemyBehaviour.STATE.FROZEN;
                if (!piercing) {
                    owner.AttackObject = null;
                    GameObject.Destroy(gameObject);
                }
            }else {
                player.TakeDamage(owner.AttackValue);
            }
        }
    }
}
