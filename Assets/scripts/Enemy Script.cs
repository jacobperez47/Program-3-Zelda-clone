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
    public int health;

    public string enemyName;

    public int baseAttack;

    public EnemyStates currentState;

    public void Knock(Rigidbody2D rigidBody, Vector2 finalKnockVelocity, float knockTime)
    {
        // Safety check
        if (rigidBody == null) return;

        // Force application and state setting (instantaneous)
        currentState = EnemyStates.stagger; 
        rigidBody.velocity = finalKnockVelocity;
        
        // Start the coroutine for the delayed reset
        StopCoroutine("KnockbackCoroutine"); 
        StartCoroutine(KnockbackCoroutine(rigidBody, knockTime));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator KnockbackCoroutine(Rigidbody2D myRigidBody, float knockTime)
    {
        yield return new WaitForSeconds(knockTime); 

        // Reset velocity and state
        if (myRigidBody != null)
        {
            myRigidBody.velocity = Vector2.zero;
        }
        currentState = EnemyStates.idle;
    }
}