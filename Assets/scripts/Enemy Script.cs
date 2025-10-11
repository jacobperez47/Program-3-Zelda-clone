using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    idle,
    walk,
    attack,
    stagger,
}

public class EnemyScript : MonoBehaviour
{
    private float health;

    public FloatValue maxHealth;

    public string enemyName;

    public int baseAttack;

    public EnemyStates currentState;

    public CircleCollider2D vision;

    public CircleCollider2D attack;

    public Collider2D attackHitboxes;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        health = maxHealth.initialValue;

    }

    public IEnumerator KnockbackCoroutine(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        yield return new WaitForSeconds(knockTime);

        // Reset velocity and state
        if (myRigidBody != null)
        {
            myRigidBody.velocity = Vector2.zero;
        }

        currentState = EnemyStates.idle;
    }

    private void takeDamage(float damageToTake)
    {
        health -= damageToTake;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Knock(Rigidbody2D rigidBody, Vector2 finalKnockVelocity, float knockTime, float damage)
    {
        if (rigidBody == null) return;

        currentState = EnemyStates.stagger;
        rigidBody.velocity = finalKnockVelocity;

        // Start the coroutine for the delayed reset
        StopCoroutine("KnockbackCoroutine");
        StartCoroutine(KnockbackCoroutine(rigidBody, knockTime, damage));
        takeDamage(damage);
    }

    public bool IsSensor(Collider2D otherCollider)
    {
        // Check if the collider being hit is one of the sensor colliders
        return otherCollider == vision || otherCollider == attack;
    }
}