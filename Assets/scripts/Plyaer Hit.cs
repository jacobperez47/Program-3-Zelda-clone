using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : knockback
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("breakable"))
        {
            other.GetComponent<PotScript>().smash();
        }

        if (other.CompareTag("enemy"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            EnemyScript enemy = other.GetComponent<EnemyScript>();


            if (hit != null && enemy != null)
            {
                if (enemy.currentState != EnemyStates.stagger)
                {
                    Vector2 difference = hit.transform.position - transform.position;
                    Vector2 force = difference.normalized * thrust;
                    Debug.Log("enemy knocked back");
                    enemy.currentState = EnemyStates.stagger;
                    enemy.Knock(hit, force, .2f);
                }
            }
        }
    }
}