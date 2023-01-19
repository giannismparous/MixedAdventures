using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LakituSpinyAttack : MonoBehaviour
{

    private float distance;
    private float attackTimer;
    private System.Random rand;
    [SerializeField] private LakituMove lakituMove;
    [SerializeField] private float attackTimerValue;

    void Start() {
        rand = new System.Random();
        attackTimer = (float)(attackTimerValue + rand.NextDouble() * 10 - 5);
    }

    void Update()
    {
        if (lakituMove.IsActive()){
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0 && !lakituMove.GetIsFallingAttacking()) {
                lakituMove.SpinyAttack();
                attackTimer = (float)(attackTimerValue + rand.NextDouble() * 10 - 5);
            }
        }
    }
}
